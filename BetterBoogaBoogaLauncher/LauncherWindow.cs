using RobloxAutoLauncher.RobloxSDK;
using RobloxAutoLauncher.SDK.Jobs;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace RobloxAutoLauncher
{
    public partial class LauncherWindow : Form
    {
        public static LauncherWindow Window;

        private string loadingSuffix = "Checking Version";
        private int dots = 1;
        bool? versionValid = null;

        private string placeId = "0"; // TODO: remove this and use new sdk

        private JobManager jobManager;

        public LauncherWindow()
        {
            Window = this;

            jobManager = new JobManager((value) =>
            {
                processBarControl1.SetProgress(value);
            });

            // no clue what i fucked up w/ this
            jobManager.AddJob(new Job(() => { return versionValid == true; }, () => { loadingSuffix = "Checking Version"; }));
            jobManager.AddJob(new Job(() => { return IsRobloxRunning(); }, () => { loadingSuffix = "Waiting"; }));
            jobManager.AddJob(new Job(() => { return false; }, () => {
                loadingSuffix = "Have fun!";
            }));
            jobManager.End();

            TopMost = true;
            InitializeComponent();
        }

        public bool IsRobloxRunning() => Process.GetProcesses().Any(proc => proc.MainWindowTitle == "Roblox");

        public void CancelShutdown()
        {
            robloxTimer.Enabled = false;
            SuspendTimer.Enabled = false;
            Close();
        }

        public void InitRobloxDetectTask()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1);

                    if (Process.GetProcessesByName("RobloxPlayerBeta").Length == 0)
                    {
                        RobloxClient.ExitApp(); // No Roblox instances are open
                    }
                }
            });
        }

        private void RobloxTimer_Tick(object sender, EventArgs e)
        {
            if (IsRobloxRunning())
            {
                Focus();

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    RobloxClient.ExitApp();
                });
            }

            if (RobloxClient.Process.curPlace != null && placeId == null)
            {
                placeId = HttpUtility.UrlDecode(Program.la.PlaceLauncherUrl)
                .Split('&')[2]
                .Split('=')[1];

                switch (placeId)
                {
                    case "4483381587":
                        break;
                }
            }
        }

        public string GetGameTitle()
        {
            if (RobloxClient.Process.curPlace != null)
                return "\n" + CleanText(RobloxClient.Process.curPlace.data.First().sourceName);

            return "";
        }

        private void SuspendTimer_Tick(object sender, EventArgs e)
        {
            dots = (dots == 4) ? 0 : dots + 1;
            label1.Text = $"{loadingSuffix} {new string('.', dots)}{GetGameTitle()}";

            jobManager.TickJobs();
        }

        public static string CleanText(string input)
        {
            string pattern = @"[^a-zA-Z0-9!@#$%^&*()\[\]{} ]";
            return Regex.Replace(input, pattern, "");
        }

        private void LauncherWindow_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public void VersionValid() => versionValid = true;

        public void VersionInvalid()
        {
            versionValid = false;

            robloxTimer.Enabled = false;
            SuspendTimer.Enabled = false;

            TopMost = false;

            var startInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath,
                Verb = "runas",
                Arguments = $"--reinstall {Program.args[0]}"
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Admin privileges are required, restart installer with admin.");
            }

            RobloxClient.ExitApp();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SoundPlayer.PlayClick(); // funny easter egg
        }
    }
}
