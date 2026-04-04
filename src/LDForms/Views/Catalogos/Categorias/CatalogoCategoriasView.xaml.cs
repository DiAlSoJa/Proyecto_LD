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
using LD.Contracts.Category;
using LD.Contracts.Constants;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Location;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Categorias;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    /// <summary>
    /// Lógica de interacción para CatalogoCategoriasView.xaml
    /// </summary>
    public partial class CatalogoCategoriasView : UserControl
    {
        private readonly CategoryService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<CategoryDto> _gridFilter;

        private CategoryDto? _selectedX;
        private bool _loaded;
        public CatalogoCategoriasView(CategoryService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<CategoryDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                 { "Id", 80 },
                 { "Categoria", 120 },
                 { "Frecuencia", 120 },
                 { "Descripcion", 250 },
                 { "Cliente", 250 },
                 { "Proyecto", 250 }                 
            });

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo categorías...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility      = UserData.HasPermission(PermissionKeys.Category_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility     = UserData.HasPermission(PermissionKeys.Category_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Category_View)   ? Visibility.Visible : Visibility.Collapsed;
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
            var result = await _service.GetCategory();

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
            await CargarDatosConLoaderAsync("Trayendo categorías...");
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
            var dialog = _serviceProvider.GetRequiredService<NuevaCategoriaView>();
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

            var dialog = _serviceProvider.GetRequiredService<NuevaCategoriaView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetCategory(_selectedX);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

      
    }
}