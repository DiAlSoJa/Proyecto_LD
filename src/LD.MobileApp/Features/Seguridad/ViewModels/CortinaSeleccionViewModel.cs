using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

public partial class CortinaSeleccionViewModel : ObservableObject
{
    private readonly IPatioService _patioService;
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
        IPatioService patioService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioService = patioService;
        _loaderService = loaderService;
        _context = context;

        CargarCommand   = new AsyncCommand(CargarAsync);
        SeleccionarCommand = new Command<Cortina>(Seleccionar);
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
        _loaderService.Show("Cargando cortinas...");
        try
        {
            var lista = await _patioService.GetCortinasDisponiblesAsync();
            Cortinas = new ObservableCollection<Cortina>(lista);
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
