using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class TaskSecurityViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
    private readonly ILoaderService _loaderService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<SecurityTaskDto> tareas = new();

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private string errorMessage = "";

    public ICommand CargarCommand { get; }
    public ICommand AbrirCortinaCommand { get; }
    public ICommand CerrarRegistroCommand { get; }
    public ICommand AtrasCommand { get; }

    public TaskSecurityViewModel(PatioClientService patioClientService, ILoaderService loaderService)
    {
        _patioClientService  = patioClientService;
        _loaderService = loaderService;

        CargarCommand        = new AsyncCommand(CargarAsync);
        AbrirCortinaCommand  = new AsyncCommand<SecurityTaskDto>(AbrirCortinaAsync);
        CerrarRegistroCommand = new AsyncCommand<SecurityTaskDto>(CerrarRegistroAsync);
        AtrasCommand         = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        _loaderService.Show("Cargando tareas...");
        HasError     = false;
        ErrorMessage = "";

        try
        {
            var response = await _patioClientService.GetTasksAsync(soloPendientes: true);
            var lista = response.IsSuccess ? (response.Data ?? []) : [];
            Tareas = new ObservableCollection<SecurityTaskDto>(
                lista.OrderByDescending(t => t.CreatedAt));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al cargar tareas: {ex.Message}";
            HasError     = true;
            Tareas       = new ObservableCollection<SecurityTaskDto>();
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task AbrirCortinaAsync(SecurityTaskDto? tarea)
    {
        if (tarea is null || tarea.Completada || tarea.TipoAccion != "AbrirCortina") return;

        _loaderService.Show("Abriendo cortina...");
        try
        {
            var response = await _patioClientService.AbrirCortinaAsync(tarea.SecurityTaskId);
            var ok = response.IsSuccess;
            if (ok)
                await CargarAsync();
            else
                await Shell.Current.DisplayAlertAsync("Error", response.Message ?? "No se pudo completar la acción.", "OK");
        }
        finally { _loaderService.Hide(); }
    }

    private async Task CerrarRegistroAsync(SecurityTaskDto? tarea)
    {
        if (tarea is null || tarea.Completada || tarea.TipoAccion != "CerrarRegistro") return;

        var confirm = await Shell.Current.DisplayAlertAsync(
            "Cerrar registro",
            $"¿Confirmas que el vehículo {tarea.Placa} salió del patio?",
            "Sí, salió", "Cancelar");

        if (!confirm) return;

        _loaderService.Show("Cerrando registro...");
        try
        {
            var response = await _patioClientService.CerrarRegistroAsync(tarea.SecurityTaskId);
            var ok = response.IsSuccess;
            if (ok)
                await CargarAsync();
            else
                await Shell.Current.DisplayAlertAsync("Error", response.Message ?? "No se pudo cerrar el registro.", "OK");
        }
        finally { _loaderService.Hide(); }
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");
}
