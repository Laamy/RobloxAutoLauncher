using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.MDI;
using System.Net;
using System.Threading;

namespace RobloxAutoLauncher.RobloxSDK
{
    public class RobloxClient
    {
        public static Mutex robloxMutex;
        public static RobloxProcess Process = new RobloxProcess();

        private static WebClient wc = new WebClient();

        public static void InitMutex()
        {
            robloxMutex = new Mutex(true, "ROBLOX_singletonMutex");
        }

        public static void UninitMutex()
        {
            if (robloxMutex != null)
            {
                robloxMutex.Close();
                robloxMutex.Dispose();
            }
        }

        public static void ExitApp() // this is called everytime we want to exit the application
        {
            UninitMutex();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public static void UpdateRoblox()
        {
            MDIDirectory.CheckCreate("Tmp");
            var destination = $"{MDI.mdiBase}\\Tmp\\RobloxPlayerLauncher.exe";
            RobloxAPI.DownloadRobloxInstaller(RobloxAPI.GetVersion(), destination);

            ProcessStartInfo startinfo = new ProcessStartInfo
            {
                FileName = destination,
                Verb = "runas"
            };

            System.Diagnostics.Process.Start(startinfo);
        }

        public static void InstallLauncher() => Process.ReplaceRoblox();

        public static List<string> GetRobloxVersionFolders()
        {
            string localAppDataPathh = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roblox\\Versions";
            string programFilesPath = "C:\\Program Files (x86)\\Roblox\\Versions";
            List<string> folders = new List<string>();

            if (Directory.Exists(programFilesPath))
                folders.AddRange(Directory.GetDirectories(programFilesPath));

            if (Directory.Exists(localAppDataPathh))
                folders.AddRange(Directory.GetDirectories(localAppDataPathh));

            return folders;
        }

        public static string GetRobloxVersionPath()
        {
            string localAppDataPathh = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Roblox\\Versions";
            string programFilesPath = "C:\\Program Files (x86)\\Roblox\\Versions";
            string currentVersion = RobloxAPI.GetVersion();

            if (Directory.Exists(programFilesPath))
                return $"{programFilesPath}\\{currentVersion}";

            if (Directory.Exists(localAppDataPathh))
                return $"{localAppDataPathh}\\{currentVersion}";

            return string.Empty;
        }
    }
}
