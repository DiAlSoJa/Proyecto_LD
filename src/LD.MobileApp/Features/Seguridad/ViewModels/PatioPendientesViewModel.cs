using CommunityToolkit.Mvvm.ComponentModel;
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
    private ObservableCollection<VehiculoEnPatio> vehiculos = new();

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

        CargarCommand   = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new AsyncCommand<VehiculoEnPatio>(SeleccionarAsync);
        AtrasCommand    = new AsyncCommand(AtrasAsync);
    }

    public async Task InicializarAsync() => await CargarAsync();

    private async Task CargarAsync()
    {
        _loaderService.Show("Cargando vehículos...");
        try
        {
            var lista = await _patioService.GetVehiculosSinSalidaAsync();
            Vehiculos = new ObservableCollection<VehiculoEnPatio>(lista);
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task SeleccionarAsync(VehiculoEnPatio? vehiculo)
    {
        if (vehiculo is null) return;
        _context.VehiculoSeleccionado = vehiculo;
        await Shell.Current.GoToAsync(nameof(PatioDetallePage));
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("//dashboard");
}
