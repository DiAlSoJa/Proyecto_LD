using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Controls;

public partial class StyledDataGrid : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(StyledDataGrid),
            new PropertyMetadata(null, (d, e) =>
                ((StyledDataGrid)d).InnerGrid.ItemsSource = (IEnumerable?)e.NewValue));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(StyledDataGrid),
            new PropertyMetadata(null, (d, e) =>
                ((StyledDataGrid)d).InnerGrid.SelectedItem = e.NewValue));

    public static readonly RoutedEvent SelectionChangedEvent =
        EventManager.RegisterRoutedEvent(nameof(SelectionChanged), RoutingStrategy.Bubble,
            typeof(SelectionChangedEventHandler), typeof(StyledDataGrid));

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public event SelectionChangedEventHandler SelectionChanged
    {
        add => AddHandler(SelectionChangedEvent, value);
        remove => RemoveHandler(SelectionChangedEvent, value);
    }

    /// <summary>Acceso directo al DataGrid interno (para WpfGridFilter u otras utilidades).</summary>
    public DataGrid DataGrid => InnerGrid;

    public StyledDataGrid() => InitializeComponent();

    public void ShowLoader(string text = "Cargando...") => Loader.Show(text);
    public void HideLoader() => Loader.Hide();

    private void InnerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedItem = InnerGrid.SelectedItem;
        RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, e.RemovedItems, e.AddedItems));
    }
}
