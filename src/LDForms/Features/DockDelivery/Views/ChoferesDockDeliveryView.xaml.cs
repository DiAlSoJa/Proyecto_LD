using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Driver;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LDForms.Features.DockDelivery.Views;

public partial class ChoferesDockDeliveryView : Window
{
    private readonly DriverService _driverService;
    private readonly IServiceProvider _serviceProvider;
    private readonly WpfGridFilter<DriverDto> _gridFilter;
    private bool _loaded;
    private DriverDto? _selectedDriver;

    public ChoferesDockDeliveryView(DriverService driverService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _driverService = driverService;
        _serviceProvider = serviceProvider;

        _gridFilter = new WpfGridFilter<DriverDto>(dgChoferes, ldBuscar.TextBoxElement);
        _gridFilter.SetColumnOrder("DriverId", "DriverNumber", "FullName", "Licence", "IMSS");
        _gridFilter.SetColumnWidths(new Dictionary<string, double>
        {
            { "DriverId", 80 },
            { "DriverNumber", 120 },
            { "FullName", 260 },
            { "Licence", 160 },
            { "IMSS", 140 }
        });
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
            return;

        _loaded = true;

        if (!UserData.HasPermission(PermissionKeys.Vehicle_View))
        {
            DialogHelper.ShowWarning("No tienes permiso para consultar choferes.");
            Close();
            return;
        }

        AplicarPermisos();
        await CargarDatosConLoaderAsync("Trayendo choferes...");
    }

    private void AplicarPermisos()
    {
        btnNuevo.Visibility = UserData.HasPermission(PermissionKeys.Vehicle_Create) ? Visibility.Visible : Visibility.Collapsed;
        btnEditar.Visibility = UserData.HasPermission(PermissionKeys.Vehicle_Update) ? Visibility.Visible : Visibility.Collapsed;
        btnEliminar.Visibility = UserData.HasPermission(PermissionKeys.Vehicle_Delete) ? Visibility.Visible : Visibility.Collapsed;
        btnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Vehicle_View) ? Visibility.Visible : Visibility.Collapsed;
    }

    private async Task CargarDatosConLoaderAsync(string mensaje)
    {
        try
        {
            MostrarLoader(true, mensaje);
            await CargarDatosAsync();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            MostrarLoader(false);
        }
    }

    private void MostrarLoader(bool mostrar, string mensaje = "Cargando...")
    {
        TxtLoading.Text = mensaje;
        LoadingOverlay.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
    }

    private async Task CargarDatosAsync()
    {
        var result = await _driverService.GetDrivers();
        if (!result.IsSuccess)
        {
            DialogHelper.ShowWarning(result.ErrorMessage ?? result.Message ?? "No se pudieron obtener los choferes.");
            return;
        }

        var data = result.Data ?? [];
        _gridFilter.SetData(data);
        _selectedDriver = null;
        txtStatus.Text = $"Registros: {data.Count}";
    }

    private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
    {
        await CargarDatosConLoaderAsync("Trayendo choferes...");
    }

    private void DgChoferes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedDriver = _gridFilter.SelectedItem;
    }

    private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<NuevoChoferDockDeliveryView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

        if (dialog.ShowDialog() == true)
        {
            await CargarDatosConLoaderAsync("Trayendo choferes...");
        }
    }

    private async void BtnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedDriver is null)
        {
            DialogHelper.ShowWarning("Selecciona un chofer para editarlo.");
            return;
        }

        var dialog = _serviceProvider.GetRequiredService<NuevoChoferDockDeliveryView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetDriver(_selectedDriver);

        if (dialog.ShowDialog() == true)
        {
            await CargarDatosConLoaderAsync("Trayendo choferes...");
        }
    }

    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedDriver is null)
        {
            DialogHelper.ShowWarning("Selecciona un chofer para eliminarlo.");
            return;
        }

        if (!DialogHelper.ShowConfirm($"¿Deseas eliminar el chofer {_selectedDriver.FullName}?"))
            return;

        try
        {
            MostrarLoader(true, "Eliminando chofer...");
            var response = await _driverService.DeleteDriver(_selectedDriver.DriverId);

            if (response.IsSuccess)
            {
                DialogHelper.ShowSuccess(response.Message ?? "Chofer eliminado correctamente.");
                await CargarDatosAsync();
            }
            else
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar el chofer.");
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            MostrarLoader(false);
        }
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
