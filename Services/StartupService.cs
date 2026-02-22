using Microsoft.Win32;
using System;
using System.Reflection;

namespace QuickGridLauncher.Services
{
    public static class StartupService
    {
        private const string AppName = "QuickGridLauncher";
        private const string RegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static void SetStartup(bool enabled)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);

                if (key == null)
                    return;

                if (enabled)
                {
                    var exePath = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                    key.SetValue(AppName, $"\"{exePath}\"");
                }
                else
                {
                    key.DeleteValue(AppName, false);
                }
            }
            catch
            {
                // Silently fail if registry access denied
            }
        }

        public static bool IsStartupEnabled()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, false);
                return key?.GetValue(AppName) != null;
            }
            catch
            {
                return false;
            }
        }
    }
}