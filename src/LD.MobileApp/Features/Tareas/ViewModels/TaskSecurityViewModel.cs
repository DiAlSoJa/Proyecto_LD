using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class TaskSecurityViewModel : ObservableObject
{
    private readonly IPatioNotificacionService _notificacionService;
    private readonly ILoaderService _loaderService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<PatioNotificacion> tareas = new();

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private string errorMessage = "";

    public ICommand CargarCommand { get; }
    public ICommand CompletarTareaCommand { get; }
    public ICommand AtrasCommand { get; }

    public TaskSecurityViewModel(
        IPatioNotificacionService notificacionService,
        ILoaderService loaderService)
    {
        _notificacionService = notificacionService;
        _loaderService       = loaderService;

        CargarCommand       = new AsyncCommand(CargarAsync);
        CompletarTareaCommand = new AsyncCommand<PatioNotificacion>(CompletarTareaAsync);
        AtrasCommand        = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        _loaderService.Show("Cargando tareas...");
        ErrorMessage = "";
        HasError     = false;

        try
        {
            var lista = await _notificacionService.GetTodasAsync();
            Tareas = new ObservableCollection<PatioNotificacion>(
                lista.OrderByDescending(t => t.FechaHora));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al cargar tareas: {ex.Message}";
            HasError     = true;
            Tareas       = new ObservableCollection<PatioNotificacion>();
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task CompletarTareaAsync(PatioNotificacion? tarea)
    {
        if (tarea is null || tarea.Completada) return;

        _loaderService.Show("Completando tarea...");
        try
        {
            var ok = await _notificacionService.CompletarTareaAsync(tarea.Id);
            if (ok)
                await CargarAsync();
            else
                await Shell.Current.DisplayAlertAsync("Error", "No se pudo completar la tarea.", "OK");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");
}
