using System.Globalization;

namespace MauiAppLogin.Converters;

public class OptionsToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isTwoOptions = false;

        if (value is IList<string> options)
            isTwoOptions = options.Count == 2;

        if (parameter?.ToString()?.ToLower() == "invert")
            return !isTwoOptions;

        return isTwoOptions;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}