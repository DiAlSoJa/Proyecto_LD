using CommunityToolkit.Mvvm.ComponentModel;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class TaskSecurityViewModel : ObservableObject
{
    private readonly IPatioService _patioService;
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

    public TaskSecurityViewModel(IPatioService patioService, ILoaderService loaderService)
    {
        _patioService  = patioService;
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
            var lista = await _patioService.GetTasksAsync(soloPendientes: false);
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
            var ok = await _patioService.AbrirCortinaAsync(tarea.SecurityTaskId);
            if (ok)
                await CargarAsync();
            else
                await Shell.Current.DisplayAlertAsync("Error", "No se pudo completar la acción.", "OK");
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
            var ok = await _patioService.CerrarRegistroAsync(tarea.SecurityTaskId);
            if (ok)
                await CargarAsync();
            else
                await Shell.Current.DisplayAlertAsync("Error", "No se pudo cerrar el registro.", "OK");
        }
        finally { _loaderService.Hide(); }
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");
}
