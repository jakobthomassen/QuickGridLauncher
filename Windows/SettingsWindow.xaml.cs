using Microsoft.Win32;
using QuickGridLauncher.Models;
using QuickGridLauncher.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuickGridLauncher.Windows
{
    public partial class SettingsWindow : Window
    {
        private readonly AppConfig _config;
        private readonly ObservableCollection<AppEntry> _apps;
        private Point _dragStartPoint;
        private AppEntry? _draggedItem;

        public SettingsWindow(AppConfig config)
        {
            InitializeComponent();

            _config = config;
            _apps = new ObservableCollection<AppEntry>(config.Apps);

            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load appearance settings
            ColumnsTextBox.Text = _config.Columns.ToString();

            // Strip alpha channel from background color if present (we use opacity slider instead)
            var bgColor = _config.BackgroundColor;
            if (bgColor.Length == 9 && bgColor.StartsWith("#"))
            {
                // #AARRGGBB -> #RRGGBB
                bgColor = "#" + bgColor.Substring(3);
            }
            BackgroundColorTextBox.Text = bgColor;

            OpacitySlider.Value = _config.BackgroundOpacity;
            OpacityValueText.Text = $"{(int)(_config.BackgroundOpacity * 100)}%";

            HighlightColorTextBox.Text = _config.HighlightColor;

            // Load hotkey settings
            AltCheckBox.IsChecked = (_config.HotkeyModifiers & 0x0001) != 0;
            CtrlCheckBox.IsChecked = (_config.HotkeyModifiers & 0x0002) != 0;
            ShiftCheckBox.IsChecked = (_config.HotkeyModifiers & 0x0004) != 0;

            // Select the correct key in combo box
            foreach (ComboBoxItem item in KeyComboBox.Items)
            {
                if (item.Tag != null)
                {
                    // Convert hex string (e.g., "0x20") to integer
                    var tagStr = item.Tag.ToString();
                    if (tagStr != null && tagStr.StartsWith("0x"))
                    {
                        var keyValue = Convert.ToInt32(tagStr, 16);
                        if (keyValue == _config.HotkeyKey)
                        {
                            KeyComboBox.SelectedItem = item;
                            break;
                        }
                    }
                }
            }

            // Load startup setting
            StartupCheckBox.IsChecked = _config.RunOnStartup;

            // Load app list
            AppListBox.ItemsSource = _apps;

            // Update hotkey preview
            UpdateHotkeyPreviewText();
        }

        private void UpdateHotkeyPreview(object sender, RoutedEventArgs e)
        {
            UpdateHotkeyPreviewText();
        }

        private void UpdateHotkeyPreviewText()
        {
            // Null check for initialization - controls may not be ready yet
            if (CurrentHotkeyText == null || AltCheckBox == null || CtrlCheckBox == null || ShiftCheckBox == null || KeyComboBox == null)
                return;

            var parts = new System.Collections.Generic.List<string>();

            if (CtrlCheckBox.IsChecked == true) parts.Add("Ctrl");
            if (AltCheckBox.IsChecked == true) parts.Add("Alt");
            if (ShiftCheckBox.IsChecked == true) parts.Add("Shift");

            var selectedItem = KeyComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem?.Content != null)
            {
                parts.Add(selectedItem.Content.ToString() ?? "");
            }

            CurrentHotkeyText.Text = parts.Count > 0 ? $"Current: {string.Join(" + ", parts)}" : "Current: (none)";
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OpacityValueText != null)
            {
                OpacityValueText.Text = $"{(int)(OpacitySlider.Value * 100)}%";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Executables (*.exe)|*.exe|All Files (*.*)|*.*",
                Title = "Select Application"
            };

            if (dialog.ShowDialog() == true)
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                var entry = new AppEntry
                {
                    Name = name,
                    Path = dialog.FileName
                };

                _apps.Add(entry);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppListBox.SelectedItem is AppEntry selected)
            {
                _apps.Remove(selected);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Save appearance settings
                if (int.TryParse(ColumnsTextBox.Text, out int columns) && columns > 0 && columns <= 10)
                {
                    _config.Columns = columns;
                }
                else
                {
                    MessageBox.Show("Columns must be between 1 and 10", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Save background color (without alpha) and opacity separately
                var bgColor = BackgroundColorTextBox.Text;
                if (bgColor.Length == 9 && bgColor.StartsWith("#"))
                {
                    // If user entered #AARRGGBB, strip alpha
                    bgColor = "#" + bgColor.Substring(3);
                }
                _config.BackgroundColor = bgColor;
                _config.BackgroundOpacity = OpacitySlider.Value;

                _config.HighlightColor = HighlightColorTextBox.Text;

                // Save hotkey settings
                int modifiers = 0;
                if (AltCheckBox.IsChecked == true) modifiers |= 0x0001;
                if (CtrlCheckBox.IsChecked == true) modifiers |= 0x0002;
                if (ShiftCheckBox.IsChecked == true) modifiers |= 0x0004;

                _config.HotkeyModifiers = modifiers;

                var selectedKey = KeyComboBox.SelectedItem as ComboBoxItem;
                if (selectedKey?.Tag != null)
                {
                    var tagStr = selectedKey.Tag.ToString();
                    if (tagStr != null && tagStr.StartsWith("0x"))
                    {
                        _config.HotkeyKey = Convert.ToInt32(tagStr, 16);
                    }
                }

                // Save startup setting
                _config.RunOnStartup = StartupCheckBox.IsChecked == true;
                StartupService.SetStartup(_config.RunOnStartup);

                // Save app list
                _config.Apps = _apps.ToList();

                // Save to disk
                ConfigService.Save(_config);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Drag-and-drop reordering
        private void AppListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void AppListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _draggedItem == null)
            {
                Point currentPosition = e.GetPosition(null);
                Vector diff = _dragStartPoint - currentPosition;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
                    if (listBoxItem != null)
                    {
                        _draggedItem = (AppEntry)AppListBox.ItemContainerGenerator.ItemFromContainer(listBoxItem);
                        if (_draggedItem != null)
                        {
                            DragDrop.DoDragDrop(listBoxItem, _draggedItem, DragDropEffects.Move);
                            _draggedItem = null;
                        }
                    }
                }
            }
        }

        private void AppListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(AppEntry)))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void AppListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(AppEntry)))
            {
                var droppedItem = (AppEntry)e.Data.GetData(typeof(AppEntry));
                var targetItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (targetItem != null)
                {
                    var targetData = (AppEntry)AppListBox.ItemContainerGenerator.ItemFromContainer(targetItem);
                    if (targetData != null && droppedItem != targetData)
                    {
                        int oldIndex = _apps.IndexOf(droppedItem);
                        int newIndex = _apps.IndexOf(targetData);

                        if (oldIndex >= 0 && newIndex >= 0)
                        {
                            _apps.Move(oldIndex, newIndex);
                        }
                    }
                }
            }
        }

        private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = System.Windows.Media.VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}