using Microsoft.Win32;
using Roblox.API;
using System;
using System.Diagnostics;

namespace RobloxAutoLauncher.RobloxSDK
{
    public class RobloxProcess
    {
        public Process roblox;
        public RobloxUniverse curPlace;
        public string version;

        public void ReplaceRoblox(string proc = null)
        {
            if (proc == null)
                proc = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;

            RegistryKey key = Registry.ClassesRoot.OpenSubKey("roblox-player\\shell\\open\\command", true);
            key.SetValue(string.Empty, "\"" + proc + "\" %1");
            key.Close();
        }
    }
}
