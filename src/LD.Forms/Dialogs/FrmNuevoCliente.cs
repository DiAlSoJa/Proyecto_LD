using LD.Contracts.Requests;
using LD.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Dialogs
{
    public partial class FrmNuevoCliente : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        private readonly ClientService _clientService;
        public FrmNuevoCliente()
        {
            InitializeComponent();
            _clientService = new ClientService();
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


        private async  void btnSave_Click(object sender, EventArgs e)
        {
            var request = new ClientRequest
            {
                CommercialName = txtComercialName.Text,
                City = txtCiudad.Text,
                PostalCode = txtCodigoPostal.Text,
                BusinessName = txtRazonSocial.Text,
                Rfc= txtRFC.Text,
                Phone = txtTelefono.Text,
                IsActive = checkIsActive.Checked,
                CommercialAddress = txtDomicilioComercial.Text
            };

           var result =  await _clientService.CreateClient(request);

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Data, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
