using System.Collections.ObjectModel;
using System.Linq;
using MauiAppLogin.Controls;
using MauiAppLogin.Services;

namespace MauiAppLogin;

public partial class BluetoothPrinterSettingsPage : ContentPage
{
    private readonly IDialogService _dialogService;
    private readonly IBluetoothPrinterService _bluetoothPrinterService;
    private readonly ObservableCollection<BluetoothPrinterDeviceInfo> _printers = new();
    private BluetoothPrinterDeviceInfo? _savedPrinter;
    private BluetoothPrinterDeviceInfo? _selectedPrinter;
    private bool _isBusy;
    private string _hintText = "Detecta una impresora una sola vez y luego quedara guardada.";

    public BluetoothPrinterSettingsPage(
        IDialogService dialogService,
        IBluetoothPrinterService bluetoothPrinterService)
    {
        InitializeComponent();
        _dialogService = dialogService;
        _bluetoothPrinterService = bluetoothPrinterService;
        BindingContext = this;
    }

    public ObservableCollection<BluetoothPrinterDeviceInfo> Printers => _printers;

    public BluetoothPrinterDeviceInfo? SelectedPrinter
    {
        get => _selectedPrinter;
        set
        {
            if (Equals(_selectedPrinter, value))
                return;

            _selectedPrinter = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSaveSelection));
        }
    }

    public string SavedPrinterText => _savedPrinter is null
        ? "Sin impresora guardada."
        : _savedPrinter.Summary;

    public string HintText => _hintText;

    public bool HasSavedPrinter => _savedPrinter is not null;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy == value)
                return;

            _isBusy = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanDetectPrinters));
            OnPropertyChanged(nameof(CanSaveSelection));
            OnPropertyChanged(nameof(CanClearPrinter));
        }
    }

    public bool CanDetectPrinters => !IsBusy;

    public bool CanSaveSelection => !IsBusy && SelectedPrinter is not null;

    public bool CanClearPrinter => !IsBusy && HasSavedPrinter;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshSavedPrinterAsync();
    }

    private async Task RefreshSavedPrinterAsync()
    {
        _savedPrinter = await _bluetoothPrinterService.GetSavedPrinterAsync();
        RefreshSavedPrinterBindings();

        if (_savedPrinter is null)
            SetHint("Aun no hay impresora guardada. Usa Detectar para buscar una Bluetooth cercana.");
        else
            SetHint("La impresora guardada se usara para imprimir sin volver a detectar.");

        if (_savedPrinter is not null && Printers.Count > 0)
        {
            var match = Printers.FirstOrDefault(printer =>
                string.Equals(printer.Address, _savedPrinter.Address, StringComparison.OrdinalIgnoreCase));

            SelectedPrinter = match;
        }
    }

    private async void OnDetectPrintersClicked(object sender, EventArgs e)
    {
        await LoadPrintersAsync();
    }

    private async Task LoadPrintersAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        SelectedPrinter = null;
        SetHint("Buscando impresoras Bluetooth...");

        try
        {
            var printers = await _bluetoothPrinterService.DiscoverPrintersAsync();
            Printers.Clear();
            foreach (var printer in printers)
                Printers.Add(printer);

            if (Printers.Count == 1)
            {
                SelectedPrinter = Printers[0];
                SetHint("Se encontro una impresora. Guarda la seleccion para usarla al imprimir.");
            }
            else if (_savedPrinter is not null)
            {
                var match = Printers.FirstOrDefault(printer =>
                    string.Equals(printer.Address, _savedPrinter.Address, StringComparison.OrdinalIgnoreCase));

                SelectedPrinter = match;
                SetHint(Printers.Count == 0
                    ? "No se encontraron impresoras Bluetooth."
                    : $"Se encontraron {Printers.Count} impresoras Bluetooth.");
            }
            else
            {
                SetHint(Printers.Count == 0
                    ? "No se encontraron impresoras Bluetooth."
                    : $"Se encontraron {Printers.Count} impresoras Bluetooth. Selecciona una para guardarla.");
            }

            RefreshSavedPrinterBindings();
        }
        catch (Exception ex)
        {
            SetHint("No se pudo detectar la impresora Bluetooth.");
            await _dialogService.ShowErrorAsync("Bluetooth", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnSavePrinterClicked(object sender, EventArgs e)
    {
        if (SelectedPrinter is null)
        {
            await _dialogService.ShowInfoAsync("Bluetooth", "Selecciona una impresora para guardarla.");
            return;
        }

        IsBusy = true;
        try
        {
            await _bluetoothPrinterService.SavePrinterAsync(SelectedPrinter);
            _savedPrinter = SelectedPrinter;
            RefreshSavedPrinterBindings();
            SetHint("Impresora guardada. Ya no sera necesario volver a detectarla para imprimir.");
            await _dialogService.ShowSuccessAsync("Bluetooth", "Impresora guardada correctamente.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Bluetooth", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnClearPrinterClicked(object sender, EventArgs e)
    {
        if (!HasSavedPrinter)
        {
            await _dialogService.ShowInfoAsync("Bluetooth", "No hay una impresora guardada.");
            return;
        }

        var confirm = await _dialogService.ShowWarningAsync("Bluetooth", "Se olvidara la impresora guardada. Deseas continuar?");
        if (!confirm)
            return;

        IsBusy = true;
        try
        {
            await _bluetoothPrinterService.ClearSavedPrinterAsync();
            _savedPrinter = null;
            SelectedPrinter = null;
            RefreshSavedPrinterBindings();
            SetHint("La impresora guardada se elimino. Vuelve a detectar una nueva si lo necesitas.");
            await _dialogService.ShowSuccessAsync("Bluetooth", "Impresora eliminada.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Bluetooth", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void SetHint(string text)
    {
        _hintText = text;
        OnPropertyChanged(nameof(HintText));
    }

    private void RefreshSavedPrinterBindings()
    {
        OnPropertyChanged(nameof(SavedPrinterText));
        OnPropertyChanged(nameof(HasSavedPrinter));
        OnPropertyChanged(nameof(CanDetectPrinters));
        OnPropertyChanged(nameof(CanSaveSelection));
        OnPropertyChanged(nameof(CanClearPrinter));
    }
}
