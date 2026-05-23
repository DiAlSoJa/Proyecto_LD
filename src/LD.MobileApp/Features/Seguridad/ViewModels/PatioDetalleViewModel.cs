using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class PatioDetalleViewModel : ObservableObject
{
    private readonly PatioClientService _patioClientService;
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
        PatioClientService patioClientService,
        IDialogService dialogService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioClientService = patioClientService;
        _dialogService      = dialogService;
        _loaderService      = loaderService;
        _context            = context;

        AsignarCortinaCommand = new AsyncCommand(AsignarCortinaAsync);
        AtrasCommand          = new AsyncCommand(AtrasAsync);
    }

    public void Inicializar()
    {
        Vehiculo = _context.VehiculoSeleccionado;
        RefrescarCortina();
    }

    public async Task VerificarCortinaSeleccionadaAsync()
    {
        var cortina = _context.CortinaSeleccionada;
        if (cortina is null || Vehiculo is null) return;

        _context.CortinaSeleccionada = null;

        _loaderService.Show("Asignando cortina...");
        try
        {
            var response = await _patioClientService.AsignarCortinaAsync(Vehiculo.Id, cortina.Id);
            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.Message ?? "No se pudo asignar la cortina.");
                return;
            }

            Vehiculo.CortinaAsignada = cortina.Numero;
            RefrescarCortina();

            await _dialogService.ShowSuccessAsync(
                "Cortina asignada",
                $"Vehículo {Vehiculo.Placa} → Cortina {cortina.Numero}.\nTarea creada en Task Manager de Seguridad.");
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private async Task AsignarCortinaAsync()
    {
        _context.CortinaSeleccionada = null;
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
