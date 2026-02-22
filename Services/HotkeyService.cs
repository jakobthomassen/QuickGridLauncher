using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Keys = System.Windows.Forms.Keys;

namespace QuickGridLauncher.Services
{
    public class HotkeyService
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;

        private readonly int _id = 9000;

        public event Action? OnHotkeyPressed;

        public HotkeyService(Window window)
        {
            window.SourceInitialized += (_, _) =>
            {
                var source = (HwndSource)PresentationSource.FromVisual(window)!;
                source.AddHook(HwndHook);

                bool result = RegisterHotKey(
                    source.Handle,
                    _id,
                    MOD_CONTROL | MOD_SHIFT,
                    (uint)Keys.Space);

                if (!result)
                    MessageBox.Show("Hotkey registration FAILED");
                else
                    MessageBox.Show("Hotkey registration succeeded");
            };
        }

        private IntPtr HwndHook(
        IntPtr hwnd,
        int msg,
        IntPtr wParam,
        IntPtr lParam,
        ref bool handled)
            {
                if (msg == WM_HOTKEY && wParam.ToInt32() == _id)
                {
                    OnHotkeyPressed?.Invoke();
                    handled = true;
                }

                return IntPtr.Zero;
            }
    }
}