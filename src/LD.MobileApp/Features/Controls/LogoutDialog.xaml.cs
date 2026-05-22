using CommunityToolkit.Maui.Views;

namespace MauiAppLogin.Views.Controls;

public partial class LogoutDialog : Popup
{
    private bool _closing;

    public LogoutDialog()
    {
        InitializeComponent();
    }

    private void OnCancelTapped(object? sender, TappedEventArgs e)
    {
        if (_closing) return;
        _closing = true;
        Close(false);
    }

    private void OnConfirmTapped(object? sender, TappedEventArgs e)
    {
        if (_closing) return;
        _closing = true;
        Close(true);
    }
}
