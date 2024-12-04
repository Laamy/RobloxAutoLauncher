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
using System.CodeDom;

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
        static void Main(string[] argsc)
        {
            args = argsc; // store globally for if we need to relaunch..

            //Application.ThreadException += (sender, e) =>
            //{
            //    MessageBox.Show(e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //};
            //
            //AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            //{
            //    if (e.ExceptionObject is Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //};

            if (args.Length == 0)
            {
                Application.Run(new InstallerWindow());
            }
            else
            {
                if (args[0] == "--reinstall")
                {
                    // TODO: make more advanced..
                    RobloxClient.Process.version = RobloxAPI.GetVersion();
                    RobloxClient.UpdateRoblox();

                    while (Process.GetProcessesByName("RobloxPlayerLauncher").Length == 0) { Thread.Sleep(1); }

                    while (true)
                    {
                        Thread.Sleep(1);

                        if (Process.GetProcessesByName("RobloxPlayerLauncher").Length == 0)
                        {

                            RobloxClient.Process.ReplaceRoblox();
                            Program.config.Write("RequiresReinstall", "0", "System"); // reset reinstall

                            //MessageBox.Show("Reinstall successful, try now!", "RobloxAL Installer");
                            args = new string[] { args[1] };

                            break;
                        }
                    }
                }

                la = Launcher.ParseArgs(args[0]);

                Task.Factory.StartNew(() => Application.Run(new LauncherWindow()));

                // https://setup.rbxcdn.com/version
                // TODO: combine this into a util..
                string robloxFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) // otherwise its put here..
                    + "\\Roblox\\Versions";
                string robloxPFPath = "C:\\Program Files (x86)\\Roblox\\Versions"; // when roblox is run as admin this is always wheree it goes

                RobloxClient.Process.version = RobloxAPI.GetVersion();

                string robloxPath = "";

                if (!Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version) && !Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
                {
                    config.Write("RequiresReinstall", "1", "System");

                    //Task.Factory.StartNew(() => Application.Run(new InstallerWindow(true)));
                    LauncherWindow.Window.VersionInvalid();
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
                                MessageBox.Show("Roblox cant start due to needing a reinstall", "RobloxAL");
                                RobloxClient.ExitApp();
                            }
                        }
                    }

                    if (Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
                        robloxPath = robloxPFPath + "\\" + RobloxClient.Process.version;

                    if (Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version))
                        robloxPath = robloxFolder + "\\" + RobloxClient.Process.version;
                }

                LauncherWindow.Window.VersionValid();

                string placeId = HttpUtility.UrlDecode(la.PlaceLauncherUrl).Split('&')[2].Split('=')[1];

                RobloxClient.Process.curPlace = RobloxAPI.GetMainUniverse(placeId);

                Thread.Sleep(-1); // pause console
            }
        }
    }
}
