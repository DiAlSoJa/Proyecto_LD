using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace MauiAppLogin.Views.Controls;

public partial class Card : Border
{
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(Card),
            new CornerRadius(16), propertyChanged: OnCornerRadiusChanged);

    private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        => ((Card)bindable).ShapeRect.CornerRadius = (CornerRadius)newValue;

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public Card() => InitializeComponent();
}
