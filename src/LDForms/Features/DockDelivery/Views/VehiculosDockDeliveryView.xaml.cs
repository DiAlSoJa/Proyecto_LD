using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Requests;
using LD.Contracts.Vehicle;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LDForms.Features.DockDelivery.Views;

public partial class VehiculosDockDeliveryView : Window
{
    private readonly VehicleService _vehicleService;
    private readonly IServiceProvider _serviceProvider;
    private readonly WpfGridFilter<VehicleDto> _gridFilter;
    private bool _loaded;
    private VehicleDto? _selectedVehicle;

    public VehiculosDockDeliveryView(VehicleService vehicleService, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _vehicleService = vehicleService;
        _serviceProvider = serviceProvider;

        _gridFilter = new WpfGridFilter<VehicleDto>(dgVehiculos, ldBuscar.TextBoxElement);
        _gridFilter.SetHiddenColumns("CreatedAt");
        _gridFilter.SetColumnOrder("Placas", "NumeroVehiculo", "Nombre", "Tipo", "Capacidad", "Largo", "Ancho", "Alto", "CreatedAt");
        _gridFilter.SetColumnWidths(new Dictionary<string, double>
        {
            { "Placas", 120 },
            { "NumeroVehiculo", 140 },
            { "Nombre", 220 },
            { "Tipo", 160 },
            { "Capacidad", 90 },
            { "Largo", 80 },
            { "Ancho", 80 },
            { "Alto", 80 },
            { "CreatedAt", 150 }
        });
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
            return;

        _loaded = true;

        if (!UserData.HasPermission(PermissionKeys.Vehicle_View))
        {
            DialogHelper.ShowWarning("No tienes permiso para consultar vehículos.");
            Close();
            return;
        }

        AplicarPermisos();
        await CargarDatosConLoaderAsync("Trayendo vehículos...");
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
        var result = await _vehicleService.GetVehicles();
        if (!result.IsSuccess)
        {
            DialogHelper.ShowWarning(result.ErrorMessage ?? result.Message ?? "No se pudieron obtener los vehículos.");
            return;
        }

        var data = result.Data ?? [];
        _gridFilter.SetData(data);
        _selectedVehicle = null;
        txtStatus.Text = $"Registros: {data.Count}";
    }

    private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
    {
        await CargarDatosConLoaderAsync("Trayendo vehículos...");
    }

    private void DgVehiculos_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedVehicle = _gridFilter.SelectedItem;
    }

    private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<NuevoVehiculoDockDeliveryView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

        if (dialog.ShowDialog() == true)
        {
            await CargarDatosConLoaderAsync("Trayendo vehículos...");
        }
    }

    private async void BtnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedVehicle is null)
        {
            DialogHelper.ShowWarning("Selecciona un vehículo para editarlo.");
            return;
        }

        var dialog = _serviceProvider.GetRequiredService<NuevoVehiculoDockDeliveryView>();
        WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetVehicle(_selectedVehicle);

        if (dialog.ShowDialog() == true)
        {
            await CargarDatosConLoaderAsync("Trayendo vehículos...");
        }
    }

    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedVehicle is null)
        {
            DialogHelper.ShowWarning("Selecciona un vehículo para eliminarlo.");
            return;
        }

        if (!DialogHelper.ShowConfirm($"¿Deseas eliminar el vehículo {_selectedVehicle.Placas}?"))
            return;

        try
        {
            MostrarLoader(true, "Eliminando vehículo...");
            var response = await _vehicleService.DeleteVehicle(_selectedVehicle.Placas);

            if (response.IsSuccess)
            {
                DialogHelper.ShowSuccess(response.Message ?? "Vehículo eliminado correctamente.");
                await CargarDatosAsync();
            }
            else
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo eliminar el vehículo.");
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
