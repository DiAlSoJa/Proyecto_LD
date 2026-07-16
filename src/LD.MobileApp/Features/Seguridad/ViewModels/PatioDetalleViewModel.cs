using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using MauiAppLogin;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using Microsoft.Maui.Media;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class PatioDetalleViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;
    private List<SecurityTaskDto> tareas = new();
    private static readonly string[] AbrirCortinaTipos = new[] { "AbrirCortina" };
    private static readonly string[] IniciarOperacionTipos = new[] { "IniciarOperacion", "ComenzarOperacion" };
    private static readonly string[] FinalizarOperacionTipos = new[] { "FinalizarOperacion", "TerminarOperacion" };
    private static readonly string[] CerrarRegistroTipos = new[] { "CerrarRegistro" };

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private VehiculoEnPatio? vehiculo;

    public string IniciarOperacionTexto => $"Iniciar operación{ObtenerReferenciaOperacion()}";

    [ObservableProperty]
    private bool tieneCortina;

    [ObservableProperty]
    private string cortinaTexto = "Sin asignar";

    [ObservableProperty]
    private bool puedeIniciarOperacion;

    [ObservableProperty]
    private bool puedeFinalizarOperacion;

    [ObservableProperty]
    private bool puedeCerrarCortina;

    public ICommand AsignarCortinaCommand { get; }
    public ICommand IniciarOperacionCommand { get; }
    public ICommand FinalizarOperacionCommand { get; }
    public ICommand CerrarCortinaCommand { get; }
    public ICommand AtrasCommand { get; }

    public PatioDetalleViewModel(
        PatioClientService patioClientService,
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioClientService = patioClientService;
        _dialogService = dialogService;
        _loaderService = loaderService;
        _context = context;

        AsignarCortinaCommand = new AsyncCommand(AsignarCortinaAsync);
        IniciarOperacionCommand = new AsyncCommand(IniciarOperacionAsync);
        FinalizarOperacionCommand = new AsyncCommand(FinalizarOperacionAsync);
        CerrarCortinaCommand = new AsyncCommand(CerrarCortinaAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync()
    {
        Vehiculo = _context.VehiculoSeleccionado;
        if (Vehiculo is null)
        {
            ActualizarEstadoLocal();
            await _dialogService.ShowErrorAsync("Error", "No se encontró un vehículo seleccionado.");
            return;
        }

        OnPropertyChanged(nameof(IniciarOperacionTexto));

        _loaderService.Show("Cargando detalle del vehículo...");
        try
        {
            await RefrescarEstadoAsync();
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    public async Task VerificarCortinaSeleccionadaAsync()
    {
        var cortina = _context.CortinaSeleccionada;
        if (cortina is null || Vehiculo is null)
            return;

        _context.CortinaSeleccionada = null;

        _loaderService.Show("Asignando cortina...");
        try
        {
            var response = await _patioClientService.AsignarCortinaAsync(Vehiculo.Id, cortina.Id);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo asignar la cortina.");
                return;
            }

            Vehiculo.CortinaAsignada = cortina.Numero;
            RefrescarCortina();
            await RefrescarEstadoAsync();

            await _dialogService.ShowSuccessAsync(
                "Cortina asignada",
                $"Vehículo {Vehiculo.Placa} -> Cortina {cortina.Numero}.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo asignar la cortina: {ex.Message}");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task RefrescarEstadoAsync()
    {
        if (Vehiculo is null)
        {
            ActualizarEstadoLocal();
            return;
        }

        try
        {
            var response = await _patioClientService.GetTasksAsync(
                soloPendientes: false,
                securityRegistrationId: Vehiculo.Id);

            if (!response.IsSuccess)
            {
                tareas = new List<SecurityTaskDto>();
                ActualizarEstadoLocal();
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudieron cargar las tareas del vehículo.");
                return;
            }

            tareas = (response.Data ?? [])
                .OrderByDescending(t => t.FechaIniciada)
                .ThenByDescending(t => t.SecurityTaskId)
                .ToList();

            var estadoDesdeTarea = tareas.Select(t => t.RegistrationStatus).FirstOrDefault();
            if (estadoDesdeTarea != default)
            {
                Vehiculo.Estado = estadoDesdeTarea;
            }

            ActualizarEstadoLocal();
        }
        catch (Exception ex)
        {
            tareas = new List<SecurityTaskDto>();
            ActualizarEstadoLocal();
            await _dialogService.ShowErrorAsync("Error", $"No se pudieron cargar las tareas del vehículo: {ex.Message}");
        }
    }

    private async Task IniciarOperacionAsync()
    {
        if (Vehiculo is null)
            return;

        var tarea = ObtenerTareaParaIniciarOperacion();
        if (tarea is null)
        {
            await _dialogService.ShowErrorAsync("Error", "No hay una operación lista para iniciar.");
            return;
        }

        var confirmar = await _dialogService.ShowWarningAsync(
            "Iniciar operación",
            $"¿Confirmas iniciar la operación del vehículo {Vehiculo.Placa}?");
        if (!confirmar)
            return;

        var fotoBase64 = await CapturarFotoBase64Async();
        if (string.IsNullOrWhiteSpace(fotoBase64))
            return;

        _loaderService.Show("Iniciando operación...");
        try
        {
            var response = await _patioClientService.IniciarOperacionAsync(tarea.SecurityTaskId, fotoBase64);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo iniciar la operación.");
                return;
            }

            await RefrescarEstadoAsync();
            await _dialogService.ShowSuccessAsync("Operación iniciada", "La operación quedó iniciada.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo iniciar la operación: {ex.Message}");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task FinalizarOperacionAsync()
    {
        if (Vehiculo is null)
            return;

        var tarea = ObtenerTareaParaFinalizarOperacion();
        if (tarea is null)
        {
            await _dialogService.ShowErrorAsync("Error", "No hay una operación lista para finalizar.");
            return;
        }

        var confirmar = await _dialogService.ShowWarningAsync(
            "Finalizar operación",
            $"¿Confirmas finalizar la operación del vehículo {Vehiculo.Placa}?");
        if (!confirmar)
            return;

        var fotoBase64 = await CapturarFotoBase64Async();
        if (string.IsNullOrWhiteSpace(fotoBase64))
            return;

        _loaderService.Show("Finalizando operación...");
        try
        {
            var response = await _patioClientService.FinalizarOperacionAsync(tarea.SecurityTaskId, fotoBase64);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo finalizar la operación.");
                return;
            }

            await RefrescarEstadoAsync();
            await _dialogService.ShowSuccessAsync("Operación finalizada", "La operación quedó finalizada.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo finalizar la operación: {ex.Message}");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task CerrarCortinaAsync()
    {
        if (Vehiculo is null)
            return;

        var confirmar = await _dialogService.ShowWarningAsync(
            "Cerrar cortina",
            "¿Deseas enviar esta cortina a cerrar?");
        if (!confirmar)
            return;

        _loaderService.Show("Generando cierre de cortina...");
        try
        {
            var response = await _patioClientService.GenerarCierreCortinaAsync(Vehiculo.Id);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo generar el cierre de cortina.");
                return;
            }

            await RefrescarEstadoAsync();
            await _dialogService.ShowSuccessAsync("Cierre generado", "Ahora aparecerá en Seguridad para cerrar la cortina.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", $"No se pudo generar el cierre de cortina: {ex.Message}");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task AsignarCortinaAsync()
    {
        _context.CortinaSeleccionada = null;
        await Shell.Current.GoToAsync(nameof(CortinaSeleccionPage));
    }

    private void RefrescarCortina()
    {
        TieneCortina = !string.IsNullOrWhiteSpace(Vehiculo?.CortinaAsignada);
        CortinaTexto = Vehiculo?.CortinaAsignada ?? "Sin asignar";
    }

    private string ObtenerReferenciaOperacion()
    {
        var tipoOperacion = Vehiculo?.TipoOperacion?.Trim();
        if (string.IsNullOrWhiteSpace(tipoOperacion))
            return string.Empty;

        if (string.Equals(tipoOperacion, "Carga", StringComparison.OrdinalIgnoreCase))
            return " de carga";

        if (string.Equals(tipoOperacion, "Descarga", StringComparison.OrdinalIgnoreCase))
            return " de descarga";

        return $" de {tipoOperacion}";
    }

    private void ActualizarEstadoLocal()
    {
        if (Vehiculo is null)
        {
            TieneCortina = false;
            CortinaTexto = "Sin asignar";
            PuedeIniciarOperacion = false;
            PuedeFinalizarOperacion = false;
            PuedeCerrarCortina = false;
            return;
        }

        RefrescarCortina();

        var aperturaTarea = ObtenerTareaMasReciente(AbrirCortinaTipos, completada: true);
        var aperturaCompletada = aperturaTarea?.Completada == true;

        var inicioCompletado = aperturaCompletada
            ? ObtenerTareaMasRecienteDesde(IniciarOperacionTipos, completada: true, desde: aperturaTarea!.FechaCompletada)
              ?? ObtenerTareaMasRecienteDesde(IniciarOperacionTipos, completada: true, desde: null)
            : null;

        var finalizacionCompletada = inicioCompletado is not null
            ? ObtenerTareaMasRecienteDesde(FinalizarOperacionTipos, completada: true, desde: inicioCompletado.FechaCompletada)
              ?? ObtenerTareaMasRecienteDesde(FinalizarOperacionTipos, completada: true, desde: null)
            : null;

        var cierrePendiente = finalizacionCompletada is not null
            ? ObtenerTareaMasRecienteDesde(CerrarRegistroTipos, completada: false, desde: finalizacionCompletada.FechaCompletada)
              ?? ObtenerTareaMasRecienteDesde(CerrarRegistroTipos, completada: false, desde: null)
            : null;

        PuedeIniciarOperacion = aperturaCompletada && inicioCompletado is null;
        PuedeFinalizarOperacion = inicioCompletado is not null && finalizacionCompletada is null;
        PuedeCerrarCortina = finalizacionCompletada is not null && cierrePendiente is null;
    }

    private SecurityTaskDto? ObtenerTareaParaIniciarOperacion()
    {
        var aperturaTarea = ObtenerTareaMasReciente(AbrirCortinaTipos, completada: true);
        if (aperturaTarea is null)
            return null;

        return ObtenerTareaMasRecienteDesde(IniciarOperacionTipos, completada: false, desde: aperturaTarea.FechaCompletada)
               ?? ObtenerTareaMasRecienteDesde(IniciarOperacionTipos, completada: false, desde: null)
               ?? aperturaTarea;
    }

    private SecurityTaskDto? ObtenerTareaParaFinalizarOperacion()
    {
        var aperturaTarea = ObtenerTareaMasReciente(AbrirCortinaTipos, completada: true);
        if (aperturaTarea is null)
            return null;

        var inicioTarea = ObtenerTareaMasRecienteDesde(IniciarOperacionTipos, completada: true, desde: aperturaTarea.FechaCompletada)
            ?? ObtenerTareaMasRecienteDesde(IniciarOperacionTipos, completada: true, desde: null);
        if (inicioTarea is null)
            return null;

        return ObtenerTareaMasRecienteDesde(FinalizarOperacionTipos, completada: false, desde: inicioTarea.FechaCompletada)
               ?? ObtenerTareaMasRecienteDesde(FinalizarOperacionTipos, completada: false, desde: null)
               ?? inicioTarea;
    }

    private SecurityTaskDto? ObtenerTareaMasReciente(string tipoAccion, bool? completada = null, DateTime? desde = null)
        => ObtenerTareaMasRecienteDesde(new[] { tipoAccion }, completada, desde);

    private SecurityTaskDto? ObtenerTareaMasReciente(string[] tiposAccion, bool? completada = null, DateTime? desde = null)
        => ObtenerTareaMasRecienteDesde(tiposAccion, completada, desde);

    private SecurityTaskDto? ObtenerTareaMasRecienteDesde(string[] tiposAccion, bool? completada, DateTime? desde)
    {
        var query = tareas.Where(t => tiposAccion.Contains(t.TipoAccion));
        if (completada.HasValue)
        {
            query = query.Where(t => t.Completada == completada.Value);
        }

        if (desde.HasValue)
        {
            query = query.Where(t => t.FechaIniciada >= desde.Value);
        }

        return query
            .OrderByDescending(t => t.FechaIniciada)
            .ThenByDescending(t => t.SecurityTaskId)
            .FirstOrDefault();
    }

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

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");
}
