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

        // my C# is corrupt so this isn't needed..
        static void DisplayError(Exception ex)
        {
            var stackTrace = new System.Diagnostics.StackTrace(ex, true);
            var frame = stackTrace.GetFrame(0);
            var lineNumber = frame.GetFileLineNumber();
            var fileName = frame.GetFileName();

            MessageBox.Show($"Exception: {ex.Message}\n" +
                            $"Source: {ex.Source}\n" +
                            $"File: {fileName}\n" +
                            $"Line: {lineNumber}\n" +
                            $"StackTrace: {ex.StackTrace}", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [STAThread]
        static void Main(string[] cmdLineArgs)
        {
            args = cmdLineArgs;

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var exception = (Exception)e.ExceptionObject;
                DisplayError(exception);
            };

            if (args.Length == 0)
            {
                Application.Run(new InstallerWindow());
            }
            else
            {
                if (args[0] == "--reinstall")
                    ReinstallRoblox();

                la = Launcher.ParseArgs(args[0]);
                Task.Factory.StartNew(() => Application.Run(new LauncherWindow()));

                string robloxPath = RobloxClient.GetRobloxVersionPath();
                if (string.IsNullOrEmpty(robloxPath))
                {
                    LauncherWindow.Window.VersionInvalid();
                    Thread.Sleep(-1);
                }
                else
                {
                    CheckReinstallRequired();
                    LauncherWindow.Window.VersionValid();
                    Launchable = true;

                    // NOTE: should probably put this on another thread..
                    string placeId = HttpUtility.UrlDecode(la.PlaceLauncherUrl).Split('&')[2].Split('=')[1];
                    RobloxClient.Process.curPlace = RobloxAPI.GetMainUniverse(placeId);
                    
                    Thread.Sleep(-1); // pause app
                }
            }
        }

        public static bool Launchable = false;

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

        public static void StartRoblox()
        {
            string robloxPath = RobloxClient.GetRobloxVersionPath();

            RobloxClient.InitMutex(); // legacy
            Task.Factory.StartNew(() =>
            {
                RobloxClient.Process.roblox = Process.Start(robloxPath + "\\RobloxPlayerBeta.exe", args[0]);
            });
        }
    }
}