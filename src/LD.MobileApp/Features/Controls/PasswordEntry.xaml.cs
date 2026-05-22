namespace MauiAppLogin.Views.Controls;

public partial class PasswordEntry : ContentView
{
    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(PasswordEntry), string.Empty);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(PasswordEntry), string.Empty);

    public static readonly BindableProperty PasswordProperty =
        BindableProperty.Create(nameof(Password), typeof(string), typeof(PasswordEntry), string.Empty,
            BindingMode.TwoWay, propertyChanged: OnPasswordChanged);

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(PasswordEntry), null,
            propertyChanged: OnIconSourceChanged);

    private static void OnPasswordChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ctrl = (PasswordEntry)bindable;
        var val = (string?)newValue ?? string.Empty;
        if (ctrl.PasswordField.Text != val)
            ctrl.PasswordField.Text = val;
    }

    private static void OnIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        => ((PasswordEntry)bindable).IconImage.IsVisible = newValue is not null;

    private bool _isPasswordVisible;

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    public ImageSource? IconSource
    {
        get => (ImageSource?)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public PasswordEntry()
    {
        InitializeComponent();

        PasswordField.TextChanged += OnPasswordFieldTextChanged;
        PasswordField.Focused += OnPasswordFieldFocused;
        PasswordField.Unfocused += OnPasswordFieldUnfocused;
        PasswordField.HandlerChanged += OnPasswordFieldHandlerChanged;
        ToggleButton.Clicked += OnToggleButtonClicked;
        Unloaded += OnUnloaded;
    }

    private void OnPasswordFieldTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (Password != e.NewTextValue)
            Password = e.NewTextValue;
    }

    private async void OnPasswordFieldFocused(object? sender, FocusEventArgs e)
        => await FadeUnderlineAsync(1);

    private async void OnPasswordFieldUnfocused(object? sender, FocusEventArgs e)
        => await FadeUnderlineAsync(0.25);

    private void OnPasswordFieldHandlerChanged(object? sender, EventArgs e)
        => RemoveNativeBorder(PasswordField);

    private void OnToggleButtonClicked(object? sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        PasswordField.IsPassword = !_isPasswordVisible;
        ToggleButton.Source = _isPasswordVisible ? "hidden.png" : "eye.png";
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        PasswordField.TextChanged -= OnPasswordFieldTextChanged;
        PasswordField.Focused -= OnPasswordFieldFocused;
        PasswordField.Unfocused -= OnPasswordFieldUnfocused;
        PasswordField.HandlerChanged -= OnPasswordFieldHandlerChanged;
        ToggleButton.Clicked -= OnToggleButtonClicked;
        Unloaded -= OnUnloaded;
    }

    private async Task FadeUnderlineAsync(double targetOpacity)
    {
        if (Handler is null || Window is null || Underline?.Handler is null)
            return;

        try
        {
            await Underline.FadeTo(targetOpacity, 300);
        }
        catch (ObjectDisposedException)
        {
        }
    }

    private static void RemoveNativeBorder(Entry entry)
    {
#if ANDROID
        if (entry.Handler?.PlatformView is Android.Widget.EditText editText)
            editText.Background = null;
#elif IOS || MACCATALYST
        if (entry.Handler?.PlatformView is UIKit.UITextField textField)
            textField.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
    }
}
