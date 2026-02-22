using System.Windows;
using QuickGridLauncher.Windows;

namespace QuickGridLauncher
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var overlay = new OverlayWindow();

            // Force handle creation
            overlay.Show();
            overlay.Hide();
        }
    }
}