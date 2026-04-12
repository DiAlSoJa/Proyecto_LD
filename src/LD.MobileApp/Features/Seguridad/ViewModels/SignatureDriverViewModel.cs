using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Models;
using MvvmHelpers.Commands;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

public partial class SignatureDriverViewModel : ObservableObject
{
    private readonly SecurityRegistrationContext _context;

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

    public SignatureDriverViewModel(SecurityRegistrationContext context)
    {
        _context = context;

        ClearCommand = new Command(ClearSignature);
        FinalizarCommand = new AsyncCommand(FinalizarAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);

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

            // Build the final request
            var request = SecurityRegistrationRequest.FromContext(_context);

            // TODO: send request to API via injected service
            // await _apiService.PostSecurityRegistrationAsync(request);

            await Shell.Current.DisplayAlertAsync("Listo", "Registro completado.", "OK");

            _context.Clear();
            await Shell.Current.GoToAsync("//dashboard");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AtrasAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
