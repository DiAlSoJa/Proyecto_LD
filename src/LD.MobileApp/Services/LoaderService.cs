using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiAppLogin.Services;

public class LoaderService : ILoaderService, INotifyPropertyChanged
{
    private bool _isVisible;
    private string _message = "Cargando...";

    public bool IsVisible
    {
        get => _isVisible;
        private set => SetProperty(ref _isVisible, value);
    }

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public void Show(string message = "Cargando...")
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Message = message;
            IsVisible = true;
        });
    }

    public void Hide()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            IsVisible = false;
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
