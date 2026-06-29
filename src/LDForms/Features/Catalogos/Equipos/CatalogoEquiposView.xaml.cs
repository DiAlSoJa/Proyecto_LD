using LD.Client.Configuration;
using LD.Contracts.Constants;
using LD.Contracts.EquipmentType;
using LD.FormsX.Features.Catalogos.Equipos.ViewModels;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Equipos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views
{
    public partial class CatalogoEquiposView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<EquipmentTypeDto> _gridFilter;
        private bool _loaded;

        private CatalogoEquiposViewModel ViewModel => (CatalogoEquiposViewModel)DataContext;

        public CatalogoEquiposView(CatalogoEquiposViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<EquipmentTypeDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "EquipmentTypeId", 120 },
                { "EquipmentName", 260 },
                { "IsBattery", 140 }
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo equipos...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility = UserData.HasPermission(PermissionKeys.EquipmentType_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility = UserData.HasPermission(PermissionKeys.EquipmentType_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.EquipmentType_View) ? Visibility.Visible : Visibility.Collapsed;
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
            await CargarDatosConLoaderAsync("Trayendo equipos...");
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ViewModel.SelectedEquipmentType = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            var result = dialog.ShowDialog();
            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedEquipmentType is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetEquipmentType(ViewModel.SelectedEquipmentType);

            var result = dialog.ShowDialog();
            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo equipos...");
            }
        }
    }
}

