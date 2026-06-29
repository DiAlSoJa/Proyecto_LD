using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Configuration;
using LD.FormsX.Features.Catalogos.Dimensionador.ViewModels;
using LD.Contracts.Constants;
using LD.Contracts.Dimensioner;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Dimensionador;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class CatalogoDimensionadorView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<DimensionerDto> _gridFilter;
        private bool _loaded;

        private CatalogoDimensionadorViewModel ViewModel => (CatalogoDimensionadorViewModel)DataContext;

        public CatalogoDimensionadorView(CatalogoDimensionadorViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<DimensionerDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                 { "DimensionerId", 100 },
                 { "Description", 200 },
                 { "Height", 120 },
                 { "Width", 120  },
                 { "Length",  120  },
                 { "Weight",  120  }
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo dimensiones...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility = UserData.HasPermission(PermissionKeys.Dimensioner_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility = UserData.HasPermission(PermissionKeys.Dimensioner_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Dimensioner_View) ? Visibility.Visible : Visibility.Collapsed;
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
            await CargarDatosConLoaderAsync("Trayendo dimensiones...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ViewModel.SelectedDimensioner = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoDimensionadorView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedDimensioner is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoDimensionadorView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetDimensioner(ViewModel.SelectedDimensioner);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo dimensiones...");
            }
        }
    }
}

