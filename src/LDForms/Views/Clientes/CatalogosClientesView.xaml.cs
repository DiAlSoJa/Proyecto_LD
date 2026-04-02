using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.DTOs.Auth;
using LD.FormsX.Helpers;

using LD.Contracts.Constants;
using LD.FormsX.Views.Dialogs;
using LD.Formx.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class CatalogosClientesView : UserControl
    {
        private readonly ClientService _clientService;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<ClientDto> _gridFilter;
        private ClientDto? _selectedClient;
        private bool _loaded;

        public CatalogosClientesView(
            ClientService clientService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _clientService = clientService;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<ClientDto>(dgClientes, txtBuscar);
            /*
            _gridFilter.SetHiddenColumns(
                "DomicilioFiscal",
                "ColoniaFiscal",
                "CiudadFiscal",
                "CodigoPostalFiscal",
                "EmailFiscal",
                "TelefonoFiscal"
            );*/

            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo", 80 },
                { "Id", 70 },
                { "NombreComercial", 220 },
                { "DomicilioComercial", 250 },
                { "Telefono", 140 },
                { "Ciudad", 140 },
                { "CodigoPostal", 120 },
                { "RazonSocial", 250 },
                { "Rfc", 150 },
                { "IsProvider", 120 }
            });

            _gridFilter.SetColumnOrder(
                "Activo",
                "Id",
                "NombreComercial",
                "DomicilioComercial",
                "Telefono",
                "Ciudad",
                "CodigoPostal",
                "RazonSocial",
                "Rfc",
                "IsProvider"
            );
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            AplicarPermisos();
            await CargarDatosConLoaderAsync("Trayendo clientes...");
        }

        private void AplicarPermisos()
        {

            btnNuevo.Visibility    = UserData.HasPermission(PermissionKeys.Client_Create) ? Visibility.Visible : Visibility.Collapsed;
            btnEditar.Visibility   = UserData.HasPermission(PermissionKeys.Client_Update) ? Visibility.Visible : Visibility.Collapsed;
            BtnActualizar.Visibility = UserData.HasPermission(PermissionKeys.Client_View) ? Visibility.Visible : Visibility.Collapsed;
        }

     
        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
            if (!UserData.HasPermission(PermissionKeys.Client_View))
            {
                dgClientes.Visibility = Visibility.Collapsed;
                return;
            }

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
            var result = await _clientService.GetClients();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _gridFilter.SetData(result.Data);
            _selectedClient = null;
            txtStatus.Text = $"Registros: {result.Data.Count}";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Trayendo clientes...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void DgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedClient = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoClienteView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClient is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoClienteView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetClient(_selectedClient);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                await CargarDatosAsync();
            }
        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedClient == null)
                {
                    DialogHelper.ShowInfo("Selecciona un cliente.");
                    return;
                }

                bool confirmar = DialogHelper.ShowConfirm("¿Estás seguro de eliminar el cliente seleccionado?");

                if (!confirmar)
                    return;

                // Ajusta este bloque al método real de tu servicio:
                // var result = await _clientService.DeleteClient(_selectedClient.Id);

                // Ejemplo:
                // if (!result.IsSuccess)
                // {
                //     DialogHelper.ShowWarning(result.Message);
                //     return;
                // }

                DialogHelper.ShowSuccess("El cliente se eliminó correctamente.");
                await CargarDatosConLoaderAsync("Trayendo clientes...");
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }
    }
}