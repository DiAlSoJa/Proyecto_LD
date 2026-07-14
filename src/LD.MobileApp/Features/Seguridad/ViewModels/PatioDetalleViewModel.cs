using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class PatioDetalleViewModel : ObservableObject
{
    private readonly LocationService _locationService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private VehiculoEnPatio? vehiculo;

    [ObservableProperty]
    private bool tieneCortina;

    [ObservableProperty]
    private string cortinaTexto = "Sin asignar";

    public ICommand AsignarCortinaCommand { get; }
    public ICommand AtrasCommand { get; }

    public PatioDetalleViewModel(
        LocationService locationService,
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _locationService = locationService;
        _dialogService   = dialogService;
        _loaderService   = loaderService;
        _context         = context;

        AsignarCortinaCommand = new AsyncCommand(AsignarCortinaAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);
    }

    public void Inicializar()
    {
        Vehiculo = _context.VehiculoSeleccionado;
        RefrescarCortina();
    }

    public async Task VerificarCortinaSeleccionadaAsync()
    {
        var ubicacion = _context.UbicacionSeleccionada;
        if (ubicacion is null || Vehiculo is null) return;

        _context.UbicacionSeleccionada = null;

        _loaderService.Show("Asignando ubicación...");
        try
        {
            var request = new LocationRequest
            {
                LocationId = ubicacion.LocationId,
                WarehouseId = ubicacion.WarehouseId,
                LocationName = ubicacion.Ubicacion,
                IsFiscal = ubicacion.EsFiscal,
                HasControlledTemperature = ubicacion.ControlTemperatura,
                HasPaso = ubicacion.EsTienePaso,
                HasCortina = ubicacion.EsTieneCortina,
                Ocupado = true,
                Placas = Vehiculo.Placa,
                IsActive = ubicacion.Activo
            };

            var response = await _locationService.UpdateLocation(ubicacion.LocationId, request);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo asignar la ubicación.");
                return;
            }

            Vehiculo.CortinaAsignada = ubicacion.Ubicacion;
            RefrescarCortina();

            await _dialogService.ShowSuccessAsync(
                "Ubicación asignada",
                $"Vehículo {Vehiculo.Placa} → Ubicación {ubicacion.Ubicacion}.\nSe marcó como ocupada y se guardaron las placas.");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task AsignarCortinaAsync()
    {
        _context.UbicacionSeleccionada = null;
        await Shell.Current.GoToAsync(nameof(CortinaSeleccionPage));
    }

    private void RefrescarCortina()
    {
        TieneCortina = !string.IsNullOrEmpty(Vehiculo?.CortinaAsignada);
        CortinaTexto = Vehiculo?.CortinaAsignada ?? "Sin asignar";
    }

    private async Task AtrasAsync()
        => await Shell.Current.GoToAsync("..");
}
