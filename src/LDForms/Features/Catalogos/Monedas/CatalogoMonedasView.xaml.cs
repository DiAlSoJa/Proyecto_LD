using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Configuration;
using LD.FormsX.Features.Catalogos.Monedas.ViewModels;
using LD.Contracts.Constants;
using LD.Contracts.Currency;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Monedas;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class CatalogoMonedasView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<CurrencyDto> _gridFilter;
        private bool _loaded;

        private CatalogoMonedasViewModel ViewModel => (CatalogoMonedasViewModel)DataContext;

        public CatalogoMonedasView(CatalogoMonedasViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<CurrencyDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                 { "CurrencyIdS", 150 },
                 { "Descripcion", 250 }
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo monedas...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility = UserData.HasPermission(PermissionKeys.Currency_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility = UserData.HasPermission(PermissionKeys.Currency_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Currency_View) ? Visibility.Visible : Visibility.Collapsed;
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
            await CargarDatosConLoaderAsync("Trayendo monedas...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ViewModel.SelectedCurrency = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaMonedaView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedCurrency is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevaMonedaView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetCurrency(ViewModel.SelectedCurrency);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo monedas...");
            }
        }
    }
}

