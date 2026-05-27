using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MvvmHelpers.Commands;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

public partial class SignatureDriverViewModel : ObservableObject
{
    private readonly SecurityRegistrationContext _context;
    private readonly SecurityService _securityService;
    private readonly ILoaderService _loaderService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string resumenTipo = "";

    [ObservableProperty]
    private string resumenChofer = "";

    [ObservableProperty]
    private string resumenVehiculo = "";

    public DrawingView? SignaturePad { get; set; }

    public ICommand ClearCommand { get; }
    public ICommand FinalizarCommand { get; }
    public ICommand AtrasCommand { get; }

    public SignatureDriverViewModel(
        SecurityRegistrationContext context,
        SecurityService securityService,
        ILoaderService loaderService)
    {
        _context         = context;
        _securityService = securityService;
        _loaderService   = loaderService;

        ClearCommand     = new Command(ClearSignature);
        FinalizarCommand = new AsyncCommand(FinalizarAsync);
        AtrasCommand     = new AsyncCommand(AtrasAsync);

        LoadResumen();
    }

    private void LoadResumen()
    {
        ResumenTipo = _context.Tipo;
        ResumenChofer = _context.Nombre;
        ResumenVehiculo = $"{_context.TipoVehiculo} — {_context.Placa}";
    }

    private void ClearSignature()
    {
        SignaturePad?.Lines.Clear();
    }

    private async Task FinalizarAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Export signature
            if (SignaturePad is not null && SignaturePad.Lines.Count > 0)
            {
                var imgStream = await SignaturePad.GetImageStream(600, 300);
                if (imgStream is not null)
                {
                    using var ms = new MemoryStream();
                    await imgStream.CopyToAsync(ms);
                    _context.Firma = ms.ToArray();
                }
            }

            if (_context.Firma is null || _context.Firma.Length == 0)
            {
                await Shell.Current.DisplayAlertAsync("Atención", "Firme antes de finalizar.", "OK");
                return;
            }

            var request = new SecurityRegistrationRequest
            {
                Tipo          = _context.Tipo,
                Nombre        = _context.Nombre,
                Licencia      = _context.Licencia,
                Vencimiento   = _context.Vencimiento,
                Celular       = _context.Celular,
                LicenciaFoto1 = _context.LicenciaFoto1,
                LicenciaFoto2 = _context.LicenciaFoto2,
                TipoVehiculo  = _context.TipoVehiculo,
                Linea         = _context.Linea,
                Origen        = _context.Origen,
                Numero        = _context.Numero,
                Placa         = _context.Placa,
                VehiculoFoto1 = _context.VehiculoFoto1,
                VehiculoFoto2 = _context.VehiculoFoto2,
                Firma         = _context.Firma
            };

            _loaderService.Show("Guardando registro...");
            var response = await _securityService.RegisterAsync(request);

            if (!response.IsSuccess)
            {
                await Shell.Current.DisplayAlertAsync("Error", response.ErrorMessage, "OK");
                return;
            }

            await Shell.Current.DisplayAlertAsync("Listo", "Registro completado. Control de Patio asignará una cortina.", "OK");
            _context.Clear();
            await Shell.Current.GoToAsync("//dashboard");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            _loaderService.Hide();
            IsBusy = false;
        }
    }

    private async Task AtrasAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
