using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

using LD.Contracts.Enums;
using MauiAppLogin.Controls;
using MauiAppLogin.Common.Imaging;
using MauiAppLogin.Models;
using MauiAppLogin.Services;
using MauiAppLogin.Views.Controls;
using MvvmHelpers.Commands;
using Plugin.Maui.OCR;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Command = MvvmHelpers.Commands.Command;

namespace MauiAppLogin.ViewModels;

[QueryProperty(nameof(Tipo), "tipo")]
public partial class RegisterLicenseViewModel : ObservableObject
{
    private readonly SecurityRegistrationContext _context;
    private readonly ILoaderService _loaderService;
    private readonly IDialogService _dialogService;

    public ILoaderService Loader => _loaderService;

    [ObservableProperty]
    private string tipo = "";

    [ObservableProperty]
    private string nombre = "";

    [ObservableProperty]
    private string licencia = "";

    [ObservableProperty]
    private DateTime vencimiento = DateTime.Today;

    [ObservableProperty]
    private string celular = "";

    [ObservableProperty]
    private ImageSource? previewImage;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<LicensePhotoItem> Photos { get; } = new();

    public ICommand CapturarCommand { get; }
    public ICommand GaleriaCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand SiguienteCommand { get; }
    public ICommand AtrasCommand { get; }
    public ICommand RemovePhotoCommand { get; }
    public ICommand ViewPhotoCommand { get; }

    public RegisterLicenseViewModel(
        SecurityRegistrationContext context,
        ILoaderService loaderService,
        IDialogService dialogService)
    {
        _context       = context;
        _loaderService = loaderService;
        _dialogService = dialogService;


        CapturarCommand    = new AsyncCommand(CapturarAsync);
        CancelarCommand    = new Command(Cancelar);
        SiguienteCommand   = new AsyncCommand(SiguienteAsync);
        AtrasCommand       = new AsyncCommand(AtrasAsync);
        RemovePhotoCommand = new MvvmHelpers.Commands.Command<LicensePhotoItem>(RemovePhoto);
        ViewPhotoCommand   = new MvvmHelpers.Commands.Command<LicensePhotoItem>(ViewPhoto);

        CapturarCommand = new AsyncCommand(CapturarAsync);
        GaleriaCommand = new AsyncCommand(SeleccionarGaleriaAsync);
        CancelarCommand = new Command(Cancelar);
        SiguienteCommand = new AsyncCommand(SiguienteAsync);
        AtrasCommand = new AsyncCommand(AtrasAsync);

        LoadFromContext();
    }

    private void LoadFromContext()
    {
        if (!string.IsNullOrEmpty(_context.Nombre))   Nombre   = _context.Nombre;
        if (!string.IsNullOrEmpty(_context.Licencia)) Licencia = _context.Licencia;
        if (_context.Vencimiento != default)          Vencimiento = _context.Vencimiento;
        if (!string.IsNullOrEmpty(_context.Celular))  Celular  = _context.Celular;

        Photos.Clear();
        foreach (var entry in _context.LicenciaFotos)
        {
            Photos.Add(new LicensePhotoItem
            {
                Bytes  = entry.Bytes,
                Orden  = entry.Orden,
                Source = ImageSource.FromStream(() => new MemoryStream(entry.Bytes)),
            });
        }
    }

    private void SaveToContext()
    {
        _context.Tipo      = Tipo;
        _context.Nombre    = Nombre;
        _context.Licencia  = Licencia;
        _context.Vencimiento = Vencimiento;
        _context.Celular   = Celular;

        _context.LicenciaFotos = Photos.Select(p => new SecurityPhotoEntry
        {
            Categoria = PhotoCategoria_e.Licencia,
            Orden     = p.Orden,
            Bytes     = p.Bytes,
        }).ToList();
    }

    private async Task CapturarAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await _dialogService.ShowInfoAsync("Cámara", "Este dispositivo no soporta captura de fotos.");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;

            await ProcesarFotoAsync(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task SeleccionarGaleriaAsync()
    {
        try
        {
            var photos = await MediaPicker.Default.PickPhotosAsync(new MediaPickerOptions
            {
                Title = "Selecciona una imagen de la licencia"
            });
            var photo = photos?.FirstOrDefault();

            if (photo is null) return;

            await ProcesarFotoAsync(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task ProcesarFotoAsync(FileResult photo)
    {
        try
        {
            _loaderService.Show("Procesando imagen...");

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(bytes);
            var text = ocrResult?.AllText ?? "";
            if (!string.IsNullOrWhiteSpace(text))
                ParseLicense(text);

            var bytesFinales = await ComprimirImagenAsync(bytes);
            var img = ImageSource.FromStream(() => new MemoryStream(bytesFinales));

            Photos.Add(new LicensePhotoItem
            {
                Bytes  = bytesFinales,
                Orden  = Photos.Count,
                Source = img,
            });

        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        finally
        {
            _loaderService.Hide();
        }
    }

    private static async Task<byte[]> ComprimirImagenAsync(byte[] bytes)
    {
        try
        {
            return await ImageCompressor.ComprimirAsync(bytes);
        }
        catch
        {
            return bytes;
        }
    }

    private void ParseLicense(string text)
    {
        var nombreLabel = Regex.Match(text,
            @"(?:NOMBRE|NAME)[:\s]+([A-ZÁÉÍÓÚÜÑ][A-ZÁÉÍÓÚÜÑ\s]{3,60})",
            RegexOptions.IgnoreCase);

        if (nombreLabel.Success)
        {
            Nombre = nombreLabel.Groups[1].Value.Trim();
        }
        else
        {
            var nombreBloque = Regex.Match(text,
                @"^([A-ZÁÉÍÓÚÜÑ]{2,}\s+[A-ZÁÉÍÓÚÜÑ]{2,}(?:\s+[A-ZÁÉÍÓÚÜÑ]{2,})*)\s*$",
                RegexOptions.Multiline);
            if (nombreBloque.Success)
                Nombre = nombreBloque.Groups[1].Value.Trim();
        }

        var licMatch = Regex.Match(text, @"\b(\d{6,12})\b");
        if (licMatch.Success)
            Licencia = licMatch.Groups[1].Value;
    }

    private void RemovePhoto(LicensePhotoItem? item)
    {
        if (item is null) return;
        Photos.Remove(item);
        for (int i = 0; i < Photos.Count; i++)
            Photos[i].Orden = i;
    }

    private void ViewPhoto(LicensePhotoItem? item)
    {
        if (item?.Source is null) return;
        (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
            ?.ShowPopup(new ImagePreviewPopup(item.Source));
    }

    private void Cancelar()
    {
        PreviewImage = null;
    }

    private async Task SiguienteAsync()
    {
        if (string.IsNullOrEmpty(Tipo)     ||
            string.IsNullOrEmpty(Licencia) ||
            string.IsNullOrEmpty(Nombre)   ||
            string.IsNullOrEmpty(Celular))
        {
            await _dialogService.ShowInfoAsync("Atención", "Llene todos los campos, por favor.");
            return;
        }

        if (Photos.Count < 2)
        {
            await _dialogService.ShowInfoAsync("Atención", "Agregue al menos 2 fotos de la licencia.");
            return;
        }

        SaveToContext();
        await Shell.Current.GoToAsync(nameof(RegisterVehicule));
    }

    private async Task AtrasAsync()
    {
        _context.Clear();
        await Shell.Current.GoToAsync("..");
    }
}

public class LicensePhotoItem
{
    public byte[] Bytes { get; set; } = Array.Empty<byte>();
    public int Orden { get; set; }
    public ImageSource? Source { get; set; }
}
