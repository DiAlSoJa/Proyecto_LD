using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DamageReports;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.ReporteDanos.ViewModels;

public partial class DamageReportViewModel : ObservableObject
{
    private readonly DamageReportService _damageReportService;
    private readonly WarehouseService _warehouseService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Cargando reporte de daños...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private DateTime? desde;

    [ObservableProperty]
    private DateTime? hasta;

    [ObservableProperty]
    private WarehouseDto? selectedWarehouse;

    [ObservableProperty]
    private string? selectedDamageType;

    [ObservableProperty]
    private string partNumber = "";

    [ObservableProperty]
    private string standardId = "";

    [ObservableProperty]
    private bool canView;

    [ObservableProperty]
    private DamageReportDto? selectedReport;

    public ObservableCollection<WarehouseDto> Warehouses { get; } = [];
    public ObservableCollection<DamageReportDto> Reports { get; } = [];
    public ObservableCollection<string> DamageTypes { get; } =
    [
        "Sin Daño",
        "Material con Daño"
    ];

    public DamageReportViewModel(DamageReportService damageReportService, WarehouseService warehouseService)
    {
        _damageReportService = damageReportService;
        _warehouseService = warehouseService;
        CanView = UserData.HasPermission(PermissionKeys.DamageReport_View);
    }

    [RelayCommand]
    public async Task InicializarAsync()
    {
        if (!CanView) return;

        await LoadWarehousesAsync();
        await CargarDatosAsync();
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Cargando reporte de daños...";

            var standardIdValue = int.TryParse(StandardId?.Trim(), out var parsedStandardId)
                ? parsedStandardId
                : (int?)null;

            var result = await _damageReportService.GetDamageReports(
                Desde,
                Hasta,
                standardIdValue,
                SelectedWarehouse?.Id,
                SelectedWarehouse?.NombreAlmacen,
                PartNumber,
                SelectedDamageType);

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            Reports.Clear();
            foreach (var report in result.Data ?? [])
                Reports.Add(report);

            SelectedReport = Reports.FirstOrDefault();
            StatusText = $"Registros: {Reports.Count}";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task LimpiarFiltrosAsync()
    {
        Desde = null;
        Hasta = null;
        SelectedWarehouse = null;
        SelectedDamageType = null;
        PartNumber = "";
        StandardId = "";

        await CargarDatosAsync();
    }

    private async Task LoadWarehousesAsync()
    {
        try
        {
            var result = await _warehouseService.GetWarehouses();
            if (!result.IsSuccess)
                return;

            Warehouses.Clear();
            foreach (var warehouse in result.Data ?? [])
                Warehouses.Add(warehouse);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    public async Task<DamageReportDto?> ObtenerReporteSeleccionadoAsync()
    {
        if (SelectedReport is null)
            return null;

        try
        {
            IsLoading = true;
            LoadingMessage = "Preparando reporte de daños...";

            var result = await _damageReportService.GetDamageReportById(SelectedReport.DamageReportId);
            if (result.IsSuccess && result.Data is not null)
                return result.Data;

            return SelectedReport;
        }
        catch
        {
            return SelectedReport;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public string? GetImageUrl(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return null;

        if (Uri.TryCreate(relativePath, UriKind.Absolute, out _))
            return relativePath;

        return _damageReportService.GetImageUrl(relativePath);
    }

    public Task<byte[]> GetImageBytesAsync(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return Task.FromResult(Array.Empty<byte>());

        return _damageReportService.GetImageBytesAsync(relativePath);
    }
}
