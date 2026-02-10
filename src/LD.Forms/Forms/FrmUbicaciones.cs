using LD.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms
{
    public partial class FrmUbicaciones : Form
    {
        private Formularios formularios;
        private readonly LocationService _locationService;
        public FrmUbicaciones()
        {
            InitializeComponent();
        }
        public FrmUbicaciones(Formularios f)
        {
            InitializeComponent();
            _locationService = new();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevaUbicacion frmNuevoCliente = new FrmNuevaUbicacion();
            frmNuevoCliente.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmNuevaUbicacionMasiva frmNuevaUbicacionMasiva = new FrmNuevaUbicacionMasiva();
            frmNuevaUbicacionMasiva.ShowDialog();
        }
    }
}
