using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using LD.Contracts.DTOs.Realtime;

namespace LD.FormsX.Views.Usuarios;

/// <summary>
/// Resume la lista de almacenes de un usuario conectado para la celda del grid:
/// sin almacenes → "—", uno → su nombre, varios → "•••" (el detalle va en el ToolTip).
/// </summary>
public sealed class WarehousesSummaryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IEnumerable<ConnectedUserWarehouseDto> warehouses)
            return "—";

        var list = warehouses.ToList();
        return list.Count switch
        {
            0 => "—",
            1 => list[0].WarehouseName ?? "—",
            _ => "•••"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
