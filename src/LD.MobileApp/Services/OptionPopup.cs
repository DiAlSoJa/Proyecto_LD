using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;

namespace MauiAppLogin;

public enum OptionPopupPresentation
{
    Standard,
    Separated
}

public sealed class OptionPopup : Popup
{
    private readonly TaskCompletionSource<string?> _tcs = new();
    public Task<string?> Result => _tcs.Task;

    public OptionPopup(
        string title,
        IEnumerable<string> options,
        OptionPopupPresentation presentation = OptionPopupPresentation.Standard)
    {
        var separatedOptions = presentation == OptionPopupPresentation.Separated;

        // Contenedor (tarjeta)
        var container = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(18) },
            Padding = 16,
            WidthRequest = 320
        };

        // Header: título + botón cerrar
        var header = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        var titleLbl = new Label
        {
            Text = title,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#1E293B"),
            VerticalOptions = LayoutOptions.Center
        };

        var closeBtn = new Button
        {
            Text = "✕",
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#64748B"),
            Padding = new Thickness(8, 0),
            FontSize = 16,
            VerticalOptions = LayoutOptions.Center
        };
        closeBtn.Clicked += (_, __) => CloseWith(null);

        header.Add(titleLbl);
        header.Add(closeBtn);
        Grid.SetColumn(closeBtn, 1);

        var desc = new Label
        {
            Text = separatedOptions
                ? "Selecciona cuidadosamente el tipo de operación:"
                : "Selecciona una opción:",
            FontSize = 13,
            TextColor = Color.FromArgb("#64748B")
        };

        // Lista de opciones
        var list = new VerticalStackLayout
        {
            Spacing = separatedOptions ? 12 : 0
        };

        foreach (var opt in options)
        {
            list.Add(separatedOptions ? BuildSeparatedRow(opt) : BuildRow(opt));

            if (!separatedOptions)
            {
                list.Add(new BoxView
                {
                    HeightRequest = 1,
                    BackgroundColor = Color.FromArgb("#EEF2F7")
                });
            }
        }

        if (!separatedOptions && list.Count > 0)
            list.RemoveAt(list.Count - 1);

        var cancel = new Button
        {
            Text = "Cancelar",
            BackgroundColor = Color.FromArgb("#F1F5F9"),
            TextColor = Color.FromArgb("#1E293B"),
            CornerRadius = 14,
            HeightRequest = 44,
            Margin = new Thickness(0, 12, 0, 0)
        };
        cancel.Clicked += (_, __) => CloseWith(null);

        container.Content = new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                header,
                desc,
                new ScrollView
                {
                    Content = list,
                    HeightRequest = separatedOptions ? 164 : 260
                },
                cancel
            }
        };

        // Fondo semitransparente (SIN tap para cerrar)
        // Si tocan afuera, no pasa nada.
        var background = new Grid
        {
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.35f)
        };

        // Captura el tap en el contenedor (solo para evitar propagación)
        var tapInside = new TapGestureRecognizer();
        tapInside.Tapped += (_, __) => { };
        container.GestureRecognizers.Add(tapInside);

        background.Add(new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { container }
        });

        Content = background;
    }

    private View BuildRow(string text)
    {
        var rowBorder = new Border
        {
            StrokeThickness = 0,
            BackgroundColor = Colors.Transparent,
            Padding = new Thickness(12, 12),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) }
        };

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        var lbl = new Label
        {
            Text = text,
            FontSize = 14,
            TextColor = Color.FromArgb("#1E293B"),
            VerticalOptions = LayoutOptions.Center
        };

        var chevron = new Label
        {
            Text = "›",
            FontSize = 18,
            TextColor = Color.FromArgb("#94A3B8"),
            VerticalOptions = LayoutOptions.Center
        };

        grid.Add(lbl);
        grid.Add(chevron);
        Grid.SetColumn(chevron, 1);

        rowBorder.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, __) => CloseWith(text);
        rowBorder.GestureRecognizers.Add(tap);

        return rowBorder;
    }

    private View BuildSeparatedRow(string text)
    {
        var isLoad = string.Equals(text, "Carga", StringComparison.OrdinalIgnoreCase);
        var accentColor = Color.FromArgb(isLoad ? "#1D4ED8" : "#C2410C");
        var backgroundColor = Color.FromArgb(isLoad ? "#EFF6FF" : "#FFF7ED");
        var strokeColor = Color.FromArgb(isLoad ? "#BFDBFE" : "#FED7AA");

        var rowBorder = new Border
        {
            Stroke = strokeColor,
            StrokeThickness = 1.5,
            BackgroundColor = backgroundColor,
            Padding = new Thickness(12, 10),
            MinimumHeightRequest = 70,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) }
        };

        var grid = new Grid
        {
            ColumnSpacing = 12,
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        var badge = new Border
        {
            WidthRequest = 42,
            HeightRequest = 42,
            StrokeThickness = 0,
            BackgroundColor = accentColor,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(13) },
            Content = new Label
            {
                Text = isLoad ? "C" : "D",
                TextColor = Colors.White,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            }
        };

        var textBlock = new VerticalStackLayout
        {
            Spacing = 1,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label
                {
                    Text = text,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0F172A")
                },
                new Label
                {
                    Text = isLoad
                        ? "Registrar operación de carga"
                        : "Registrar operación de descarga",
                    FontSize = 12,
                    TextColor = Color.FromArgb("#64748B")
                }
            }
        };

        var chevron = new Label
        {
            Text = "›",
            FontSize = 22,
            TextColor = accentColor,
            VerticalOptions = LayoutOptions.Center
        };

        grid.Add(badge);
        grid.Add(textBlock);
        grid.Add(chevron);
        Grid.SetColumn(textBlock, 1);
        Grid.SetColumn(chevron, 2);
        rowBorder.Content = grid;

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, __) => CloseWith(text);
        rowBorder.GestureRecognizers.Add(tap);

        return rowBorder;
    }

    private void CloseWith(string? value)
    {
        if (!_tcs.Task.IsCompleted)
            _tcs.SetResult(value);

        Close();
    }
}
