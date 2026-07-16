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

public partial class CortinaSeleccionViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
    private readonly IDialogService _dialogService;
    private readonly ILoaderService _loaderService;
    private readonly PatioContext _context;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private ObservableCollection<CortinaDto> cortinas = new();

    [ObservableProperty]
    private CortinaDto? cortinaSeleccionada;

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
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioClientService = patioClientService;
        _dialogService      = dialogService;
        _loaderService      = loaderService;
        _context            = context;

        CargarCommand = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new MvvmHelpers.Commands.Command<CortinaDto>(Seleccionar);
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

            var response = await _patioClientService.GetCortinasDisponiblesAsync();
            if (!response.IsSuccess || response.Data is null)
            {
                Cortinas = new ObservableCollection<CortinaDto>();
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudieron cargar las cortinas.");
                return;
            }

            Cortinas = new ObservableCollection<CortinaDto>(response.Data);
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private void Seleccionar(CortinaDto cortina)
    {
        CortinaSeleccionada = cortina;
        PuedeConfirmar = true;
    }

    private async Task ConfirmarAsync()
    {
        if (CortinaSeleccionada is null || _context.VehiculoSeleccionado is null)
            return;

        _context.CortinaSeleccionada = new Cortina
        {
            Id = CortinaSeleccionada.CortinaId,
            Numero = CortinaSeleccionada.Numero,
            Descripcion = CortinaSeleccionada.Descripcion,
            EstaDisponible = CortinaSeleccionada.EstaDisponible
        };

        await Shell.Current.GoToAsync("..");
    }

    private async Task CancelarAsync()
        => await Shell.Current.GoToAsync("..");
}
