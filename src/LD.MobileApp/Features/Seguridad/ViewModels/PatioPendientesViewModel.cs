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

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<SecurityRegistrationDto> vehiculos = new();

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
        _dialogService      = dialogService;
        _loaderService      = loaderService;
        _context            = context;

        CargarCommand      = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new AsyncCommand<SecurityRegistrationDto>(SeleccionarAsync);
        AtrasCommand       = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        try
        {
            _loaderService.Show("Obteniendo vehículos...");
            var response = await _patioClientService.GetVehiculosSinSalidaAsync();
            var lista = response.IsSuccess ? (response.Data ?? []) : [];
            Vehiculos = new ObservableCollection<SecurityRegistrationDto>(lista);
            if (!response.IsSuccess)
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudieron cargar los vehículos.");
        }
        catch (Exception ex)
        {
            Vehiculos = new ObservableCollection<SecurityRegistrationDto>();
            await _dialogService.ShowErrorAsync("Error", $"Error al cargar vehículos: {ex.Message}");
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
            CortinaAsignada = registro.CortinaNumero,
            Status       = "Dentro"
        };

        await Shell.Current.GoToAsync(nameof(PatioDetallePage));
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("//dashboard");
}
