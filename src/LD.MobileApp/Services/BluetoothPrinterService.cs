using System.Text;
using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

#if ANDROID
using Android.Content;
using Android.Bluetooth;
using Android.OS;
using Java.Util;
#endif

namespace MauiAppLogin.Services;

public interface IBluetoothPrinterService
{
    Task PrintDamageReportAsync(DamageReportRequest report);
    Task<IReadOnlyList<BluetoothPrinterDeviceInfo>> DiscoverPrintersAsync();
    Task<BluetoothPrinterDeviceInfo?> GetSavedPrinterAsync();
    Task SavePrinterAsync(BluetoothPrinterDeviceInfo printer);
    Task ClearSavedPrinterAsync();
}

public class BluetoothPrinterService : IBluetoothPrinterService
{
    private const string PrinterAddressPreferenceKey = "BluetoothPrinterAddress";
    private const string PrinterNamePreferenceKey = "BluetoothPrinterName";
    private const int TicketTopBottomMarginDots = 400;
    private const byte QrModuleSize = 6;
    private const byte QrErrorCorrectionLevel = 50; // Q
    private static readonly Encoding PrinterEncoding = Encoding.ASCII;

#if ANDROID
    private const string SppUuid = "00001101-0000-1000-8000-00805F9B34FB";
#endif

    public async Task PrintDamageReportAsync(DamageReportRequest report)
    {
        if (report is null)
            throw new ArgumentNullException(nameof(report));

        var damageReportCode = DamageReportCodeGenerator.Normalize(report.DamageReportCode);
        if (string.IsNullOrWhiteSpace(damageReportCode))
        {
            damageReportCode = DamageReportCodeGenerator.Generate(
                report.ReportDate == default ? DateTime.Now : report.ReportDate);
            report.DamageReportCode = damageReportCode;
        }

        var qrUrl = DamageReportCodeGenerator.BuildPublicUrl(damageReportCode);
        if (string.IsNullOrWhiteSpace(qrUrl))
            throw new InvalidOperationException("No se pudo construir la URL del QR.");

#if ANDROID
        await EnsureBluetoothConnectPermissionAsync();

        var device = await ResolveSavedPrinterDeviceAsync();
        using var socket = await ConnectSocketAsync(device);

        try
        {
            using var output = socket.OutputStream;

            WriteCommand(output, 0x1B, 0x40);
            WriteFeedDots(output, TicketTopBottomMarginDots);
            WriteTextLine(output, "REPORTE DE DANOS", center: true, bold: true);
            WriteTextLine(output, damageReportCode, center: true);
            WriteQrCode(output, qrUrl);
            WriteTextLine(output, "rd.ld.com.mx", center: true);
            WriteFeedDots(output, TicketTopBottomMarginDots);
            output.Flush();
        }
        finally
        {
            socket.Close();
        }
#else
        throw new NotSupportedException("La impresion por Bluetooth solo esta disponible en Android.");
#endif
    }

    public async Task<IReadOnlyList<BluetoothPrinterDeviceInfo>> DiscoverPrintersAsync()
    {
#if ANDROID
        await EnsureBluetoothScanPermissionAsync();

        var adapter = BluetoothAdapter.DefaultAdapter
            ?? throw new InvalidOperationException("El dispositivo no soporta Bluetooth.");

        if (!adapter.IsEnabled)
            throw new InvalidOperationException("Activa Bluetooth para continuar.");

        var devices = await GetCandidateDevicesAsync(adapter);
        return devices
            .Select(ToPrinterInfo)
            .OrderByDescending(device => device.IsBonded)
            .ThenByDescending(device => device.IsLikelyPrinter)
            .ThenBy(device => device.DisplayName, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(device => device.Address, StringComparer.OrdinalIgnoreCase)
            .ToList();
#else
        throw new NotSupportedException("La deteccion de impresoras Bluetooth solo esta disponible en Android.");
#endif
    }

    public Task<BluetoothPrinterDeviceInfo?> GetSavedPrinterAsync()
    {
        var address = GetConfiguredPrinterAddress();
        if (string.IsNullOrWhiteSpace(address))
            return Task.FromResult<BluetoothPrinterDeviceInfo?>(null);

        var name = GetConfiguredPrinterName();
        var printer = new BluetoothPrinterDeviceInfo(
            address,
            string.IsNullOrWhiteSpace(name) ? null : name,
            false,
            IsLikelyPrinterName(name));

        return Task.FromResult<BluetoothPrinterDeviceInfo?>(printer);
    }

    public Task SavePrinterAsync(BluetoothPrinterDeviceInfo printer)
    {
        if (printer is null)
            throw new ArgumentNullException(nameof(printer));

        var address = printer.Address.Trim();
        if (string.IsNullOrWhiteSpace(address))
            throw new InvalidOperationException("La impresora seleccionada no tiene una direccion Bluetooth valida.");

        Preferences.Default.Set(PrinterAddressPreferenceKey, address);
        Preferences.Default.Set(PrinterNamePreferenceKey, printer.Name?.Trim() ?? string.Empty);

        return Task.CompletedTask;
    }

    public Task ClearSavedPrinterAsync()
    {
        Preferences.Default.Remove(PrinterAddressPreferenceKey);
        Preferences.Default.Remove(PrinterNamePreferenceKey);
        return Task.CompletedTask;
    }

    private static bool IsLikelyPrinterName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var printerKeywords = new[]
        {
            "printer",
            "pos",
            "thermal",
            "receipt",
            "zebra",
            "xprinter",
            "epson",
            "tsc",
            "rpp",
            "mpt",
            "xp"
        };

        return printerKeywords.Any(keyword => name.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    private static string GetConfiguredPrinterAddress()
    {
        return Preferences.Default.Get(PrinterAddressPreferenceKey, string.Empty).Trim();
    }

    private static string GetConfiguredPrinterName()
    {
        return Preferences.Default.Get(PrinterNamePreferenceKey, string.Empty).Trim();
    }

#if ANDROID
    private static async Task EnsureBluetoothConnectPermissionAsync()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
        {
            var connectGranted = await EnsurePermissionAsync<BluetoothConnectPermission>();
            if (!connectGranted)
                throw new InvalidOperationException("Se requiere permiso de Bluetooth para imprimir.");

            return;
        }

        // En versiones anteriores a Android 12 no se requiere permiso runtime para conectar.
    }

    private static async Task EnsureBluetoothScanPermissionAsync()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
        {
            var scanGranted = await EnsurePermissionAsync<BluetoothScanPermission>();
            if (!scanGranted)
                throw new InvalidOperationException("Se requiere permiso de Bluetooth para detectar impresoras.");

            return;
        }

        var locationGranted = await EnsurePermissionAsync<BluetoothLocationPermission>();
        if (!locationGranted)
            throw new InvalidOperationException("Se requiere permiso de ubicacion para buscar impresoras Bluetooth.");
    }

    private static async Task<bool> EnsurePermissionAsync<TPermission>()
        where TPermission : Permissions.BasePermission, new()
    {
        var status = await Permissions.CheckStatusAsync<TPermission>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<TPermission>();

        return status == PermissionStatus.Granted;
    }

    private static Task<BluetoothDevice> ResolveSavedPrinterDeviceAsync()
    {
        var configuredAddress = GetConfiguredPrinterAddress();
        if (string.IsNullOrWhiteSpace(configuredAddress))
            throw new InvalidOperationException(
                "No hay una impresora Bluetooth guardada. Abre la configuracion para detectarla y guardarla.");

        var adapter = BluetoothAdapter.DefaultAdapter
            ?? throw new InvalidOperationException("El dispositivo no soporta Bluetooth.");

        if (!adapter.IsEnabled)
            throw new InvalidOperationException("Activa Bluetooth para continuar.");

        return Task.FromResult(adapter.GetRemoteDevice(configuredAddress));
    }

    private static async Task<IReadOnlyList<BluetoothDevice>> GetCandidateDevicesAsync(BluetoothAdapter adapter)
    {
        var devices = new Dictionary<string, BluetoothDevice>(StringComparer.OrdinalIgnoreCase);
        AddDevices(devices, adapter.BondedDevices?.ToArray() ?? Array.Empty<BluetoothDevice>());

        var context = Android.App.Application.Context
            ?? throw new InvalidOperationException("No se pudo obtener el contexto de Android.");

        var discoveryFinished = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var receiver = new BluetoothDiscoveryReceiver(
            device => AddCandidateDevice(devices, device),
            () => discoveryFinished.TrySetResult(true));

        var filter = new IntentFilter(BluetoothDevice.ActionFound);
        filter.AddAction(BluetoothAdapter.ActionDiscoveryFinished);

        context.RegisterReceiver(receiver, filter);
        try
        {
            if (adapter.IsDiscovering)
                adapter.CancelDiscovery();

            if (!adapter.StartDiscovery())
                throw new InvalidOperationException("No se pudo iniciar el escaneo Bluetooth.");

            var finishedTask = await Task.WhenAny(discoveryFinished.Task, Task.Delay(TimeSpan.FromSeconds(15)));
            if (finishedTask != discoveryFinished.Task)
                adapter.CancelDiscovery();

            return devices.Values.ToList();
        }
        finally
        {
            try
            {
                context.UnregisterReceiver(receiver);
            }
            catch
            {
                // Ignorar si ya se desregistro desde la plataforma.
            }

            if (adapter.IsDiscovering)
                adapter.CancelDiscovery();
        }
    }

    private static void AddDevices(IDictionary<string, BluetoothDevice> devices, IEnumerable<BluetoothDevice> items)
    {
        foreach (var device in items)
            AddCandidateDevice(devices, device);
    }

    private static void AddCandidateDevice(IDictionary<string, BluetoothDevice> devices, BluetoothDevice? device)
    {
        if (device is null || string.IsNullOrWhiteSpace(device.Address))
            return;

        devices[device.Address] = device;
    }

    private static BluetoothPrinterDeviceInfo ToPrinterInfo(BluetoothDevice device)
    {
        var name = device.Name?.Trim();
        return new BluetoothPrinterDeviceInfo(
            device.Address,
            string.IsNullOrWhiteSpace(name) ? null : name,
            device.BondState == Bond.Bonded,
            IsLikelyPrinterName(name));
    }

    private sealed class BluetoothDiscoveryReceiver : BroadcastReceiver
    {
        private readonly Action<BluetoothDevice> _onDeviceFound;
        private readonly Action _onDiscoveryFinished;

        public BluetoothDiscoveryReceiver(Action<BluetoothDevice> onDeviceFound, Action onDiscoveryFinished)
        {
            _onDeviceFound = onDeviceFound;
            _onDiscoveryFinished = onDiscoveryFinished;
        }

        public override void OnReceive(Context? context, Intent? intent)
        {
            var action = intent?.Action;
            if (string.IsNullOrWhiteSpace(action))
                return;

            if (action == BluetoothDevice.ActionFound)
            {
                var device = intent?.GetParcelableExtra(BluetoothDevice.ExtraDevice) as BluetoothDevice;
                if (device is not null)
                    _onDeviceFound(device);

                return;
            }

            if (action == BluetoothAdapter.ActionDiscoveryFinished)
                _onDiscoveryFinished();
        }
    }

    private static async Task<BluetoothSocket> ConnectSocketAsync(BluetoothDevice device)
    {
        var uuid = UUID.FromString(SppUuid);

        var secureSocket = device.CreateRfcommSocketToServiceRecord(uuid);
        try
        {
            await Task.Run(() => secureSocket.Connect());
            return secureSocket;
        }
        catch
        {
            secureSocket.Close();
        }

        var insecureSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
        try
        {
            await Task.Run(() => insecureSocket.Connect());
            return insecureSocket;
        }
        catch
        {
            insecureSocket.Close();
            throw;
        }
    }

    private static void WriteTextLine(Stream stream, string text, bool center = false, bool bold = false)
    {
        WriteCommand(stream, 0x1B, 0x61, center ? (byte)1 : (byte)0);
        WriteCommand(stream, 0x1B, 0x45, bold ? (byte)1 : (byte)0);
        WriteBytes(stream, PrinterEncoding.GetBytes(text ?? string.Empty));
        WriteBytes(stream, new byte[] { 0x0A });
    }

    private static void WriteFeed(Stream stream, byte lines)
    {
        WriteCommand(stream, 0x1B, 0x64, lines);
    }

    private static void WriteFeedDots(Stream stream, int dots)
    {
        var remainingDots = Math.Max(0, dots);
        while (remainingDots > 0)
        {
            var chunk = (byte)Math.Min(255, remainingDots);
            WriteCommand(stream, 0x1B, 0x4A, chunk);
            remainingDots -= chunk;
        }
    }

    // Comando QR nativo ESC/POS. Evita que la impresora interprete los bytes del bitmap como texto basura.
    private static void WriteQrCode(Stream stream, string qrValue)
    {
        var data = PrinterEncoding.GetBytes(qrValue ?? string.Empty);
        if (data.Length == 0)
            throw new InvalidOperationException("No se pudo generar el contenido del codigo QR.");

        // Selecciona modelo 2.
        WriteCommand(stream, 0x1D, 0x28, 0x6B, 0x04, 0x00, 0x31, 0x41, 0x32, 0x00);
        // Ajusta el tamano del modulo.
        WriteCommand(stream, 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, QrModuleSize);
        // Nivel de correccion Q.
        WriteCommand(stream, 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, QrErrorCorrectionLevel);

        var payloadLength = data.Length + 3;
        WriteCommand(
            stream,
            0x1D,
            0x28,
            0x6B,
            (byte)(payloadLength & 0xFF),
            (byte)(payloadLength >> 8),
            0x31,
            0x50,
            0x30);
        WriteBytes(stream, data);

        WriteCommand(stream, 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30);
    }

    private static void WriteCommand(Stream stream, params byte[] command)
    {
        stream.Write(command, 0, command.Length);
    }

    private static void WriteBytes(Stream stream, byte[] bytes)
    {
        stream.Write(bytes, 0, bytes.Length);
    }
#endif
}

#if ANDROID
public sealed class BluetoothConnectPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (Android.Manifest.Permission.BluetoothConnect, true)
    ];
}

public sealed class BluetoothScanPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (Android.Manifest.Permission.BluetoothScan, true)
    ];
}

public sealed class BluetoothLocationPermission : Permissions.BasePlatformPermission
{
    public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
    [
        (Android.Manifest.Permission.AccessFineLocation, true)
    ];
}
#endif
