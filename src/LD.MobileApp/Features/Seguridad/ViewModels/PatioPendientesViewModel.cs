using CommunityToolkit.Mvvm.ComponentModel;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class PatioPendientesViewModel : ObservableObject
{
    private readonly IPatioService _patioService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<SecurityRegistrationDto> vehiculos = new();

    [ObservableProperty]
    private string errorMessage = "";

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private bool hasItems;

    public ICommand CargarCommand { get; }
    public ICommand SeleccionarCommand { get; }
    public ICommand AtrasCommand { get; }

    public PatioPendientesViewModel(
        IPatioService patioService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioService = patioService;
        _loaderService = loaderService;
        _context = context;

        CargarCommand    = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new AsyncCommand<SecurityRegistrationDto>(SeleccionarAsync);
        AtrasCommand     = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        _loaderService.Show("Cargando vehículos...");
        ErrorMessage = "";
        HasError     = false;

        try
        {
            var lista = await _patioService.GetVehiculosSinSalidaAsync();
            Vehiculos = new ObservableCollection<SecurityRegistrationDto>(lista);
            HasItems  = Vehiculos.Count > 0;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al cargar vehículos: {ex.Message}";
            HasError     = true;
            Vehiculos    = new ObservableCollection<SecurityRegistrationDto>();
            HasItems     = false;
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task SeleccionarAsync(SecurityRegistrationDto? registro)
    {
        if (registro is null) return;

        _context.VehiculoSeleccionado = new VehiculoEnPatio
        {
            Id           = registro.SecurityRegistrationId,
            Placa        = registro.Placa,
            HoraEntrada  = registro.CreatedAt,
            Operador     = registro.Nombre,
            TipoVehiculo = registro.TipoVehiculo,
            Linea        = registro.Linea,
            Status       = "Dentro"
        };

        await Shell.Current.GoToAsync(nameof(PatioDetallePage));
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("//dashboard");
}
