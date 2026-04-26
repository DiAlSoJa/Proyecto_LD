using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Windows.Input;

namespace MauiAppLogin.ViewModels;

public partial class PatioDetalleViewModel : ObservableObject
{
    private readonly IPatioService _patioService;
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
        IPatioService patioService,
        ILoaderService loaderService,
        PatioContext context)
    {
        _patioService  = patioService;
        _loaderService = loaderService;
        _context       = context;

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
            var ok = await _patioService.AsignarCortinaAsync(Vehiculo.Id, cortina.Id);
            if (!ok)
            {
                await Shell.Current.DisplayAlertAsync("Error", "No se pudo asignar la cortina.", "OK");
                return;
            }

            Vehiculo.CortinaAsignada = cortina.Numero;
            RefrescarCortina();

            await Shell.Current.DisplayAlertAsync(
                "Cortina asignada",
                $"Vehículo {Vehiculo.Placa} → Cortina {cortina.Numero}.\nTarea creada en Task Manager de Seguridad.",
                "OK");
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
