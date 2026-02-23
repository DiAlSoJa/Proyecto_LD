using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using MauiAppLogin;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
//using TuApp.Models;

namespace MauiAppLogin;

public sealed class NotificationsPopup : Popup
{
    private readonly ObservableCollection<NotificationItem> _items;
    private readonly Action<NotificationItem> _onOpen;

    public NotificationsPopup(IEnumerable<NotificationItem> items, Action<NotificationItem> onOpen)
    {
        _items = new ObservableCollection<NotificationItem>(items ?? Array.Empty<NotificationItem>());
        _onOpen = onOpen;

        var card = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
            WidthRequest = 340,
            HorizontalOptions = LayoutOptions.Center
        };

        card.Shadow = new Shadow { Opacity = 0.25f, Radius = 18, Offset = new Point(0, 10) };

        // Header
        var header = new Grid
        {
            Padding = new Thickness(16, 14),
            BackgroundColor = Color.FromArgb("#F8FAFC"),
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        header.Add(new Label { Text = "🔔", FontSize = 18, VerticalOptions = LayoutOptions.Center });
        header.Add(new Label
        {
            Text = "Notificaciones",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#1E293B"),
            VerticalOptions = LayoutOptions.Center
        });
       // Grid.SetColumn(header.Children[1], 1);

        var closeBtn = new Button
        {
            Text = "✕",
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#64748B"),
            FontSize = 16,
            Padding = new Thickness(10, 2),
            VerticalOptions = LayoutOptions.Center
        };
        closeBtn.Clicked += (_, __) => Close();
        header.Add(closeBtn);
        Grid.SetColumn(closeBtn, 2);

        // Lista
        View listView = BuildList();

        // Footer
        var footer = new Grid { Padding = new Thickness(16, 14) };
        var okBtn = new Button
        {
            Text = "Cerrar",
            BackgroundColor = Color.FromArgb("#1F3A5F"),
            TextColor = Colors.White,
            CornerRadius = 14,
            HeightRequest = 44
        };
        okBtn.Clicked += (_, __) => Close();
        footer.Add(okBtn);

        card.Content = new VerticalStackLayout
        {
            Spacing = 0,
            Children =
            {
                header,
                new BoxView { HeightRequest = 1, BackgroundColor = Color.FromArgb("#E2E8F0") },
                listView,
                footer
            }
        };

        Content = new Grid
        {
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.40f),
            Children =
            {
                new VerticalStackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { card }
                }
            }
        };
    }

    private View BuildList()
    {
        if (_items.Count == 0)
        {
            return new VerticalStackLayout
            {
                Padding = new Thickness(16, 18),
                Children =
                {
                    new Label
                    {
                        Text = "No tienes notificaciones.",
                        TextColor = Color.FromArgb("#64748B"),
                        FontSize = 14
                    }
                }
            };
        }

        var cv = new CollectionView
        {
            ItemsSource = _items,
            SelectionMode = SelectionMode.Single,
            Margin = 0,
            HeightRequest = 260,
            ItemTemplate = new DataTemplate(() =>
            {
                var row = new Grid
                {
                    Padding = new Thickness(16, 12),
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };

                // Punto: azul si no leída, gris si leída
                var dot = new BoxView
                {
                    WidthRequest = 8,
                    HeightRequest = 8,
                    CornerRadius = 4,
                    VerticalOptions = LayoutOptions.Center
                };
                dot.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(NotificationItem.IsRead),
                    converter: new ReadDotColorConverter()));

                // Título
                var title = new Label
                {
                    FontSize = 14,
                    TextColor = Color.FromArgb("#334155"),
                    VerticalOptions = LayoutOptions.Center,
                    LineBreakMode = LineBreakMode.TailTruncation
                };
                title.SetBinding(Label.TextProperty, nameof(NotificationItem.Title));

                // No leída = bold
                title.SetBinding(Label.FontAttributesProperty, new Binding(nameof(NotificationItem.IsRead),
                    converter: new ReadToFontAttributesConverter()));

                // Chevron
                var chevron = new Label
                {
                    Text = "›",
                    FontSize = 18,
                    TextColor = Color.FromArgb("#94A3B8"),
                    VerticalOptions = LayoutOptions.Center
                };

                row.Add(dot);
                row.Add(title);
                row.Add(chevron);
                Grid.SetColumn(title, 1);
                Grid.SetColumn(chevron, 2);

                var sep = new BoxView
                {
                    HeightRequest = 1,
                    BackgroundColor = Color.FromArgb("#EEF2F7"),
                    Margin = new Thickness(16, 0)
                };

                return new VerticalStackLayout { Spacing = 0, Children = { row, sep } };
            })
        };

        // Click: marcar leída + abrir destino
        cv.SelectionChanged += (_, e) =>
        {
            var item = e.CurrentSelection?.FirstOrDefault() as NotificationItem;
            if (item == null) return;

            item.IsRead = true; // marca leída (si usas MVVM, mejor con INotifyPropertyChanged)
            cv.SelectedItem = null;

            // Cierra popup y luego abre
            Close();
            _onOpen?.Invoke(item);
        };

        return cv;
    }
}