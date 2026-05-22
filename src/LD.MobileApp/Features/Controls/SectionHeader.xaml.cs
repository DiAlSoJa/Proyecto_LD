namespace MauiAppLogin.Views.Controls;

public partial class SectionHeader : ContentView
{
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(SectionHeader), string.Empty);

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(SectionHeader), string.Empty);

    public static readonly BindableProperty IsExpandedProperty =
        BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(SectionHeader), false,
            propertyChanged: OnIsExpandedChanged);

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public SectionHeader()
    {
        InitializeComponent();
    }

    private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var ctrl = (SectionHeader)bindable;
        var expanded = (bool)newValue;
        if (ctrl.Handler is not null)
            _ = ctrl.Chevron.RotateTo(expanded ? 180 : 0, 250, Easing.CubicInOut);
        else
            ctrl.Chevron.Rotation = expanded ? 180 : 0;
    }
}
