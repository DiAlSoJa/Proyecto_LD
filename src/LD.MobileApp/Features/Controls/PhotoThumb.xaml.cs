using System.Windows.Input;

namespace MauiAppLogin.Views.Controls;

public partial class PhotoThumb : ContentView
{
    public static readonly BindableProperty PhotoSourceProperty =
        BindableProperty.Create(nameof(PhotoSource), typeof(ImageSource), typeof(PhotoThumb), null);

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(PhotoThumb), null);

    public static readonly BindableProperty DeleteCommandParameterProperty =
        BindableProperty.Create(nameof(DeleteCommandParameter), typeof(object), typeof(PhotoThumb), null);

    public static readonly BindableProperty ViewCommandProperty =
        BindableProperty.Create(nameof(ViewCommand), typeof(ICommand), typeof(PhotoThumb), null);

    public static readonly BindableProperty ViewCommandParameterProperty =
        BindableProperty.Create(nameof(ViewCommandParameter), typeof(object), typeof(PhotoThumb), null);

    public static readonly BindableProperty BadgeTextProperty =
        BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(PhotoThumb), string.Empty);

    public ImageSource? PhotoSource
    {
        get => (ImageSource?)GetValue(PhotoSourceProperty);
        set => SetValue(PhotoSourceProperty, value);
    }

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public object? DeleteCommandParameter
    {
        get => GetValue(DeleteCommandParameterProperty);
        set => SetValue(DeleteCommandParameterProperty, value);
    }

    public ICommand? ViewCommand
    {
        get => (ICommand?)GetValue(ViewCommandProperty);
        set => SetValue(ViewCommandProperty, value);
    }

    public object? ViewCommandParameter
    {
        get => GetValue(ViewCommandParameterProperty);
        set => SetValue(ViewCommandParameterProperty, value);
    }

    public string BadgeText
    {
        get => (string)GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public PhotoThumb()
    {
        InitializeComponent();
    }
}
