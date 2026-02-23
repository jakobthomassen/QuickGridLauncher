using QuickGridLauncher.Helpers;
using QuickGridLauncher.Models;
using QuickGridLauncher.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace QuickGridLauncher.Windows
{
    public partial class OverlayWindow : Window
    {
        private readonly HotkeyService _hotkey;
        private readonly List<AppEntry> _apps = new();
        private int _selectedIndex = 0;
        private int _columns = 4;

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            if (IsVisible)
                FadeOut();
        }

        public OverlayWindow()
        {
            InitializeComponent();

            _hotkey = new HotkeyService(this);
            _hotkey.OnHotkeyPressed += Toggle;

            LoadConfig();
        }

        private void LoadConfig()
        {
            var config = ConfigService.Load();

            _columns = config.Columns;
            _apps.Clear();

            // Filter out missing executables silently
            var validApps = config.Apps.Where(app => System.IO.File.Exists(app.Path)).ToList();

            // If any apps were removed, save the updated config
            if (validApps.Count != config.Apps.Count)
            {
                config.Apps = validApps;
                ConfigService.Save(config);
            }

            _apps.AddRange(validApps);

            // OPTIMIZATION: Only extract icons if not already cached
            foreach (var app in _apps)
            {
                if (app.Icon == null)
                {
                    app.Icon = Helpers.IconHelper.GetIcon(app.Path);
                }
            }

            // Apply background color and opacity from config
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(config.BackgroundColor);
                // Apply the opacity from config (0.0 to 1.0)
                color.A = (byte)(config.BackgroundOpacity * 255);
                Container.Background = new SolidColorBrush(color);
            }
            catch
            {
                // Default: dark gray with 80% opacity
                Container.Background = new SolidColorBrush(Color.FromArgb(204, 30, 30, 30));
            }

            // Show/hide empty state
            if (_apps.Count == 0)
            {
                AppGrid.Visibility = Visibility.Collapsed;
                EmptyStateText.Visibility = Visibility.Visible;
            }
            else
            {
                AppGrid.Visibility = Visibility.Visible;
                EmptyStateText.Visibility = Visibility.Collapsed;
            }

            _selectedIndex = 0;
            UpdateSelection();
            RenderGrid();
        }

        private void RenderGrid()
        {
            // OPTIMIZATION: Removed unnecessary null assignment
            AppGrid.ItemsSource = _apps;
        }

        private void Toggle()
        {
            if (IsVisible)
                FadeOut();
            else
                ShowOverlay();
        }

        private void ShowOverlay()
        {
            var area = MonitorService.GetActiveMonitorArea();

            Opacity = 0;
            ContainerScale.ScaleX = 0.97;
            ContainerScale.ScaleY = 0.97;

            Show();
            UpdateLayout();

            Left = area.Left + (area.Width - ActualWidth) / 2;
            Top = area.Top + (area.Height - ActualHeight) / 2;

            Activate();
            Focus();
            Keyboard.Focus(this);

            // Fade in opacity
            var fade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(120));
            BeginAnimation(OpacityProperty, fade);

            // Scale in container (subtle zoom)
            var scaleAnim = new DoubleAnimation(0.97, 1.0, TimeSpan.FromMilliseconds(120))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            ContainerScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            ContainerScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }

        private void FadeOut()
        {
            var fade = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));
            fade.Completed += (_, _) => Hide();
            BeginAnimation(OpacityProperty, fade);

            // Scale out container (subtle zoom out)
            var scaleAnim = new DoubleAnimation(1.0, 0.97, TimeSpan.FromMilliseconds(100))
            {
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            ContainerScale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            ContainerScale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnim);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            int count = _apps.Count;
            int rows = (count + _columns - 1) / _columns;
            int currentRow = _selectedIndex / _columns;
            int currentCol = _selectedIndex % _columns;

            switch (e.Key)
            {
                case Key.W:
                    currentRow--;
                    if (currentRow < 0)
                        currentRow = rows - 1; // Wrap to bottom
                    _selectedIndex = Math.Min(currentRow * _columns + currentCol, count - 1);
                    break;

                case Key.S:
                    currentRow++;
                    if (currentRow >= rows)
                        currentRow = 0; // Wrap to top
                    _selectedIndex = Math.Min(currentRow * _columns + currentCol, count - 1);
                    break;

                case Key.A:
                    _selectedIndex--;
                    if (_selectedIndex < 0)
                        _selectedIndex = count - 1; // Wrap to end
                    break;

                case Key.D:
                    _selectedIndex++;
                    if (_selectedIndex >= count)
                        _selectedIndex = 0; // Wrap to start
                    break;

                case Key.Space:
                case Key.Enter:
                    LaunchSelected();
                    return;

                case Key.Escape:
                    FadeOut();
                    return;

                case Key.Q:
                    {
                        try
                        {
                            Hide();   // immediate, no animation

                            var config = ConfigService.Load();
                            var settings = new SettingsWindow(config);

                            if (settings.ShowDialog() == true)
                            {
                                // Reload configuration after settings save
                                LoadConfig();

                                // Offer to restart app for hotkey/column changes
                                var result = MessageBox.Show(
                                    "Some settings (hotkey, columns) require restarting QuickGridLauncher to take effect.\n\n" +
                                    "Would you like to restart the application now?",
                                    "Restart Application?",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    RestartApplication();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error opening settings: {ex.Message}\n\n{ex.StackTrace}",
                                "Settings Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        return;
                    }
            }

            UpdateSelection();
        }

        private void UpdateSelection()
        {
            for (int i = 0; i < _apps.Count; i++)
            {
                bool selected = i == _selectedIndex;

                _apps[i].IsSelected = selected;
                _apps[i].Scale = selected ? 1.08 : 1.0;
            }
        }

        private void LaunchSelected()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _apps.Count)
                return;

            var selectedApp = _apps[_selectedIndex];

            // Check if executable still exists
            if (!System.IO.File.Exists(selectedApp.Path))
            {
                // Remove the missing app and save config
                _apps.RemoveAt(_selectedIndex);
                var config = ConfigService.Load();
                config.Apps = _apps.ToList();
                ConfigService.Save(config);

                // Adjust selection
                if (_selectedIndex >= _apps.Count)
                    _selectedIndex = Math.Max(0, _apps.Count - 1);

                UpdateSelection();
                RenderGrid();

                // Update empty state
                if (_apps.Count == 0)
                {
                    AppGrid.Visibility = Visibility.Collapsed;
                    EmptyStateText.Visibility = Visibility.Visible;
                }

                return;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = selectedApp.Path,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch {selectedApp.Name}: {ex.Message}",
                    "Launch Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            FadeOut();
        }

        private void RestartApplication()
        {
            try
            {
                // Use Environment.ProcessPath for single-file apps
                var exePath = Environment.ProcessPath;

                if (string.IsNullOrEmpty(exePath))
                {
                    // Fallback: Get path from current process
                    exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
                }

                if (string.IsNullOrEmpty(exePath))
                {
                    MessageBox.Show("Unable to determine application path. Please restart manually.",
                        "Restart Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Start a new instance
                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = true
                });

                // Close this instance
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restart application: {ex.Message}\n\nPlease restart manually.",
                    "Restart Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}