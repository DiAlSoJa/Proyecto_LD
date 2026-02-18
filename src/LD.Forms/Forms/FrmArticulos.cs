using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LD.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services;

namespace LD.Forms
{
    public partial class FrmArticulos : Form
    {
        private Formularios formularios;
        private readonly ItemService _itemService;

        public FrmArticulos(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
            _itemService = new();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoArticulo frmNuevoCliente = new FrmNuevoArticulo();
            frmNuevoCliente.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmNuevoArticuloMasiva frmNuevoArticuloMasiva = new FrmNuevoArticuloMasiva();
            frmNuevoArticuloMasiva.ShowDialog();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var clientResponse = await _itemService.GetItems();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");
        }

        private void gridContainer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
