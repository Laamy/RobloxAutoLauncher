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

            string robloxFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + "\\Roblox\\Versions";
            string robloxPFPath = "C:\\Program Files (x86)\\Roblox\\Versions"; // some people have other folder so this fixes it ig

            WebClient wc = new WebClient();

            RobloxClient.Process.version = RobloxAPI.GetVersion();

            List<string> folders = new List<string>();

            if (Directory.Exists(robloxPFPath))
                folders.AddRange(Directory.GetDirectories(robloxPFPath));

            if (Directory.Exists(robloxFolder))
                folders.AddRange(Directory.GetDirectories(robloxFolder));

            float increaseBy = folders.Count == 0 ? 100 / folders.Count : 100;

            foreach (string version in folders.ToArray()) // this is so lazy
            {
                Directory.Delete(version, true);
                processBarControl1.SetProgress(processBarControl1.GetProgress() + (int)increaseBy);
            }

            RobloxClient.UpdateRoblox(); // this is a massive flaw..

            processBarControl1.SetProgress(100);

            MessageBox.Show("RobloxAutoLauncher to finish repair", "RobloxAL Installer");
        }

        private void UninstallApp(object sender, EventArgs e) // this one is instant so it doesnt really matter
        {
            processBarControl1.SetProgress(0);

            var robloxVersions = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\Versions");

            Program.config.Write("RequiresReinstall", "1", "System");

            string robloxFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + "\\Roblox\\Versions";
            string robloxPFPath = "C:\\Program Files (x86)\\Roblox\\Versions"; // some people have other folder so this fixes it ig

            WebClient wc = new WebClient();

            RobloxClient.Process.version = RobloxAPI.GetVersion();

            string robloxPath = "";

            if (!Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version) && !Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
            {
                Program.config.Write("RequiresReinstall", "1", "System");

                MessageBox.Show("Latest roblox version not detected (FATAL FAILURE)", "RobloxAL");
                return;
            }
            else
            {
                if (Directory.Exists(robloxPFPath + "\\" + RobloxClient.Process.version))
                    robloxPath = robloxPFPath + "\\" + RobloxClient.Process.version;

                if (Directory.Exists(robloxFolder + "\\" + RobloxClient.Process.version))
                    robloxPath = robloxFolder + "\\" + RobloxClient.Process.version;
            }

            RobloxClient.Process.ReplaceRoblox(robloxPath + "\\RobloxPlayerLauncher.exe");

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
