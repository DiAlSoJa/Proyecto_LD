using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Tareas.ViewModels;

public partial class TasksViewModel : ObservableObject
{
    private readonly OperationalTaskService _operationalTaskService;
    private readonly WarehouseService _warehouseService;
    private readonly WarehouseTaskService _warehouseTaskService;
    private readonly List<OperationalTaskDto> _allTasks = [];

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Cargando tareas...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private DateTime? desde;

    [ObservableProperty]
    private DateTime? hasta;

    [ObservableProperty]
    private WarehouseDto? selectedWarehouse;

    [ObservableProperty]
    private string selectedStatus = "Todos";

    [ObservableProperty]
    private bool canView;

    [ObservableProperty]
    private bool isLoadingAsignadas;

    [ObservableProperty]
    private string asignadasStatusText = "";

    public ObservableCollection<WarehouseDto> Warehouses { get; } = [];
    public ObservableCollection<OperationalTaskDto> Tasks { get; } = [];

    // Listado de la tabla WarehouseTasks (tab "Tareas asignadas"), ordenado por OrderIndex.
    public ObservableCollection<WarehouseTaskDto> WarehouseTasks { get; } = [];
    public ObservableCollection<string> Statuses { get; } =
    [
        "Todos",
        "Pendiente",
        "Finalizado"
    ];

    public TasksViewModel(
        OperationalTaskService operationalTaskService,
        WarehouseService warehouseService,
        WarehouseTaskService warehouseTaskService)
    {
        _operationalTaskService = operationalTaskService;
        _warehouseService = warehouseService;
        _warehouseTaskService = warehouseTaskService;
        CanView = UserData.HasPermission(PermissionKeys.WarehouseStaff_Tasks_View);
    }

    [RelayCommand]
    public async Task InicializarAsync()
    {
        if (!CanView) return;

        await LoadWarehousesAsync();
        await CargarDatosAsync();
        await CargarTareasAsignadasAsync();
    }

    [RelayCommand]
    public async Task CargarTareasAsignadasAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoadingAsignadas = true;

            var result = await _warehouseTaskService.GetTasksAsync();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            WarehouseTasks.Clear();
            foreach (var task in result.Data ?? [])
                WarehouseTasks.Add(task);

            AsignadasStatusText = $"Registros: {WarehouseTasks.Count}";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoadingAsignadas = false;
        }
    }

    [RelayCommand]
    private void Asignar(WarehouseTaskDto? task)
    {
        // Placeholder: la lógica de asignación manual aún no está implementada.
        var nombre = task?.Name ?? "la tarea seleccionada";
        DialogHelper.ShowInfo(
            $"Aquí se asignará manualmente «{nombre}» a un usuario.\n\n(Funcionalidad pendiente de implementar.)",
            "Asignar tarea");
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Cargando tareas...";

            var result = await _operationalTaskService.GetTasks(false, SelectedWarehouse?.Id);

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _allTasks.Clear();
            _allTasks.AddRange(result.Data ?? []);
            ApplyFilters();
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
        SelectedStatus = "Todos";

        await CargarDatosAsync();
    }

    public string GetImageUrl(string relativePath) => _operationalTaskService.GetImageUrl(relativePath);

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

    private void ApplyFilters()
    {
        var filtered = _allTasks.AsEnumerable();

        if (Desde.HasValue)
            filtered = filtered.Where(x => x.CreatedAt.Date >= Desde.Value.Date);

        if (Hasta.HasValue)
            filtered = filtered.Where(x => x.CreatedAt.Date <= Hasta.Value.Date);

        filtered = SelectedStatus switch
        {
            "Pendiente" => filtered.Where(x => !x.Completed),
            "Finalizado" => filtered.Where(x => x.Completed),
            _ => filtered
        };

        Tasks.Clear();
        foreach (var task in filtered.OrderByDescending(x => x.CreatedAt))
            Tasks.Add(task);

        StatusText = $"Registros: {Tasks.Count}";
    }
}
