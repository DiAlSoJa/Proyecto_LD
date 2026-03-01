using LD.Contracts.Client;
using LD.Contracts.Requests.Client;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Services;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoCliente : DraggableForm
    {
        private readonly ClientService _clientService;
        private  ClientDto? ClientSelected { get; set; }
      

        public FrmNuevoCliente(ClientService clientService)
        {
            InitializeComponent();
            _clientService = clientService;
            EnableDrag(panel2);
            EnableDrag(panel1);

        }

        public async void SetClient(ClientDto client)
        {
            ClientSelected = client;
            await CargarDatosAsync();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _clientService.GetClientById(ClientSelected?.Id ?? 0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var client = response.Data;
                txtId.Text = client.ClientId.ToString();
                txtComercialName.Text = client.CommercialName;
                txtCiudadComercial.Text = client.City;
                txtCPComercial.Text = client.ZipCode;
                txtTelefonoComercial.Text = client.Phone;
                txtColoniaComercial.Text = client.Neightbourhoud;
                checkIsActive.Checked = client.IsActive;
                checkIsProvider.Checked = client.IsActive;
                txtDomicilioComercial.Text = client.CommercialAddress;



                txtRazonSocial.Text= client.FiscalData?.BusinessName ?? string.Empty;
                txtRFC.Text = client.FiscalData?.Rfc ?? string.Empty;
                txtDomicilioFiscal.Text = client.FiscalData?.FiscalAddress ?? string.Empty;
                txtColonia.Text = client.FiscalData?.Neightbourhoud ?? string.Empty;
                txtCiudadFiscal.Text = client.FiscalData?.City ?? string.Empty;
                txtCPFiscal.Text = client.FiscalData?.ZipCode ?? string.Empty;
                txtEmail.Text = client.FiscalData?.Email ?? string.Empty; ;
                txtTelefonoFiscal.Text = client.FiscalData?.Phone ?? string.Empty;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Task<ApiResponseDto<string>> CreateClient(ClientRequest request) =>
            _clientService.CreateClient(request);

        private Task<ApiResponseDto<string>> EditClient(int clientId, ClientRequest request) =>
            _clientService.UpdateClient(clientId, request);
        private async Task<ApiResponseDto<string>> SaveClient(ClientRequest request)
        {
            return ClientSelected!=null
                ? await EditClient(ClientSelected?.Id ?? 0, request)
                : await CreateClient(request);
        }

        private ClientRequest BuildRequest()
        {

            return new ClientRequest
            {
                ClientId = ClientSelected!=null? ClientSelected.Id:0,
                CommercialName = txtComercialName.Text.Trim(),
                CommercialAddress = txtDomicilioComercial.Text.Trim(),
                Neightbourhoud = txtColoniaComercial.Text.Trim(),
                City = txtCiudadComercial.Text.Trim(),
                ZipCode = txtCPComercial.Text.Trim(),
                Phone = txtTelefonoComercial.Text.Trim(),
                IsActive = checkIsActive.Checked,
                IsProvider = checkIsProvider.Checked,
                FiscalData = HasFiscalData()? new ClientFiscalDataRequest
                {
                    BusinessName = txtRazonSocial.Text.Trim(),
                    Rfc = txtRFC.Text.Trim(),
                    FiscalAddress = txtDomicilioFiscal.Text.Trim(),
                    Neightbourhoud = txtColonia.Text.Trim(),
                    City = txtCiudadFiscal.Text.Trim(),
                    ZipCode = txtCPFiscal.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtTelefonoFiscal.Text.Trim()
                } :null
            };
        }
        public bool HasFiscalData()
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
        private void ShowResult(ApiResponseDto<string> result)
        {
            MessageBox.Show(
                result.IsSuccess ? result.Data : result.Message,
                result.IsSuccess ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveClient(request);

                ShowResult(result);

                ResponseForm = result.IsSuccess;
                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }
    }
}
