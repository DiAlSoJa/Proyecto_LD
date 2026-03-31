using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Requests.Client;
using LD.FormsX.Helpers;
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
using System.Windows.Shapes;

namespace LD.FormsX.Views.Usuarios
{
    /// <summary>
    /// Lógica de interacción para NuevoUsuarioView.xaml
    /// </summary>
    public partial class NuevoUsuarioView : Window
    {
        private readonly ClientService _clientService;
        private ClientDto? ClientSelected { get; set; }

        public bool ResponseForm { get; private set; }

        public NuevoUsuarioView()
        {
            InitializeComponent();
        }
        public async void SetClient(ClientDto client)
        {
            ClientSelected = client;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _clientService.GetClientById(ClientSelected?.Id ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message);
                    return;
                }

                var client = response.Data;
                txtId.Text = client.ClientId.ToString();
                txtComercialName.Text = client.CommercialName;
                txtCiudadComercial.Text = client.City;
                txtCPComercial.Text = client.ZipCode;
                txtTelefonoComercial.Text = client.Phone;
                txtColoniaComercial.Text = client.Neightbourhoud;
                checkIsActive.IsChecked = client.IsActive;
                checkIsProvider.IsChecked = client.IsProvider;
                txtDomicilioComercial.Text = client.CommercialAddress;

                txtRazonSocial.Text = client.FiscalData?.BusinessName ?? string.Empty;
                txtRFC.Text = client.FiscalData?.Rfc ?? string.Empty;
                txtDomicilioFiscal.Text = client.FiscalData?.FiscalAddress ?? string.Empty;
                txtColonia.Text = client.FiscalData?.Neightbourhoud ?? string.Empty;
                txtCiudadFiscal.Text = client.FiscalData?.City ?? string.Empty;
                txtCPFiscal.Text = client.FiscalData?.ZipCode ?? string.Empty;
                txtEmail.Text = client.FiscalData?.Email ?? string.Empty;
                txtTelefonoFiscal.Text = client.FiscalData?.Phone ?? string.Empty;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private ClientRequest BuildRequest()
        {
            return new ClientRequest
            {
                ClientId = ClientSelected != null ? ClientSelected.Id : 0,
                CommercialName = txtComercialName.Text.Trim(),
                CommercialAddress = txtDomicilioComercial.Text.Trim(),
                Neightbourhoud = txtColoniaComercial.Text.Trim(),
                City = txtCiudadComercial.Text.Trim(),
                ZipCode = txtCPComercial.Text.Trim(),
                Phone = txtTelefonoComercial.Text.Trim(),
                IsActive = checkIsActive.IsChecked == true,
                IsProvider = checkIsProvider.IsChecked == true,
                FiscalData = HasFiscalData()
                    ? new ClientFiscalDataRequest
                    {
                        BusinessName = txtRazonSocial.Text.Trim(),
                        Rfc = txtRFC.Text.Trim(),
                        FiscalAddress = txtDomicilioFiscal.Text.Trim(),
                        Neightbourhoud = txtColonia.Text.Trim(),
                        City = txtCiudadFiscal.Text.Trim(),
                        ZipCode = txtCPFiscal.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Phone = txtTelefonoFiscal.Text.Trim()
                    }
                    : null
            };
        }

        private bool HasFiscalData()
        {
            return !string.IsNullOrWhiteSpace(txtRazonSocial.Text) ||
                   !string.IsNullOrWhiteSpace(txtRFC.Text) ||
                   !string.IsNullOrWhiteSpace(txtDomicilioFiscal.Text) ||
                   !string.IsNullOrWhiteSpace(txtColonia.Text) ||
                   !string.IsNullOrWhiteSpace(txtCiudadFiscal.Text) ||
                   !string.IsNullOrWhiteSpace(txtCPFiscal.Text) ||
                   !string.IsNullOrWhiteSpace(txtEmail.Text) ||
                   !string.IsNullOrWhiteSpace(txtTelefonoFiscal.Text);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();

                var result = ClientSelected != null
                    ? await _clientService.UpdateClient(ClientSelected.Id, request)
                    : await _clientService.CreateClient(request);

                if (result.IsSuccess)
                {
                    ResponseForm = true;
                    DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente");
                    DialogResult = true;
                    Close();
                }
                else
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? "Ocurrió un error");
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

    }
}
