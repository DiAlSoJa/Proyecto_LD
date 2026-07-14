using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class CortinaSeleccionViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
    private readonly LookupService _lookupService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<Cortina> cortinas = new();

    [ObservableProperty]
    private Cortina? cortinaSeleccionada;

    [ObservableProperty]
    private bool puedeConfirmar;

    [ObservableProperty]
    private string loadingMessage = "Obteniendo cortinas disponibles...";

    public ICommand CargarCommand { get; }
    public ICommand SeleccionarCommand { get; }
    public ICommand ConfirmarCommand { get; }
    public ICommand CancelarCommand { get; }

    public CortinaSeleccionViewModel(
        PatioClientService patioClientService,
        LookupService lookupService,
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioClientService = patioClientService;
        _lookupService      = lookupService;
        _dialogService      = dialogService;
        _loaderService      = loaderService;
        _context            = context;

        CargarCommand      = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new MvvmHelpers.Commands.Command<Cortina>(Seleccionar);
        ConfirmarCommand   = new AsyncCommand(ConfirmarAsync);
        CancelarCommand    = new AsyncCommand(CancelarAsync);
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
                Cortinas = new ObservableCollection<Cortina>();
                await _dialogService.ShowErrorAsync("Error", "No se pudo identificar al usuario para cargar sus almacenes.");
                return;
            }

            var warehousesResponse = await _lookupService.GetWarehouseLookupByUser(UserData.Id);
            if (!warehousesResponse.IsSuccess || warehousesResponse.Data is null)
            {
                Cortinas = new ObservableCollection<Cortina>();
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
                Cortinas = new ObservableCollection<Cortina>();
                await _dialogService.ShowWarningAsync("Sin almacenes", "No tienes almacenes asignados, por eso no hay cortinas disponibles.");
                return;
            }

            var cortinasPorAlmacen = await Task.WhenAll(
                warehouseIds.Select(id => _patioClientService.GetCortinasDisponiblesAsync(id)));

            var lista = cortinasPorAlmacen
                .Where(response => response.IsSuccess && response.Data is not null)
                .SelectMany(response => response.Data!)
                .GroupBy(d => d.CortinaId)
                .Select(group => group.First())
                .Select(d => new Cortina
                {
                    Id             = d.CortinaId,
                    Numero         = d.Numero,
                    Descripcion    = d.Descripcion,
                    EstaDisponible = d.EstaDisponible
                })
                .OrderBy(c => c.Numero)
                .ToList();

            Cortinas = new ObservableCollection<Cortina>(lista);

            var firstError = cortinasPorAlmacen.FirstOrDefault(response => !response.IsSuccess && !string.IsNullOrWhiteSpace(response.Message));
            if (firstError is not null)
                await _dialogService.ShowErrorAsync("Error", firstError.Message ?? "No se pudieron cargar algunas cortinas.");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private void Seleccionar(Cortina cortina)
    {
        CortinaSeleccionada = cortina;
        PuedeConfirmar = true;
    }

    private async Task ConfirmarAsync()
    {
        if (CortinaSeleccionada is null) return;
        _context.CortinaSeleccionada = CortinaSeleccionada;
        await Shell.Current.GoToAsync("..");
    }

    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
