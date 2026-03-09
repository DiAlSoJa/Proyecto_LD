using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;



namespace MauiAppLogin.Converters;

public class OptionsToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IList<string> options)
            return options.Count == 2;

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null!;
    }
}