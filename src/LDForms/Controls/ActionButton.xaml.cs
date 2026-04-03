using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LD.FormsX.Controls;

public partial class ActionButton : UserControl
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(ActionButton),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(ActionButton),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(ActionButton),
            new PropertyMetadata(Brushes.DimGray));

    public static readonly RoutedEvent ClickEvent =
        EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ActionButton));

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public Brush IconColor
    {
        get => (Brush)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public event RoutedEventHandler Click
    {
        add => AddHandler(ClickEvent, value);
        remove => RemoveHandler(ClickEvent, value);
    }

    public ActionButton() => InitializeComponent();

    private void Button_Click(object sender, RoutedEventArgs e)
        => RaiseEvent(new RoutedEventArgs(ClickEvent, this));
}
