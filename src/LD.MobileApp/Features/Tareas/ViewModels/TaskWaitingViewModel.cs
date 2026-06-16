using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.SignalR;
using MauiAppLogin.Controls;
using System.Text.Json;

namespace MauiAppLogin.ViewModels;

public partial class TaskWaitingViewModel : ObservableObject
{
    private readonly WarehouseTaskService _taskService;
    private readonly SignalRService _signalRService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private bool isWaiting;

    public TaskWaitingViewModel(
        WarehouseTaskService taskService,
        SignalRService signalRService,
        IDialogService dialogService)
    {
        _taskService   = taskService;
        _signalRService = signalRService;
        _dialogService  = dialogService;
    }

    public async Task OnNavigatedToAsync()
    {
        IsWaiting = true;
        _signalRService.NotificationReceived += HandleNotification;

        try
        {
            _dialogService.ShowBlocking("Conectando", "Marcando disponibilidad...");
            var result = await _taskService.MarkUserAvailableAsync();
            if (!result.IsSuccess)
                await _dialogService.ShowErrorAsync("Error", result.Message ?? "No se pudo marcar como disponible.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        finally
        {
            _dialogService.HideBlocking();
        }
    }

    public void OnNavigatedFrom()
    {
        _signalRService.NotificationReceived -= HandleNotification;
    }

    private void HandleNotification(HubNotification notification)
    {
        if (notification.Type != "task_assigned") return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            IsWaiting = false;
            _signalRService.NotificationReceived -= HandleNotification;

            WarehouseTaskDto? task = null;
            if (!string.IsNullOrWhiteSpace(notification.Payload))
            {
                try
                {
                    task = JsonSerializer.Deserialize<WarehouseTaskDto>(notification.Payload,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch { }
            }

            var parameters = new Dictionary<string, object>
            {
                { "TaskId", task?.WarehouseTaskId ?? 0 }
            };

            await Shell.Current.GoToAsync(nameof(TaskResolve), parameters);
        });
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        try
        {
            _dialogService.ShowBlocking("Cancelando", "Cancelando espera...");
            await _taskService.CancelWaitingAsync();
        }
        catch { }
        finally
        {
            _dialogService.HideBlocking();
        }

        IsWaiting = false;
        await Shell.Current.GoToAsync("..");
    }
}
