using CommunityToolkit.Maui.Views;

namespace MauiAppLogin.Views.Controls;

public partial class PasswordPromptPopup : Popup
{
    private bool _closing;

    public static readonly BindableProperty TitleTextProperty =
        BindableProperty.Create(nameof(TitleText), typeof(string), typeof(PasswordPromptPopup), string.Empty);

    public static readonly BindableProperty MessageTextProperty =
        BindableProperty.Create(nameof(MessageText), typeof(string), typeof(PasswordPromptPopup), string.Empty);

    public string TitleText
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public string MessageText
    {
        get => (string)GetValue(MessageTextProperty);
        set => SetValue(MessageTextProperty, value);
    }

    public PasswordPromptPopup(string titleText, string messageText)
    {
        InitializeComponent();
        TitleText = titleText;
        MessageText = messageText;
    }

    private void OnCancelTapped(object? sender, TappedEventArgs e)
    {
        if (_closing) return;
        _closing = true;
        Close(null);
    }

    private void OnConfirmTapped(object? sender, TappedEventArgs e)
    {
        if (_closing) return;
        _closing = true;
        Close(string.IsNullOrWhiteSpace(PasswordField.Password) ? null : PasswordField.Password);
    }
}
