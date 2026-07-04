using LD.Client.Services;
using LD.Contracts.Requests;
using LD.Contracts.Vehicle;
using LD.FormsX.Helpers;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace LDForms.Features.DockDelivery.Views;

public partial class NuevoVehiculoDockDeliveryView : Window
{
    private readonly VehicleService _vehicleService;
    private bool _editing;
    private string? _originalPlates;

    public NuevoVehiculoDockDeliveryView(VehicleService vehicleService)
    {
        InitializeComponent();
        _vehicleService = vehicleService;
    }

    public void SetVehicle(VehicleDto vehicle)
    {
        _editing = true;
        _originalPlates = vehicle.Placas;
        txtHeaderTitle.Text = "Editar vehículo";
        btnSave.Content = "Actualizar";

        ldPlacas.Text = vehicle.Placas;
        ldPlacas.IsReadOnly = true;
        ldNumeroVehiculo.Text = vehicle.NumeroVehiculo;
        ldNombre.Text = vehicle.Nombre;
        ldTipo.Text = vehicle.Tipo;
        ldCapacidad.Text = vehicle.Capacidad.ToString(CultureInfo.CurrentCulture);
        ldLong.Text = vehicle.Largo.ToString(CultureInfo.CurrentCulture);
        ldWight.Text = vehicle.Ancho.ToString(CultureInfo.CurrentCulture);
        ldHeight.Text = vehicle.Alto.ToString(CultureInfo.CurrentCulture);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ldPlacas.TextBoxElement.Focus();
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            btnSave.IsEnabled = false;

            if (!TryBuildRequest(out var request))
            {
                return;
            }

            var response = _editing
                ? await _vehicleService.UpdateVehicle(_originalPlates ?? request.Plates, request)
                : await _vehicleService.CreateVehicle(request);

            if (response.IsSuccess)
            {
                ToastHelper.ShowSuccess(
                    response.Message ?? "Vehículo guardado correctamente.",
                    "Vehículos");
                DialogResult = true;
                Close();
                return;
            }

            DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el vehículo.");
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            btnSave.IsEnabled = true;
        }
    }

    private bool TryBuildRequest(out VechicleRequest request)
    {
        request = new VechicleRequest();

        var plates = ldPlacas.Text.Trim();
        var vehicleNumber = ldNumeroVehiculo.Text.Trim();
        var name = ldNombre.Text.Trim();
        var type = ldTipo.Text.Trim();

        if (string.IsNullOrWhiteSpace(plates))
        {
            DialogHelper.ShowWarning("Las placas son obligatorias.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(vehicleNumber))
        {
            DialogHelper.ShowWarning("El número de vehículo es obligatorio.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            DialogHelper.ShowWarning("El nombre del vehículo es obligatorio.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            DialogHelper.ShowWarning("El tipo de vehículo es obligatorio.");
            return false;
        }

        if (!TryParseDecimal(ldCapacidad.Text, out var capacity))
        {
            DialogHelper.ShowWarning("La capacidad debe ser numérica.");
            return false;
        }

        if (!TryParseDecimal(ldLong.Text, out var length))
        {
            DialogHelper.ShowWarning("El largo debe ser numérico.");
            return false;
        }

        if (!TryParseDecimal(ldWight.Text, out var width))
        {
            DialogHelper.ShowWarning("El ancho debe ser numérico.");
            return false;
        }

        if (!TryParseDecimal(ldHeight.Text, out var height))
        {
            DialogHelper.ShowWarning("El alto debe ser numérico.");
            return false;
        }

        request = new VechicleRequest
        {
            Plates = plates,
            VehicleNumber = vehicleNumber,
            Name = name,
            Type = type,
            Capacity = capacity,
            Long = length,
            Wight = width,
            Height = height
        };

        return true;
    }

    private static bool TryParseDecimal(string? value, out decimal result)
    {
        var text = value?.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            result = 0;
            return true;
        }

        return decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out result)
            || decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
    }

    private void BtnCerrar_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }
}
