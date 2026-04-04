using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Currency;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Location;
using LD.Contracts.Units;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Monedas;
using LD.FormsX.Views.Status;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    /// <summary>
    /// Lógica de interacción para CatalogoMonedasView.xaml
    /// </summary>
    public partial class CatalogoMonedasView : UserControl
    {
        private readonly CurrencyService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<CurrencyDto> _gridFilter;

        private CurrencyDto? _selectedX;
        private bool _loaded;
        public CatalogoMonedasView(CurrencyService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<CurrencyDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                 { "CurrencyIdS", 150 },
                 { "Descripcion", 250 }
                 
            });

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
            btnNuevo.Visibility      = UserData.HasPermission(PermissionKeys.Currency_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility     = UserData.HasPermission(PermissionKeys.Currency_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Currency_View)   ? Visibility.Visible : Visibility.Collapsed;
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
            var result = await _service.GetCurrency();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _gridFilter.SetData(result.Data);
            _selectedX = null;
            txtStatus.Text = $"Registros: {result.Data.Count}";
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
                _selectedX = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaMonedaView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedX is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevaMonedaView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetCurrency(_selectedX);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo monedas...");
            }
        }
    }
}