using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LD.Client.Services;
using LD.Contracts.Currency;
using LD.Contracts.Dimensioner;
using LD.Contracts.DTOs.Family;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Location;
using LD.Contracts.Units;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Dimensionador;
using LD.FormsX.Views.Familias;
using LD.FormsX.Views.Unidades;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    /// <summary>
    /// Lógica de interacción para CatalogoFamiliasView.xaml
    /// </summary>
    public partial class CatalogoDimensionadorView : UserControl
    {
        private readonly DimensionerService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<DimensionerDto> _gridFilter;
        
        private DimensionerDto? _selectedX;
        private bool _loaded;
        public CatalogoDimensionadorView(DimensionerService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
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



        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await CargarDatosConLoaderAsync("Trayendo dimensiones...");
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
            var result = await _service.GetDimensioners();

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
                _selectedX = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoDimensionadorView>();
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

            var dialog = _serviceProvider.GetRequiredService<NuevoDimensionadorView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetDimensioner(_selectedX);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo dimensiones...");
            }
        }
    }
}