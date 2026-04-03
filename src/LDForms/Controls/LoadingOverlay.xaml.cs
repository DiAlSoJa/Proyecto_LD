using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Controls;

public partial class LoadingOverlay : UserControl
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(LoadingOverlay),
            new PropertyMetadata("Cargando...", OnTextChanged));

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((LoadingOverlay)d).TxtLoadingText.Text = (string)e.NewValue;

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public LoadingOverlay() => InitializeComponent();

    public void Show(string text = "Cargando...")
    {
        Text = text;
        Visibility = Visibility.Visible;
    }

    public void Hide() => Visibility = Visibility.Collapsed;
}
