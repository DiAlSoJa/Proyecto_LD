using CommunityToolkit.Mvvm.ComponentModel;
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

    public ICommand CargarCommand { get; }
    public ICommand SeleccionarCommand { get; }
    public ICommand ConfirmarCommand { get; }
    public ICommand CancelarCommand { get; }

    public CortinaSeleccionViewModel(
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
            _loaderService.Show("Obteniendo cortinas disponibles...");
            var response = await _patioClientService.GetCortinasDisponiblesAsync();
            var lista = response.IsSuccess && response.Data is not null
                ? response.Data.Select(d => new Cortina
                {
                    Id             = d.CortinaId,
                    Numero         = d.Numero,
                    Descripcion    = d.Descripcion,
                    EstaDisponible = d.EstaDisponible
                }).ToList()
                : [];

            Cortinas = new ObservableCollection<Cortina>(lista);
            if (!response.IsSuccess)
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudieron cargar las cortinas.");
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
