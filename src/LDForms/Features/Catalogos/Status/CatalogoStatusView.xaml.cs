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
using LD.Contracts.InventaryStatus;
using LD.Contracts.Location;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Almacen;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Status;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    /// <summary>
    /// Lógica de interacción para CatalogoStatusView.xaml
    /// </summary>
    public partial class CatalogoStatusView : UserControl
    {
        private readonly InventaryStatusService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<InventaryStatusDto> _gridFilter;

        private InventaryStatusDto? _selectedX;
        private bool _loaded;
        public CatalogoStatusView(InventaryStatusService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<InventaryStatusDto>(dg, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { nameof(InventaryStatusDto.StatusId), 110 },
                { nameof(InventaryStatusDto.Descripcion), 220 },
                { nameof(InventaryStatusDto.Cliente), 180 },
                { nameof(InventaryStatusDto.Proyecto), 180 }
            });
            _gridFilter.SetHiddenColumns(
                nameof(InventaryStatusDto.ClientId),
                nameof(InventaryStatusDto.ProjectId));

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo estatus...");
        }

        private void AplicarPermisos()
        {
            btnNuevo.Visibility      = UserData.HasPermission(PermissionKeys.Status_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility     = UserData.HasPermission(PermissionKeys.Status_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Status_View)   ? Visibility.Visible : Visibility.Collapsed;
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
            var result = await _service.GetInventaryStatus();

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
            await CargarDatosConLoaderAsync("Trayendo estatus...");
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
            var dialog = _serviceProvider.GetRequiredService<NuevoStatusView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

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

            var dialog = _serviceProvider.GetRequiredService<NuevoStatusView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetInventaryStatus(_selectedX);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosConLoaderAsync("Trayendo estatus...");
            }
        }

       
    }
}

