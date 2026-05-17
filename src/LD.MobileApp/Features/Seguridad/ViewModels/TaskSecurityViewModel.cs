using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using MauiAppLogin.Services;
using Microsoft.Maui.Media;
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
    public ICommand SelectTaskCommand { get; }
    public ICommand AtrasCommand { get; }

    public TaskSecurityViewModel(PatioClientService patioClientService, ILoaderService loaderService)
    {
        _patioClientService = patioClientService;
        _loaderService      = loaderService;

        CargarCommand     = new AsyncCommand(CargarAsync);
        SelectTaskCommand = new AsyncCommand<SecurityTaskDto>(SeleccionarTareaAsync);
        AtrasCommand      = new AsyncCommand(AtrasAsync);
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

    private async Task SeleccionarTareaAsync(SecurityTaskDto? tarea)
    {
        if (tarea is null) return;

        bool esAbrir = tarea.RegistrationStatus == RegistroEstado_e.CortinaAsignada;

        string title       = esAbrir ? "Abrir Cortina"    : "Cerrar Cortina";
        string confirmText = esAbrir ? "Abrir Cortina"    : "Cerrar Cortina";
        string message     = esAbrir
            ? $"¿Confirmas abrir la cortina {tarea.CortinaNumero ?? tarea.SecurityRegistrationId.ToString()}?"
            : $"¿Confirmas cerrar la cortina del vehículo {tarea.Placa}?";

        var confirm = await Shell.Current.DisplayAlertAsync(title, message, confirmText, "No");
        if (!confirm) return;

        var fotoBase64 = await CapturarFotoBase64Async();
        if (string.IsNullOrWhiteSpace(fotoBase64))
            return;

        _loaderService.Show(esAbrir ? "Abriendo cortina..." : "Cerrando registro...");
        try
        {
            var response = esAbrir
                ? await _patioClientService.AbrirCortinaAsync(tarea.SecurityTaskId, fotoBase64)
                : await _patioClientService.CerrarRegistroAsync(tarea.SecurityTaskId, fotoBase64);

            if (response.IsSuccess)
            {
                await CargarAsync();
                await Shell.Current.DisplayAlertAsync(
                    "Completado",
                    esAbrir ? "Cortina abierta correctamente." : "Registro cerrado correctamente.",
                    "OK");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", response.Message ?? "No se pudo completar la acción.", "OK");
            }
        }
        finally { _loaderService.Hide(); }
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");

    private static async Task<string?> CapturarFotoBase64Async()
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            await Shell.Current.DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de foto.", "OK");
            return null;
        }

        try
        {
            var foto = await MediaPicker.Default.CapturePhotoAsync();
            if (foto is null)
            {
                await Shell.Current.DisplayAlertAsync("Foto requerida", "Debes tomar una foto para continuar.", "OK");
                return null;
            }

            await using var stream = await foto.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"No se pudo capturar la foto: {ex.Message}", "OK");
            return null;
        }
    }
}
