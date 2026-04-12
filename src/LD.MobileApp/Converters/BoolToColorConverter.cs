using System.Globalization;

namespace MauiAppLogin.Converters;

/// <summary>
/// Returns an "active" color when true, "inactive" color when false.
/// Used for toggle-style selection buttons.
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    public Color TrueColor { get; set; } = Color.FromArgb("#E76F51");
    public Color FalseColor { get; set; } = Color.FromArgb("#E5E7EB");

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? TrueColor : FalseColor;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
