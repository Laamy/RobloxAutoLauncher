using RobloxAutoLauncher.RobloxSDK;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace RobloxAutoLauncher
{
    partial class InstallerWindow : Form
    {
        bool hasToRepair = false;

        public InstallerWindow(bool reinstall = false)
        {
            hasToRepair = reinstall;
            InitializeComponent();
        }

        private void InstallApp(object sender, EventArgs e) // this one is instant so it doesnt really matter
        {
            processBarControl1.SetProgress(0);

            RobloxClient.Process.ReplaceRoblox();
            Program.config.Write("RequiresReinstall", "0", "System"); // reset reinstall

            processBarControl1.SetProgress(100);

            MessageBox.Show("Installed", "RobloxAL Installer");
        }

        private void RepairApp(object sender, EventArgs e)
        {
            processBarControl1.SetProgress(0);
            Program.config.Write("RequiresReinstall", "1", "System"); // force reinstall

            List<string> folders = RobloxClient.GetRobloxVersionFolders();
            float increaseBy = folders.Count == 0 ? 100 : 100 / folders.Count;

            foreach (string version in folders.ToArray())
            {
                Directory.Delete(version, true);
                processBarControl1.SetProgress(processBarControl1.GetProgress() + (int)increaseBy);
            }

            RobloxClient.UpdateRoblox();
            processBarControl1.SetProgress(100);
            MessageBox.Show("RobloxAutoLauncher to finish repair", "RobloxAL Installer");
        }

        private void UninstallApp(object sender, EventArgs e) // this one is instant so it doesnt really matter
        {
            processBarControl1.SetProgress(0);
            Program.config.Write("RequiresReinstall", "1", "System");

            string versionPath = RobloxClient.GetRobloxVersionPath();

            if (string.IsNullOrEmpty(versionPath))
            {
                MessageBox.Show("Latest roblox version not detected (FATAL FAILURE)", "RobloxAL");
                return;
            }

            RobloxClient.Process.ReplaceRoblox(versionPath + "\\RobloxPlayerLauncher.exe");
            processBarControl1.SetProgress(100);
            MessageBox.Show("Uninstalled", "RobloxAutoLauncher Installer");
        }

        private void InstallerWindow_FormClosing(object sender, FormClosingEventArgs e)
            => RobloxClient.ExitApp();

        private void InstallerWindow_Load(object sender, EventArgs e)
        {
            if (hasToRepair)
            {
                RobloxClient.UpdateRoblox();
                MessageBox.Show("Reinstall RobloxAutoLauncher to update roblox");
            }

            if (!Program.CheckAdminPerms())
            {
                smoothButton1.Enabled = false;
                smoothButton2.Enabled = false;
                smoothButton3.Enabled = false;

                Text += " (Requires Administrator)";
            }
        }
    }
}
