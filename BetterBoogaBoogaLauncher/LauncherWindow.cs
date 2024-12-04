using RobloxAutoLauncher.RobloxSDK;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RobloxAutoLauncher
{
    public partial class LauncherWindow : Form
    {
        public static LauncherWindow Window;

        private string loadingSuffix = "Checking Version";
        private int dots = 1;
        private string placeId = "0";

        public LauncherWindow()
        {
            Window = this;

            TopMost = true;
            InitializeComponent();
        }

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
            if (Process.GetProcesses().Any(proc => proc.MainWindowTitle == "Roblox"))
            {
                loadingSuffix = "Ready to launch";
                processBarControl1.SetProgress(100);
                Focus();

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    RobloxClient.ExitApp();
                });
            }

            if (RobloxClient.Process.curPlace != null && loadingSuffix == "Initializing")
            {
                loadingSuffix = "Waiting";
                processBarControl1.SetProgress(66);

                placeId = HttpUtility.UrlDecode(Program.la.PlaceLauncherUrl)
                    .Split('&')[2]
                    .Split('=')[1];

                switch (placeId)
                {
                    case "11729688377":
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
        }

        public static string CleanText(string input)
        {
            string pattern = @"[^a-zA-Z0-9!@#$%^&*()\[\]{} ]";
            return Regex.Replace(input, pattern, "");
        }

        private void LauncherWindow_Load(object sender, EventArgs e)
        {
            loadingSuffix = "Checking Version";
            processBarControl1.SetProgress(0);
            SuspendTimer_Tick(null, new EventArgs());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void VersionValid()
        {
            loadingSuffix = "Initializing";
            processBarControl1.SetProgress(33);
        }

        public void VersionInvalid()
        {
            label1.Text = $"Roblox Update required!";
            processBarControl1.SetProgress(50);

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
