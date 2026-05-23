using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Client.Services;
using MauiAppLogin.Controls;
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
    private readonly IDialogService _dialogService;

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
        ILoaderService loaderService,
        IDialogService dialogService)
    {
        _context         = context;
        _securityService = securityService;
        _loaderService   = loaderService;
        _dialogService   = dialogService;

        ClearCommand     = new Command(ClearSignature);
        FinalizarCommand = new AsyncCommand(FinalizarAsync);
        AtrasCommand     = new AsyncCommand(AtrasAsync);

        LoadResumen();
    }

    private void LoadResumen()
    {
        ResumenTipo     = _context.Tipo;
        ResumenChofer   = _context.Nombre;
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
                await _dialogService.ShowInfoAsync("Atención", "Firme antes de finalizar.");
                return;
            }

            var fotos = new List<SecurityPhotoDto>();

            foreach (var entry in _context.LicenciaFotos)
                fotos.Add(new SecurityPhotoDto { Categoria = PhotoCategoria_e.Licencia, Orden = entry.Orden, Contenido = entry.Bytes });

            foreach (var entry in _context.VehiculoFotos)
                fotos.Add(new SecurityPhotoDto { Categoria = PhotoCategoria_e.Vehiculo, Orden = entry.Orden, Contenido = entry.Bytes });

            fotos.Add(new SecurityPhotoDto { Categoria = PhotoCategoria_e.Firma, Orden = 0, Contenido = _context.Firma });

            var request = new SecurityRegistrationRequest
            {
                Tipo        = _context.Tipo,
                Nombre      = _context.Nombre,
                Licencia    = _context.Licencia,
                Vencimiento = _context.Vencimiento,
                Celular     = _context.Celular,
                TipoVehiculo = _context.TipoVehiculo,
                Linea       = _context.Linea,
                Origen      = _context.Origen,
                Numero      = _context.Numero,
                Placa       = _context.Placa,
                Fotos       = fotos,
            };

            _dialogService.ShowBlocking("Guardando", "Enviando registro...");
            var response = await _securityService.RegisterAsync(request);

            if (!response.IsSuccess)
            {
                await _dialogService.ShowErrorAsync("Error", response.ErrorMessage ?? "Error desconocido");
                return;
            }

            await _dialogService.ShowSuccessAsync("Registro completado", "Control de Patio asignará una cortina.");
            _context.Clear();
            await Shell.Current.GoToAsync("//dashboard");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        finally
        {
            _dialogService.HideBlocking();
            IsBusy = false;
        }
    }

    private async Task AtrasAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
