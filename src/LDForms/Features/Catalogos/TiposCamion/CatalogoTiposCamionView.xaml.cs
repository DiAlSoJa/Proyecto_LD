using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Configuration;
using LD.Contracts.Constants;
using LD.Contracts.TruckType;
using LD.FormsX.Features.Catalogos.TiposCamion.ViewModels;
using LD.FormsX.Helpers;
using LD.FormsX.Views.TiposCamion;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views;

public partial class CatalogoTiposCamionView : UserControl
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WpfGridFilter<TruckTypeDto> _gridFilter;
    private bool _loaded;

    private CatalogoTiposCamionViewModel ViewModel => (CatalogoTiposCamionViewModel)DataContext;

    public CatalogoTiposCamionView(CatalogoTiposCamionViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        DataContext = viewModel;
        _serviceProvider = serviceProvider;
        _gridFilter = new WpfGridFilter<TruckTypeDto>(dg, txtBuscar);
        _gridFilter.SetColumnWidths(new Dictionary<string, double>
        {
            { "TruckTypeId", 90 },
            { "Name", 220 },
            { "TieneCaja", 110 }
        });

        viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded) return;
        _loaded = true;

        AplicarPermisos();
        await CargarDatosConLoaderAsync("Trayendo tipos de camion...");
    }

    private void AplicarPermisos()
    {
        btnNuevo.Visibility = ViewModel.CanCreate ? Visibility.Visible : Visibility.Collapsed;
        btnEditar.Visibility = ViewModel.CanEdit ? Visibility.Visible : Visibility.Collapsed;
        btnEliminar.Visibility = ViewModel.CanDelete ? Visibility.Visible : Visibility.Collapsed;
        BtnActualizar.Visibility = ViewModel.CanView ? Visibility.Visible : Visibility.Collapsed;
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
        await ViewModel.CargarDatosAsync();
        txtStatus.Text = ViewModel.StatusText;
    }

    private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
    {
        await CargarDatosConLoaderAsync("Trayendo tipos de camion...");
    }

    private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
    {
        _gridFilter.ClearFilter();
    }

    private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            ViewModel.SelectedTruckType = _gridFilter.SelectedItem;
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
    {
        var dialog = _serviceProvider.GetRequiredService<NuevoTipoCamionView>();
        LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

        var result = dialog.ShowDialog();

        if (result == true)
        {
            await CargarDatosAsync();
        }
    }

    private async void BtnEditar_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedTruckType is null)
            return;

        var dialog = _serviceProvider.GetRequiredService<NuevoTipoCamionView>();
        LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        dialog.SetTruckType(ViewModel.SelectedTruckType);

        var result = dialog.ShowDialog();

        if (result == true)
        {
            await CargarDatosConLoaderAsync("Trayendo tipos de camion...");
        }
    }

    private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedTruckType is null)
        {
            DialogHelper.ShowWarning("Selecciona un tipo de camion para eliminarlo.");
            return;
        }

        if (!DialogHelper.ShowConfirm($"Eliminar el tipo de camion {ViewModel.SelectedTruckType.Name}?"))
            return;

        if (await ViewModel.DeleteSelectedAsync())
        {
            await CargarDatosConLoaderAsync("Trayendo tipos de camion...");
        }
    }
}
