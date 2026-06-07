using LD.FormsX.Features.ReporteDanos.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace LD.FormsX.Views.ReporteDanos;

public partial class DamageReportView : UserControl
{
    private bool _loaded;
    private DamageReportViewModel ViewModel => (DamageReportViewModel)DataContext;

    public DamageReportView(DamageReportViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded) return;
        _loaded = true;

        await ViewModel.InicializarAsync();
    }

    private async void PrintButton_Click(object sender, RoutedEventArgs e)
    {
        var report = await ViewModel.ObtenerReporteSeleccionadoAsync();
        if (report is null)
        {
            DialogHelper.ShowWarning("Selecciona un reporte para imprimir.", "Imprimir");
            return;
        }

        try
        {
            DamageReportDocumentExporter.Print(report, ViewModel.GetImageUrl);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message, "Imprimir");
        }
    }

    private async void ExcelButton_Click(object sender, RoutedEventArgs e)
    {
        var report = await ViewModel.ObtenerReporteSeleccionadoAsync();
        if (report is null)
        {
            DialogHelper.ShowWarning("Selecciona un reporte para exportar.", "Excel");
            return;
        }

        var saveDialog = new SaveFileDialog
        {
            Title = "Exportar reporte de daño a Excel",
            Filter = "Archivo de Excel (*.xlsx)|*.xlsx",
            FileName = $"Reporte_Dano_{report.DamageReportId}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
        };

        if (saveDialog.ShowDialog() != true)
            return;

        try
        {
            await DamageReportDocumentExporter.ExportExcelAsync(
                report,
                saveDialog.FileName,
                ViewModel.GetImageBytesAsync);

            DialogHelper.ShowSuccess("Excel exportado correctamente.", "Excel");

            Process.Start(new ProcessStartInfo
            {
                FileName = saveDialog.FileName,
                UseShellExecute = true
            });
        }
        catch (IOException ex)
        {
            DialogHelper.ShowError($"No se pudo guardar el archivo. Verifica que no esté abierto en Excel.\n{ex.Message}", "Excel");
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message, "Excel");
        }
    }
}
