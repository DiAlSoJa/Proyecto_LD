using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;

namespace MauiAppLogin.Views.Controls;

public sealed class ImagePreviewPopup : Popup
{
    public ImagePreviewPopup(ImageSource imageSource)
    {
        Color = Colors.Transparent;
        CanBeDismissedByTappingOutsideOfPopup = true;

        var card = new Border
        {
            BackgroundColor = Color.FromArgb("#0F172A"),
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
            Padding = new Thickness(12),
            WidthRequest = 340,
            HeightRequest = 420,
        };

        var layout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
            },
            RowSpacing = 12,
        };

        var image = new Image
        {
            Source = imageSource,
            Aspect = Aspect.AspectFit,
        };

        var closeBtn = new Button
        {
            Text = "Cerrar",
            BackgroundColor = Color.FromArgb("#334155"),
            TextColor = Colors.White,
            CornerRadius = 14,
            HeightRequest = 44,
            FontAttributes = FontAttributes.Bold,
            FontSize = 15,
        };
        closeBtn.Clicked += (_, _) => Close();
        Grid.SetRow(closeBtn, 1);

        layout.Add(image);
        layout.Add(closeBtn);
        card.Content = layout;
        Content = card;
    }
}
