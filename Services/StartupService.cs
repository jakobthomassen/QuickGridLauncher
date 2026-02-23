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
                    // Use Environment.ProcessPath for single-file apps
                    var exePath = Environment.ProcessPath;

                    if (string.IsNullOrEmpty(exePath))
                    {
                        // Fallback
                        exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
                    }

                    if (!string.IsNullOrEmpty(exePath))
                    {
                        key.SetValue(AppName, $"\"{exePath}\"");
                    }
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