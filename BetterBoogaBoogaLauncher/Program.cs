#region Imports

using RobloxAutoLauncher.RobloxSDK;

using System;
using System.Diagnostics;
using System.IO;
using System.MDI;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

#endregion

namespace RobloxAutoLauncher
{
    internal class Program
    {
        public static LauncherArgs la;
        public static MDIInIFile config = new MDIInIFile();

        public static bool CheckAdminPerms() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        public static string[] args;

        [STAThread]
        static void Main(string[] cmdLineArgs)
        {
            args = cmdLineArgs;

            if (args.Length == 0)
            {
                Application.Run(new InstallerWindow());
            }
            else
            {
                if (args[0] == "--reinstall")
                {
                    ReinstallRoblox();
                }

                la = Launcher.ParseArgs(args[0]);
                Task.Factory.StartNew(() => Application.Run(new LauncherWindow()));

                string robloxPath = GetRobloxPath();
                if (string.IsNullOrEmpty(robloxPath))
                {
                    LauncherWindow.Window.VersionInvalid();
                    Thread.Sleep(-1);
                }
                else
                {
                    CheckReinstallRequired();
                    LauncherWindow.Window.VersionValid();

                    string placeId = HttpUtility.UrlDecode(la.PlaceLauncherUrl).Split('&')[2].Split('=')[1];
                    RobloxClient.Process.curPlace = RobloxAPI.GetMainUniverse(placeId);

                    RobloxClient.InitMutex();
                    StartRoblox(robloxPath);
                    Thread.Sleep(-1); // pause app
                }
            }
        }

        static void ReinstallRoblox()
        {
            RobloxClient.Process.version = RobloxAPI.GetVersion();
            RobloxClient.UpdateRoblox();

            while (Process.GetProcessesByName("RobloxPlayerLauncher").Length == 0) { Thread.Sleep(1); }

            while (true)
            {
                Thread.Sleep(1);

                if (Process.GetProcessesByName("RobloxPlayerLauncher").Length == 0)
                {
                    RobloxClient.Process.ReplaceRoblox();
                    config.Write("RequiresReinstall", "0", "System");
                    args = new string[] { args[1] };
                    break;
                }
            }
        }

        static string GetRobloxPath()
        {
            string robloxFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\Versions";
            string robloxPFPath = "C:\\Program Files (x86)\\Roblox\\Versions";

            RobloxClient.Process.version = RobloxAPI.GetVersion();

            if (!Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version) && !Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
            {
                config.Write("RequiresReinstall", "1", "System");
                return null;
            }

            string robloxPath = "";
            if (Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
                robloxPath = robloxPFPath + "\\" + RobloxClient.Process.version;

            if (Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version))
                robloxPath = robloxFolder + "\\" + RobloxClient.Process.version;

            return robloxPath;
        }

        static void CheckReinstallRequired()
        {
            if (File.Exists(MDI.mdiBase + "config.ini"))
            {
                if (config.KeyExists("RequiresReinstall", "System")
                    && config.Read("RequiresReinstall", "System") != "0")
                {
                    if (!CheckAdminPerms())
                    {
                        MessageBox.Show("Roblox cant start due to needing a reinstall", "RobloxAL");
                        RobloxClient.ExitApp();
                    }
                }
            }
        }

        static void StartRoblox(string robloxPath)
        {
            Task.Factory.StartNew(() =>
            {
                RobloxClient.Process.roblox = Process.Start(robloxPath + "\\RobloxPlayerBeta.exe", args[0]);
            });
        }
    }
}