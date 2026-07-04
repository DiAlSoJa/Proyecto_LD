using LD.Client.Services;
using LD.Contracts.Driver;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace LDForms.Features.DockDelivery.Views;

public partial class NuevoChoferDockDeliveryView : Window
{
    private readonly DriverService _driverService;
    private bool _editing;
    private int _driverId;

    public NuevoChoferDockDeliveryView(DriverService driverService)
    {
        InitializeComponent();
        _driverService = driverService;
    }

    public void SetDriver(DriverDto driver)
    {
        _editing = true;
        _driverId = driver.DriverId;
        txtHeaderTitle.Text = "Editar chofer";
        btnSave.Content = "Actualizar";

        ldDriverNumber.Text = driver.DriverNumber;
        ldFullName.Text = driver.FullName;
        ldLicence.Text = driver.Licence;
        ldIMSS.Text = driver.IMSS;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ldDriverNumber.TextBoxElement.Focus();
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
                ? await _driverService.UpdateDriver(_driverId, request)
                : await _driverService.CreateDriver(request);

            if (response.IsSuccess)
            {
                ToastHelper.ShowSuccess(
                    response.Message ?? "Chofer guardado correctamente.",
                    "Choferes");
                DialogResult = true;
                Close();
                return;
            }

            DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo guardar el chofer.");
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

    private bool TryBuildRequest(out DriverRequest request)
    {
        request = new DriverRequest();

        var driverNumber = ldDriverNumber.Text.Trim();
        var fullName = ldFullName.Text.Trim();
        var licence = ldLicence.Text.Trim();
        var imss = ldIMSS.Text.Trim();

        if (string.IsNullOrWhiteSpace(driverNumber))
        {
            DialogHelper.ShowWarning("El número de chofer es obligatorio.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            DialogHelper.ShowWarning("El nombre completo es obligatorio.");
            return false;
        }

        request = new DriverRequest
        {
            DriverId = _driverId,
            DriverNumber = driverNumber,
            FullName = fullName,
            Licence = licence,
            IMSS = imss
        };

        return true;
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
