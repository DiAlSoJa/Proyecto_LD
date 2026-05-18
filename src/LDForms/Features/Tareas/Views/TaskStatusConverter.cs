using System;
using System.Globalization;
using System.Windows.Data;

namespace LD.FormsX.Views.Tareas;

public sealed class TaskStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool completed && completed
            ? "Finalizado"
            : "Pendiente";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
