using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;



namespace QuickGridLauncher.Models
{
    public class AppEntry : INotifyPropertyChanged
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";

        private double _scale = 1.0;
        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged();
            }
        }

        public ImageSource? Icon { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}