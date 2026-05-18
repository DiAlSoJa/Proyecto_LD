using LD.Client.Services;
using LD.Contracts.DTOs.OperationalTasks;
using MauiAppLogin.Features.Seguridad.Models;
using System.Collections.ObjectModel;

namespace MauiAppLogin;

public partial class TaskList : ContentPage
{
    private readonly OperationalTaskService _operationalTaskService;
    private readonly ObservableCollection<ListItemTask> _items = new();
    private readonly ObservableCollection<ListItemTask> _filtered = new();
    private bool _isLoading;

    public TaskList(OperationalTaskService operationalTaskService)
    {
        InitializeComponent();
        _operationalTaskService = operationalTaskService;
        ItemsList.ItemsSource = _filtered;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;
            var response = await _operationalTaskService.GetTasks(soloPendientes: true);

            if (!response.IsSuccess)
            {
                await DisplayAlertAsync("Tareas", response.Message ?? "No se pudieron cargar las tareas.", "OK");
                return;
            }

            _items.Clear();
            foreach (var task in response.Data ?? new List<OperationalTaskDto>())
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
        return new ListItemTask
        {
            TaskId = task.OperationalTaskId,
            Titulo = $"[{task.OperationalTaskId:0000}] -> {task.Name}",
            Subtitulo = $" {task.CreatedAt:dd MMM hh:mm tt} -> {task.Activity.ToUpperInvariant()} -> {task.Priority}"
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
}
