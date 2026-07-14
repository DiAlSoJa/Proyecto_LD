using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Location;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class CortinaSeleccionViewModel : ObservableObject
{
    private readonly LocationService _locationService;
    private readonly LookupService _lookupService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<LocationDto> cortinas = new();

    [ObservableProperty]
    private LocationDto? cortinaSeleccionada;

    [ObservableProperty]
    private bool puedeConfirmar;

    [ObservableProperty]
    private string loadingMessage = "Obteniendo cortinas disponibles...";

    public ICommand CargarCommand { get; }
    public ICommand SeleccionarCommand { get; }
    public ICommand ConfirmarCommand { get; }
    public ICommand CancelarCommand { get; }

    public CortinaSeleccionViewModel(
        LocationService locationService,
        LookupService lookupService,
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _locationService = locationService;
        _lookupService   = lookupService;
        _dialogService   = dialogService;
        _loaderService   = loaderService;
        _context         = context;

        CargarCommand = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new MvvmHelpers.Commands.Command<LocationDto>(Seleccionar);
        ConfirmarCommand = new AsyncCommand(ConfirmarAsync);
        CancelarCommand = new AsyncCommand(CancelarAsync);
    }

    public async Task InicializarAsync()
    {
        CortinaSeleccionada = null;
        PuedeConfirmar = false;
        await CargarAsync();
    }

    private async Task CargarAsync()
    {
        try
        {
            _loaderService.Show(LoadingMessage);

            if (string.IsNullOrWhiteSpace(UserData.Id))
            {
                Cortinas = new ObservableCollection<LocationDto>();
                await _dialogService.ShowErrorAsync("Error", "No se pudo identificar al usuario para cargar sus almacenes.");
                return;
            }

            var warehousesResponse = await _lookupService.GetWarehouseLookupByUser(UserData.Id);
            if (!warehousesResponse.IsSuccess || warehousesResponse.Data is null)
            {
                Cortinas = new ObservableCollection<LocationDto>();
                await _dialogService.ShowErrorAsync("Error", warehousesResponse.Message ?? "No se pudieron cargar los almacenes del usuario.");
                return;
            }

            var warehouseIds = warehousesResponse.Data
                .Select(w => int.TryParse(w.Key, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            if (warehouseIds.Count == 0)
            {
                Cortinas = new ObservableCollection<LocationDto>();
                await _dialogService.ShowWarningAsync("Sin almacenes", "No tienes almacenes asignados, por eso no hay cortinas disponibles.");
                return;
            }

            var locationResponses = await Task.WhenAll(
                warehouseIds.Select(id => _locationService.GetLocations()));

            var lista = locationResponses
                .Where(response => response.IsSuccess && response.Data is not null)
                .SelectMany(response => response.Data!)
                .Where(location => warehouseIds.Contains(location.WarehouseId) && location.EsTieneCortina && !location.Ocupado)
                .GroupBy(location => location.LocationId)
                .Select(group => group.First())
                .OrderBy(location => location.Ubicacion)
                .ToList();

            Cortinas = new ObservableCollection<LocationDto>(lista);

            var firstError = locationResponses.FirstOrDefault(response => !response.IsSuccess && !string.IsNullOrWhiteSpace(response.Message));
            if (firstError is not null)
                await _dialogService.ShowErrorAsync("Error", firstError.Message ?? "No se pudieron cargar algunas cortinas.");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private void Seleccionar(LocationDto location)
    {
        CortinaSeleccionada = location;
        PuedeConfirmar = true;
    }

    private async Task ConfirmarAsync()
    {
        if (CortinaSeleccionada is null || _context.VehiculoSeleccionado is null)
            return;

        _context.UbicacionSeleccionada = CortinaSeleccionada;

        var request = new LD.Contracts.Requests.LocationRequest
        {
            LocationId = CortinaSeleccionada.LocationId,
            WarehouseId = CortinaSeleccionada.WarehouseId,
            LocationName = CortinaSeleccionada.Ubicacion,
            IsActive = CortinaSeleccionada.Activo,
            IsFiscal = CortinaSeleccionada.EsFiscal,
            HasControlledTemperature = CortinaSeleccionada.ControlTemperatura,
            HasPaso = CortinaSeleccionada.EsTienePaso,
            HasCortina = CortinaSeleccionada.EsTieneCortina,
            Ocupado = true,
            Placas = _context.VehiculoSeleccionado.Placa
        };

        var response = await _locationService.UpdateLocation(CortinaSeleccionada.LocationId, request);
        if (!response.IsSuccess)
        {
            await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo asignar la cortina.");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
