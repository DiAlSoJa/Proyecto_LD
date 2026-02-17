using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LD.Dialogs;
using LD.Forms.Classes;

namespace LD.Forms
{
    public partial class FrmUsuarios : Form
    {
        private Formularios formularios;
        public FrmUsuarios()
        {
            InitializeComponent();
        }
        public FrmUsuarios(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoUsuario frmNuevo = new FrmNuevoUsuario();
            frmNuevo.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmNuevoArticuloMasiva frmNuevoArticuloMasiva = new FrmNuevoArticuloMasiva();
            frmNuevoArticuloMasiva.ShowDialog();
        }
    }
}
