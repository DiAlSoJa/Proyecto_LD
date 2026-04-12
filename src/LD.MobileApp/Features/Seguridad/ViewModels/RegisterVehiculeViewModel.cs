using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppLogin.Models;
using MvvmHelpers.Commands;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

public partial class RegisterVehiculeViewModel : ObservableObject
{
    private readonly SecurityRegistrationContext _context;

    [ObservableProperty]
    private string tipoVehiculo = "";

    [ObservableProperty]
    private bool isCajaSelected;

    [ObservableProperty]
    private bool isTractorSelected;

    [ObservableProperty]
    private string linea = "";

    [ObservableProperty]
    private string origen = "";

    [ObservableProperty]
    private string numero = "";

    [ObservableProperty]
    private string placa = "";

    [ObservableProperty]
    private ImageSource? previewImage;

    [ObservableProperty]
    private ImageSource? thumb1;

    [ObservableProperty]
    private ImageSource? thumb2;

    [ObservableProperty]
    private bool isBusy;

    private byte[]? _foto1Bytes;
    private byte[]? _foto2Bytes;

    public ICommand SelectCajaCommand { get; }
    public ICommand SelectTractorCommand { get; }
    public ICommand CapturarCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }

    public RegisterVehiculeViewModel(SecurityRegistrationContext context)
    {
        _context = context;

        SelectCajaCommand = new Command(SelectCaja);
        SelectTractorCommand = new Command(SelectTractor);
        CapturarCommand = new AsyncCommand(CapturarAsync);
        CancelarCommand = new Command(Cancelar);
        SiguienteCommand = new AsyncCommand(SiguienteAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);

        LoadFromContext();
    }

    private void LoadFromContext()
    {
        if (!string.IsNullOrEmpty(_context.TipoVehiculo))
        {
            TipoVehiculo = _context.TipoVehiculo;
            IsCajaSelected = TipoVehiculo == "Caja";
            IsTractorSelected = TipoVehiculo == "Tractor";
        }
        if (!string.IsNullOrEmpty(_context.Linea)) Linea = _context.Linea;
        if (!string.IsNullOrEmpty(_context.Origen)) Origen = _context.Origen;
        if (!string.IsNullOrEmpty(_context.Numero)) Numero = _context.Numero;
        if (!string.IsNullOrEmpty(_context.Placa)) Placa = _context.Placa;

        _foto1Bytes = _context.VehiculoFoto1;
        _foto2Bytes = _context.VehiculoFoto2;

        if (_foto1Bytes is not null)
            Thumb1 = ImageSource.FromStream(() => new MemoryStream(_foto1Bytes));
        if (_foto2Bytes is not null)
            Thumb2 = ImageSource.FromStream(() => new MemoryStream(_foto2Bytes));
    }

    private void SaveToContext()
    {
        _context.TipoVehiculo = TipoVehiculo;
        _context.Linea = Linea;
        _context.Origen = Origen;
        _context.Numero = Numero;
        _context.Placa = Placa;
        _context.VehiculoFoto1 = _foto1Bytes;
        _context.VehiculoFoto2 = _foto2Bytes;
    }

    private void SelectCaja()
    {
        TipoVehiculo = "Caja";
        IsCajaSelected = true;
        IsTractorSelected = false;
    }

    private void SelectTractor()
    {
        TipoVehiculo = "Tractor";
        IsCajaSelected = false;
        IsTractorSelected = true;
    }

    private async Task CapturarAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var img = ImageSource.FromStream(() => new MemoryStream(bytes));
            PreviewImage = img;

            if (_foto1Bytes is null)
            {
                _foto1Bytes = bytes;
                Thumb1 = img;
            }
            else
            {
                _foto2Bytes = bytes;
                Thumb2 = img;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void Cancelar()
    {
        PreviewImage = null;
    }

    private async Task SiguienteAsync()
    {
        if (string.IsNullOrEmpty(TipoVehiculo) ||
            string.IsNullOrEmpty(Linea) ||
            string.IsNullOrEmpty(Placa))
        {
            await Shell.Current.DisplayAlertAsync("Atención", "Seleccione tipo y llene al menos línea y placa.", "OK");
            return;
        }

        SaveToContext();
        await Shell.Current.GoToAsync(nameof(SignatureDriver));
    }

    private async Task AtrasAsync()
    {
        SaveToContext();
        await Shell.Current.GoToAsync("..");
    }
}
