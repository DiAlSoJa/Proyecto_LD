
using Microsoft.Maui.Graphics;
using System.Globalization;
namespace MauiAppLogin;

// Punto azul si NO leída, gris si leída
public sealed class ReadDotColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isRead = value is bool b && b;
        return isRead ? Color.FromArgb("#CBD5E1") : Color.FromArgb("#2F5DA8");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

// Bold si NO leída
public sealed class ReadToFontAttributesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isRead = value is bool b && b;
        return isRead ? FontAttributes.None : FontAttributes.Bold;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}