using LD.Contracts.DTOs.ReportQueries;
using LD.FormsX.Helpers;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace LD.FormsX.Views.Reportes;

public partial class ReportQueryResultView : Window
{
    private readonly DataGridColumnFilterManager _gridManager;

    public ReportQueryResultView()
    {
        InitializeComponent();
        _gridManager = new DataGridColumnFilterManager(dgResultado);
        DataGridFilterStyler.Apply(dgResultado);
    }

    public void SetResult(ReportQueryExecutionResultDto result)
    {
        txtTitulo.Text = string.IsNullOrWhiteSpace(result.Nombre)
            ? "Resultado de consulta"
            : $"Resultado - {result.Nombre}";

        txtResumen.Text = $"{result.RowCount} fila(s) | {result.Columns.Count} columna(s)";

        var table = BuildTable(result);
        dgResultado.ItemsSource = table.DefaultView;
        _gridManager.ApplyTo(CollectionViewSource.GetDefaultView(dgResultado.ItemsSource));
    }

    private static DataTable BuildTable(ReportQueryExecutionResultDto result)
    {
        var table = new DataTable();

        var columns = result.Columns
            .Where(column => !string.IsNullOrWhiteSpace(column))
            .Select((column, index) => new
            {
                Name = column.Trim(),
                Index = index
            })
            .ToList();

        if (columns.Count == 0)
            return table;

        foreach (var column in columns)
        {
            var columnName = column.Name;
            if (table.Columns.Contains(columnName))
                columnName = $"{columnName}_{column.Index + 1}";

            table.Columns.Add(columnName, typeof(string));
        }

        foreach (var row in result.Rows ?? [])
        {
            var dataRow = table.NewRow();

            for (var index = 0; index < columns.Count; index++)
            {
                var columnName = table.Columns[index].ColumnName;
                if (row.TryGetValue(columns[index].Name, out var value))
                    dataRow[columnName] = string.IsNullOrEmpty(value) ? DBNull.Value : value;
                else if (row.TryGetValue(columnName, out var alternateValue))
                    dataRow[columnName] = string.IsNullOrEmpty(alternateValue) ? DBNull.Value : alternateValue;
                else
                    dataRow[columnName] = DBNull.Value;
            }

            table.Rows.Add(dataRow);
        }

        return table;
    }

    private void BtnCerrar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        dgResultado.Focus();
    }
}
