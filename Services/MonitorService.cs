using System.Drawing;
using System.Windows.Forms;

namespace QuickGridLauncher.Services
{
    public static class MonitorService
    {
        public static Rectangle GetActiveMonitorArea()
        {
            return Screen.FromPoint(Cursor.Position).WorkingArea;
        }
    }
}