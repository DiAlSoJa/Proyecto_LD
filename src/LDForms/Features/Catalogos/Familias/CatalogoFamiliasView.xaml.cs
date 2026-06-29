using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Configuration;
using LD.FormsX.Features.Catalogos.Familias.ViewModels;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.Family;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Familias;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class CatalogoFamiliasView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<FamilyDto> _gridFilter;
        private bool _loaded;

        private CatalogoFamiliasViewModel ViewModel => (CatalogoFamiliasViewModel)DataContext;

        public CatalogoFamiliasView(CatalogoFamiliasViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<FamilyDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                 { "Id", 80 },
                 { "NombreFamilia", 250 },
                 { "Cliente", 250 },
                 { "Proyecto", 250 }
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo familias...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility = UserData.HasPermission(PermissionKeys.Family_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility = UserData.HasPermission(PermissionKeys.Family_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Family_View) ? Visibility.Visible : Visibility.Collapsed;
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
            await CargarDatosConLoaderAsync("Trayendo familias...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ViewModel.SelectedFamily = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaFamiliaView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedFamily is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevaFamiliaView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetFamily(ViewModel.SelectedFamily);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo familias...");
            }
        }
    }
}

