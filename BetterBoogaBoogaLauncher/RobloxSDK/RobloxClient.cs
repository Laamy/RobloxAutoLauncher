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
            RobloxAPI.DownloadRobloxInstaller(Process.version, destination);
            System.Diagnostics.Process.Start(destination);
        }
    }
}
