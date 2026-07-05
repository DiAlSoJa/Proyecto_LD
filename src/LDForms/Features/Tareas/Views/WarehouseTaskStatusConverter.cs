using System;
using System.Globalization;
using System.Windows.Data;
using LD.Contracts.Enums;

namespace LD.FormsX.Views.Tareas;

public sealed class WarehouseTaskStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is WarehouseTaskStatus status
            ? status switch
            {
                WarehouseTaskStatus.NoAsignada => "No asignada",
                WarehouseTaskStatus.Asignada   => "Asignada",
                WarehouseTaskStatus.Completada => "Completada",
                _                              => status.ToString()
            }
            : string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
