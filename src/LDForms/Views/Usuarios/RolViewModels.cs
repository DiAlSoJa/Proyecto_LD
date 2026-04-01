using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LD.FormsX.Views.Usuarios
{
    public class PermissionNodeVm : INotifyPropertyChanged
    {
        private bool _isChecked;
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }

        public bool IsChecked
        {
            get => _isChecked;
            set { _isChecked = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class ModuleNodeVm : INotifyPropertyChanged
    {
        private bool _isChecked;
        public string? ModuleName { get; set; }
        public List<PermissionNodeVm> Permissions { get; set; } = new();

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                foreach (var p in Permissions)
                    p.IsChecked = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
