using System.Collections.Generic;

namespace QuickGridLauncher.Models
{
    public class AppConfig
    {
        public int Columns { get; set; } = 4;
        public string BackgroundColor { get; set; } = "#CC1E1E1E";
        public double BackgroundOpacity { get; set; } = 0.8; // 80% opacity
        public string HighlightColor { get; set; } = "#7A3CFF";
        public List<AppEntry> Apps { get; set; } = new();

        // Hotkey settings (defaults: Alt + Shift + Space)
        public int HotkeyModifiers { get; set; } = 0x0001 | 0x0002; // MOD_ALT | MOD_SHIFT
        public int HotkeyKey { get; set; } = 0x20; // VK_SPACE

        // Startup settings
        public bool RunOnStartup { get; set; } = false;
    }
}