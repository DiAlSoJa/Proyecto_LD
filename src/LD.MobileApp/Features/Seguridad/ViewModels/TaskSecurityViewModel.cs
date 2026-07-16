using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Controls;
using MauiAppLogin.Services;
using Microsoft.Maui.Media;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class TaskSecurityViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;

    [ObservableProperty]
    private ObservableCollection<SecurityTaskDto> tareas = new();

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public ILoaderService Loader => _loaderService;

    public ICommand CargarCommand { get; }
    public ICommand SelectTaskCommand { get; }
    public ICommand AtrasCommand { get; }

    public TaskSecurityViewModel(
        PatioClientService patioClientService,
        IDialogService dialogService,
        ILoaderService loaderService)
    {
        _patioClientService = patioClientService;
        _dialogService = dialogService;
        _loaderService = loaderService;

        CargarCommand = new AsyncCommand(CargarAsync);
        SelectTaskCommand = new AsyncCommand<SecurityTaskDto>(SeleccionarTareaAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        _loaderService.Show("Cargando tareas...");
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            var response = await _patioClientService.GetTasksAsync(soloPendientes: true);
            if (!response.IsSuccess)
            {
                Tareas = new ObservableCollection<SecurityTaskDto>();
                ErrorMessage = response.Message ?? "No se pudieron cargar las tareas.";
                HasError = true;
                await _dialogService.ShowErrorAsync("Error", ErrorMessage);
                return;
            }

            var lista = (response.Data ?? [])
                .Where(t => (t.TipoAccion == "AbrirCortina" || t.TipoAccion == "CerrarRegistro")
                            && !t.Completada)
                .OrderByDescending(t => t.FechaIniciada)
                .ThenByDescending(t => t.SecurityTaskId)
                .ToList();

            Tareas = new ObservableCollection<SecurityTaskDto>(lista);
        }
        catch (Exception ex)
        {
            Tareas = new ObservableCollection<SecurityTaskDto>();
            ErrorMessage = $"Error al cargar tareas: {ex.Message}";
            HasError = true;
            await _dialogService.ShowErrorAsync("Error", ErrorMessage);
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task SeleccionarTareaAsync(SecurityTaskDto? tarea)
    {
        if (tarea is null || tarea.Completada)
            return;

        if (tarea.TipoAccion != "AbrirCortina" && tarea.TipoAccion != "CerrarRegistro")
            return;

        var esAbrir = tarea.TipoAccion == "AbrirCortina";
        var confirm = await _dialogService.ShowWarningAsync(
            esAbrir ? "Abrir cortina" : "Cerrar cortina",
            esAbrir
                ? $"¿Confirmas abrir la cortina {tarea.CortinaNumero ?? tarea.SecurityRegistrationId.ToString()}?"
                : $"¿Confirmas cerrar la cortina del vehículo {tarea.Placa}?");

        if (!confirm)
            return;

        var fotoBase64 = await CapturarFotoBase64Async();
        if (string.IsNullOrWhiteSpace(fotoBase64))
            return;

        _loaderService.Show(esAbrir ? "Abriendo cortina..." : "Cerrando cortina...");
        try
        {
            var response = esAbrir
                ? await _patioClientService.AbrirCortinaAsync(tarea.SecurityTaskId, fotoBase64)
                : await _patioClientService.CerrarRegistroAsync(tarea.SecurityTaskId, fotoBase64);

            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo completar la acción.");
                return;
            }

            await CargarAsync();
            await _dialogService.ShowSuccessAsync(
                "Completado",
                esAbrir ? "Cortina abierta correctamente." : "Cortina cerrada correctamente.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo completar la acción: {ex.Message}");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");

    private async Task<string?> CapturarFotoBase64Async()
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await _dialogService.ShowErrorAsync("Cámara", "Este dispositivo no soporta captura de foto.");
            return null;
        }

        try
        {
            var foto = await MediaPicker.Default.CapturePhotoAsync();
            if (foto is null)
                return null;

            await using var stream = await foto.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo capturar la foto: {ex.Message}");
            return null;
        }
    }
}
