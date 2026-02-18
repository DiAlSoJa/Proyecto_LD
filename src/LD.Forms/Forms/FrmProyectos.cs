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
    public partial class FrmProyectos : Form
    {
        private Formularios formularios;
        private readonly ProjectService _projectService;

        public FrmProyectos(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
            _projectService=new();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoProyecto frmNuevoCliente = new FrmNuevoProyecto();
            frmNuevoCliente.ShowDialog();
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
                var clientResponse = await _projectService.GetProjects();



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
    }
}
