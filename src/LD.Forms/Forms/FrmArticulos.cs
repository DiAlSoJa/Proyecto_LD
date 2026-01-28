using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LD.Classes;
using LD.Dialogs;

namespace LD.Forms
{
    public partial class FrmArticulos : Form
    {
        private Formularios formularios;
        public FrmArticulos()
        {
            InitializeComponent();
        }
        public FrmArticulos(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
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
    }
}
