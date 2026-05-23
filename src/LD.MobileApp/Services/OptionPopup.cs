using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;

namespace MauiAppLogin;

public sealed class OptionPopup : Popup
{
    private readonly TaskCompletionSource<string?> _tcs = new();
    public Task<string?> Result => _tcs.Task;

    private static readonly Dictionary<string, string> OptionIcons = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Carga"]    = "🚚",
        ["Descarga"] = "📦",
    };

    public OptionPopup(string title, IEnumerable<string> options)
    {
        var background = new Grid
        {
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.45f)
        };

        var card = new Border
        {
            BackgroundColor = Colors.White,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(24) },
            Padding = new Thickness(24, 28, 24, 24),
            WidthRequest = 340,
            Shadow = new Shadow
            {
                Brush = new SolidColorBrush(Colors.Black),
                Offset = new Point(0, 8),
                Radius = 24,
                Opacity = 0.18f
            }
        };

        // ── Header ─────────────────────────────────────────────────────────
        var headerIcon = new Label
        {
            Text = "🏢",
            FontSize = 36,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 0, 0, 4)
        };

        var titleLabel = new Label
        {
            Text = title,
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#0F2E4F"),
            HorizontalOptions = LayoutOptions.Center
        };

        var subtitleLabel = new Label
        {
            Text = "Selecciona una opción:",
            FontSize = 13,
            TextColor = Color.FromArgb("#64748B"),
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 2, 0, 0)
        };

        var separator = new BoxView
        {
            HeightRequest = 1,
            BackgroundColor = Color.FromArgb("#E2E8F0"),
            Margin = new Thickness(0, 16, 0, 0)
        };

        // ── Option cards ───────────────────────────────────────────────────
        var optionsList = new VerticalStackLayout
        {
            Spacing = 10,
            Margin = new Thickness(0, 16, 0, 0)
        };

        foreach (var opt in options)
            optionsList.Add(BuildOptionCard(opt));

        // ── Cancel ─────────────────────────────────────────────────────────
        var cancelBtn = new Button
        {
            Text = "Cancelar",
            BackgroundColor = Color.FromArgb("#F1F5F9"),
            TextColor = Color.FromArgb("#475569"),
            CornerRadius = 16,
            HeightRequest = 50,
            FontSize = 15,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 12, 0, 0)
        };
        cancelBtn.Clicked += (_, __) => CloseWith(null);

        card.Content = new VerticalStackLayout
        {
            Spacing = 0,
            Children =
            {
                headerIcon,
                titleLabel,
                subtitleLabel,
                separator,
                optionsList,
                cancelBtn
            }
        };

        // Absorb taps so the background overlay doesn't propagate
        var tapInside = new TapGestureRecognizer();
        tapInside.Tapped += (_, __) => { };
        card.GestureRecognizers.Add(tapInside);

        background.Add(new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { card }
        });

        Content = background;
    }

    private View BuildOptionCard(string text)
    {
        var icon = OptionIcons.TryGetValue(text, out var emoji) ? emoji : "•";

        var optCard = new Border
        {
            BackgroundColor = Color.FromArgb("#F8FAFC"),
            StrokeThickness = 1.5,
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
            Padding = new Thickness(18, 16),
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto),
            },
            ColumnSpacing = 14
        };

        var iconLabel = new Label
        {
            Text = icon,
            FontSize = 30,
            VerticalOptions = LayoutOptions.Center
        };

        var textLabel = new Label
        {
            Text = text,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#0F2E4F"),
            VerticalOptions = LayoutOptions.Center
        };

        var chevron = new Label
        {
            Text = "›",
            FontSize = 24,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#94A3B8"),
            VerticalOptions = LayoutOptions.Center
        };

        grid.Add(iconLabel);
        grid.Add(textLabel);
        grid.Add(chevron);
        Grid.SetColumn(textLabel, 1);
        Grid.SetColumn(chevron, 2);

        optCard.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, __) =>
        {
            optCard.BackgroundColor = Color.FromArgb("#EFF6FF");
            optCard.Stroke = Color.FromArgb("#3B82F6");
            await Task.Delay(120);
            CloseWith(text);
        };
        optCard.GestureRecognizers.Add(tap);

        return optCard;
    }

    private void CloseWith(string? value)
    {
        if (!_tcs.Task.IsCompleted)
            _tcs.SetResult(value);

        Close();
    }
}
