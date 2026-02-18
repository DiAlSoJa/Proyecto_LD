using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoCliente : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        private readonly ClientService _clientService;
        private  ClientDto? ClientSelected { get; set; }
        private bool IsEditing{ get; set; }

        public FrmNuevoCliente()
        {
            InitializeComponent();
            _clientService = new ClientService();
        }

        public FrmNuevoCliente(ClientDto client)
        {
            InitializeComponent();
            _clientService = new ClientService();
            ClientSelected = client;
            IsEditing = true;
            btnSave.Text = "Actualizar";
        }


        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if(IsEditing)
                await CargarDatosAsync();

            //await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _clientService.GetClientById(ClientSelected?.Id??0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var client = response.Data;
                txtId.Text = client.Id.ToString();
                txtComercialName.Text = client.NombreComercial;
                txtCiudadComercial.Text = client.Ciudad;
                txtCPComercial.Text=client.CodigoPostal;
                txtRazonSocial.Text=client.RazonSocial;
                txtRFC.Text = client.Rfc;
                txtTelefonoComercial.Text = client.Telefono;
                checkIsActive.Checked = client.Activo;
                txtDomicilioComercial.Text = client.DomicilioComercial;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
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
            return IsEditing
                ? await EditClient(ClientSelected?.Id ?? 0, request)
                : await CreateClient(request);
        }
        private ClientRequest BuildRequest()
        {
            return new ClientRequest
            {
                CommercialName = txtComercialName.Text.Trim(),
                City = txtCiudadComercial.Text.Trim(),
                PostalCode = txtCPComercial.Text.Trim(),
                BusinessName = txtRazonSocial.Text.Trim(),
                Rfc = txtRFC.Text.Trim(),
                Phone = txtTelefonoComercial.Text.Trim(),
                IsActive = checkIsActive.Checked,
                CommercialAddress = txtDomicilioComercial.Text.Trim()
            };
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
