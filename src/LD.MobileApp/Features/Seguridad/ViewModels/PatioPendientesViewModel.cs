using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class PatioPendientesViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;
    private readonly List<PatioMonitorDto> _vehiculosBase = new();

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<PatioMonitorDto> vehiculos = new();

    private string filtroTexto = string.Empty;

    public string FiltroTexto
    {
        get => filtroTexto;
        set
        {
            value ??= string.Empty;
            if (SetProperty(ref filtroTexto, value))
            {
                AplicarFiltro();
            }
        }
    }

    public string EmptyTitle =>
        string.IsNullOrWhiteSpace(FiltroTexto)
            ? "No hay vehículos en patio"
            : "No hay coincidencias";

    public string EmptyMessage =>
        string.IsNullOrWhiteSpace(FiltroTexto)
            ? "Registra un vehículo para comenzar."
            : "Prueba con otra placa, nombre, tipo o línea.";

    public ICommand CargarCommand { get; }
    public ICommand SeleccionarCommand { get; }
    public ICommand AtrasCommand { get; }

    public PatioPendientesViewModel(
        PatioClientService patioClientService,
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioClientService = patioClientService;
        _dialogService = dialogService;
        _loaderService = loaderService;
        _context = context;

        CargarCommand = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new AsyncCommand<PatioMonitorDto>(SeleccionarAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        try
        {
            _loaderService.Show("Obteniendo vehículos...");
            var response = await _patioClientService.GetPatioMonitorAsync();
            var lista = response.IsSuccess ? (response.Data ?? []) : [];

            _vehiculosBase.Clear();
            _vehiculosBase.AddRange(lista);
            AplicarFiltro();

            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudieron cargar los vehículos.");
            }
        }
        catch (Exception ex)
        {
            _vehiculosBase.Clear();
            AplicarFiltro();
            await _dialogService.ShowErrorAsync("Error", $"Error al cargar vehículos: {ex.Message}");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private void AplicarFiltro()
    {
        var texto = (FiltroTexto ?? string.Empty).Trim();

        var filtrados = string.IsNullOrWhiteSpace(texto)
            ? _vehiculosBase
            : _vehiculosBase.Where(registro => CoincideFiltro(registro, texto)).ToList();

        Vehiculos = new ObservableCollection<PatioMonitorDto>(filtrados);
        OnPropertyChanged(nameof(EmptyTitle));
        OnPropertyChanged(nameof(EmptyMessage));
    }

    private static bool CoincideFiltro(PatioMonitorDto registro, string texto)
    {
        return Contiene(registro.Placa, texto)
               || Contiene(registro.Nombre, texto)
               || Contiene(registro.Tipo, texto)
               || Contiene(registro.TipoVehiculo, texto)
               || Contiene(registro.Linea, texto)
               || Contiene(registro.Origen, texto)
               || Contiene(registro.Numero, texto)
               || Contiene(registro.CortinaNumero, texto)
               || Contiene(registro.CurrentStep, texto)
               || Contiene(registro.NextStep, texto)
               || Contiene(registro.StatusText, texto)
               || Contiene(registro.ProgressText, texto)
               || Contiene(registro.AlertText, texto)
               || registro.Steps.Any(paso =>
                    Contiene(paso.Label, texto)
                    || Contiene(paso.StateText, texto)
                    || Contiene(paso.Area, texto)
                    || Contiene(paso.Note, texto));
    }

    private static bool Contiene(string? source, string texto)
    {
        return !string.IsNullOrWhiteSpace(source)
               && source.Contains(texto, StringComparison.OrdinalIgnoreCase);
    }

    private async Task SeleccionarAsync(PatioMonitorDto? registro)
    {
        if (registro is null) return;

        _context.VehiculoSeleccionado = new VehiculoEnPatio
        {
            Id = registro.SecurityRegistrationId,
            Placa = registro.Placa,
            HoraEntrada = registro.CreatedAt,
            Operador = registro.Nombre,
            TipoOperacion = registro.Tipo,
            TipoVehiculo = registro.TipoVehiculo,
            Linea = registro.Linea,
            CortinaAsignada = registro.CortinaNumero,
            Estado = registro.Estado,
            Status = registro.StatusText
        };

        await Shell.Current.GoToAsync(nameof(PatioDetallePage));
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("//dashboard");
}
