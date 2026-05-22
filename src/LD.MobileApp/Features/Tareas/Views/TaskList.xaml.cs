using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.OperationalTasks;
using MauiAppLogin.Features.Seguridad.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class TaskList : ContentPage
{
    private readonly OperationalTaskService _operationalTaskService;
    private readonly LookupService _lookupService;
    private readonly ObservableCollection<ListItemTask> _items = new();
    private readonly ObservableCollection<ListItemTask> _filtered = new();
    private readonly List<DropDownDto> _warehouseFilterOptions = new();
    private readonly HashSet<int> _assignedWarehouseIds = new();
    private bool _isLoading;
    private bool _warehousesLoaded;

    public TaskList(OperationalTaskService operationalTaskService, LookupService lookupService)
    {
        InitializeComponent();
        _operationalTaskService = operationalTaskService;
        _lookupService = lookupService;
        ItemsList.ItemsSource = _filtered;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadWarehousesAsync();
        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;
            var warehouseId = GetSelectedWarehouseId();
            var response = await _operationalTaskService.GetTasks(soloPendientes: true, warehouseId);

            if (!response.IsSuccess)
            {
                await DisplayAlertAsync("Tareas", response.Message ?? "No se pudieron cargar las tareas.", "OK");
                return;
            }

            _items.Clear();
            var tasks = response.Data ?? new List<OperationalTaskDto>();
            if (!warehouseId.HasValue && _assignedWarehouseIds.Count > 0)
                tasks = tasks.Where(x => x.WarehouseId.HasValue && _assignedWarehouseIds.Contains(x.WarehouseId.Value)).ToList();

            foreach (var task in tasks)
                _items.Add(MapTask(task));

            ApplyFilter(FiltroEntry.Text);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            _isLoading = false;
        }
    }

    private static ListItemTask MapTask(OperationalTaskDto task)
    {
        var warehouse = string.IsNullOrWhiteSpace(task.WarehouseName)
            ? "Sin almacen"
            : task.WarehouseName;

        return new ListItemTask
        {
            TaskId = task.OperationalTaskId,
            Titulo = $"[{task.OperationalTaskId:0000}] -> {task.Name}",
            Subtitulo = $" {task.CreatedAt:dd MMM hh:mm tt} -> {warehouse} -> {task.Activity.ToUpperInvariant()} -> {task.Priority}"
        };
    }

    private void OnFiltroChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue);
    }

    private void ApplyFilter(string? value)
    {
        var text = (value ?? string.Empty).Trim().ToLowerInvariant();

        _filtered.Clear();

        foreach (var it in _items)
        {
            if (string.IsNullOrEmpty(text) ||
                it.Titulo.ToLowerInvariant().Contains(text) ||
                it.Subtitulo.ToLowerInvariant().Contains(text))
            {
                _filtered.Add(it);
            }
        }
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection?.FirstOrDefault() as ListItemTask;
        if (selected == null)
            return;

        ItemsList.SelectedItem = null;

        await Shell.Current.GoToAsync("TaskResolve", new Dictionary<string, object>
        {
            ["TaskId"] = selected.TaskId
        });
    }

    private async void OnTaskNewClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("NewTask");
    }

    private async void OnWarehouseFilterChanged(object sender, EventArgs e)
    {
        if (!_warehousesLoaded)
            return;

        await LoadTasksAsync();
    }

    private int? GetSelectedWarehouseId()
    {
        if (WarehouseFilterPicker.SelectedItem is not DropDownDto warehouse ||
            string.IsNullOrWhiteSpace(warehouse.Key) ||
            !int.TryParse(warehouse.Key, out var warehouseId))
        {
            return null;
        }

        return warehouseId;
    }

    private async Task LoadWarehousesAsync()
    {
        if (_warehousesLoaded)
            return;

        try
        {
            if (string.IsNullOrWhiteSpace(UserData.Id))
                return;

            var response = await _lookupService.GetWarehouseLookupByUser(UserData.Id);
            if (!response.IsSuccess)
            {
                await DisplayAlertAsync("Almacenes", response.Message ?? "No se pudieron cargar los almacenes.", "OK");
                return;
            }

            var warehouses = response.Data ?? new List<DropDownDto>();
            _assignedWarehouseIds.Clear();
            foreach (var warehouse in warehouses)
            {
                if (int.TryParse(warehouse.Key, out var warehouseId))
                    _assignedWarehouseIds.Add(warehouseId);
            }

            _warehouseFilterOptions.Clear();
            if (warehouses.Count > 1)
            {
                _warehouseFilterOptions.Add(new DropDownDto { Key = string.Empty, Value = "Todos los almacenes" });
                _warehouseFilterOptions.AddRange(warehouses);
                WarehouseFilterPicker.ItemsSource = _warehouseFilterOptions;
                WarehouseFilterPicker.SelectedIndex = 0;
                WarehouseFilterLabel.IsVisible = true;
                WarehouseFilterPicker.IsVisible = true;
            }
            else
            {
                WarehouseFilterPicker.ItemsSource = warehouses;
                if (warehouses.Count == 1)
                    WarehouseFilterPicker.SelectedIndex = 0;
                WarehouseFilterLabel.IsVisible = false;
                WarehouseFilterPicker.IsVisible = false;
            }

            _warehousesLoaded = true;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}
