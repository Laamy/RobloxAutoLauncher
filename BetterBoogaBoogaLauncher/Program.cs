#region Imports

using RobloxAutoLauncher.RobloxSDK;

using Microsoft.Win32;

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.MDI;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

#endregion

namespace RobloxAutoLauncher
{
    internal class Program
    {
        public static LauncherArgs la;
        public static MDIInIFile config = new MDIInIFile();
        
        public static void ReplaceRoblox(string proc = null)
        {
            if (proc == null)
                proc = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;

            RegistryKey key = Registry.ClassesRoot.OpenSubKey("roblox-player\\shell\\open\\command", true);
            key.SetValue(string.Empty, "\"" + proc + "\" %1");
            key.Close();
        }

        public static bool CheckAdminPerms() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        [STAThread]
        static void Main(string[] argsc)
        {
            // fake for debugging
            string[] args =
            {
                "roblox-player:1+launchmode:play+gameinfo:M13efzCSXLtK7sgUfka4BSPHZzkGjkUDz3gp-1d9qVFHq373sHvQ7MRoyn_WC6ZPt9Isp2g2j_Xf42p3nckHbtx6LgEJtYw4EQwPKPnb8md8xmkYXr-RAVHvLAzl8OZGej0eMF2UBbbfowgdf0t_i0RHxKjNwopkargbFdG9Wh7xQ8SipjVvDrc_s6JhCL8-GquqVZdXpFnSEtlFGHS2GLAGP0D9Ct7O6amhvgjSjFY+launchtime:1733279394088+placelauncherurl:https%3A%2F%2Fwww.roblox.com%2FGame%2FPlaceLauncher.ashx%3Frequest%3DRequestPrivateGame%26browserTrackerId%3D1732781422831002%26placeId%3D11729688377%26accessCode%3D127780a6-37e2-4a22-af1c-7635ce00fbac%26joinAttemptId%3Ddd7b79a8-9360-45c8-b117-c4d551c91a95%26joinAttemptOrigin%3DprivateServerListJoin+browsertrackerid:1732781422831002+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp"
            };

            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            if (args.Length == 0)
            {
                Application.Run(new InstallerWindow());
            }
            else
            {
                la = Launcher.ParseArgs(args[0]);

                // https://setup.rbxcdn.com/version

                string robloxFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    + "\\Roblox\\Versions";
                string robloxPFPath = "C:\\Program Files (x86)\\Roblox\\Versions"; // some people have other folder so this fixes it ig

                RobloxClient.Process.version = RobloxAPI.GetVersion();

                string robloxPath = "";

                if (!Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version) && !Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
                {
                    config.Write("RequiresReinstall", "1", "System");

                    Task.Factory.StartNew(() => Application.Run(new InstallerWindow(true)));
                    Thread.Sleep(-1);
                }
                else
                {
                    if (File.Exists(MDI.mdiBase + "config.ini"))
                    {
                        if (config.KeyExists("RequiresReinstall", "System")
                            && config.Read("RequiresReinstall", "System") != "0")
                        {
                            if (!CheckAdminPerms())
                            {
                                MessageBox.Show("Roblox cant start due to needing a reinstall", "BBRB");
                                RobloxClient.ExitApp();
                            }
                        }
                    }

                    if (Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
                        robloxPath = robloxPFPath + "\\" + RobloxClient.Process.version;

                    if (Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version))
                        robloxPath = robloxFolder + "\\" + RobloxClient.Process.version;
                }

                string placeId = HttpUtility.UrlDecode(la.PlaceLauncherUrl).Split('&')[2].Split('=')[1];

                RobloxClient.Process.curPlace = RobloxAPI.GetMainUniverse(placeId);

                Task.Factory.StartNew(() => Application.Run(new LauncherWindow()));

                RobloxClient.InitMutex();

                Task.Factory.StartNew(() => {
                    RobloxClient.Process.roblox = Process.Start(robloxPath + "\\RobloxPlayerBeta.exe", args[0]);
                    // legacy launcher (pass arguments directly to roblox now..)
                    //$"--play -a https://www.roblox.com/Login/Negotiate.ashx -t {la.GameInfo}" +
                    //$" -j {HttpUtility.UrlDecode(la.PlaceLauncherUrl)} -b {la.TrackerId} --launchtime={la.LaunchTime}" +
                    //$" --rloc {la.RobloxLocale} --gloc {la.GameLocale}");
                });

                Thread.Sleep(-1); // pause console
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessRectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public ProcessRectangle(Point position, Point size) // this is most likely wrong
        {
            Left = position.X;
            Top = position.X + size.X;
            Right = position.Y;
            Bottom = position.Y + size.Y;

            // Left, Top, Right, Bottom
            // X, X - X, Y, Y - Y

            // Left, Top,
            // Right, Bottom
            // X, X - X,
            // Y, Y - Y
        }
    }
}
