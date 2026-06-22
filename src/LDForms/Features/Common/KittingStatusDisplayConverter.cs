using System;
using System.Globalization;
using System.Windows.Data;
using LD.Contracts.Constants;

namespace LD.FormsX.Features.Common;

public sealed class KittingStatusDisplayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return KittingStatusNames.Display(value?.ToString());
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
