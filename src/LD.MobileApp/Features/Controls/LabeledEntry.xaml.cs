namespace MauiAppLogin.Views.Controls;

public partial class LabeledEntry : ContentView
{
    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(nameof(LabelText), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LabeledEntry), string.Empty);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(LabeledEntry), string.Empty,
            BindingMode.TwoWay, propertyChanged: OnTextChanged);

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(LabeledEntry), null,
            propertyChanged: OnIconSourceChanged);

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ctrl = (LabeledEntry)bindable;
        var val = (string?)newValue ?? string.Empty;
        if (ctrl.InnerEntry.Text != val)
            ctrl.InnerEntry.Text = val;
    }

    private static void OnIconSourceChanged(BindableObject bindable, object oldValue, object newValue)
        => ((LabeledEntry)bindable).IconImage.IsVisible = newValue is not null;

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

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ImageSource? IconSource
    {
        get => (ImageSource?)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public LabeledEntry()
    {
        InitializeComponent();

        InnerEntry.TextChanged += OnInnerEntryTextChanged;
        InnerEntry.Focused += OnInnerEntryFocused;
        InnerEntry.Unfocused += OnInnerEntryUnfocused;
        InnerEntry.HandlerChanged += OnInnerEntryHandlerChanged;
        Unloaded += OnUnloaded;
    }

    private void OnInnerEntryTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (Text != e.NewTextValue)
            Text = e.NewTextValue;
    }

    private async void OnInnerEntryFocused(object? sender, FocusEventArgs e)
        => await FadeUnderlineAsync(1);

    private async void OnInnerEntryUnfocused(object? sender, FocusEventArgs e)
        => await FadeUnderlineAsync(0.25);

    private void OnInnerEntryHandlerChanged(object? sender, EventArgs e)
        => RemoveNativeBorder(InnerEntry);

    private void OnUnloaded(object? sender, EventArgs e)
    {
        InnerEntry.TextChanged -= OnInnerEntryTextChanged;
        InnerEntry.Focused -= OnInnerEntryFocused;
        InnerEntry.Unfocused -= OnInnerEntryUnfocused;
        InnerEntry.HandlerChanged -= OnInnerEntryHandlerChanged;
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
