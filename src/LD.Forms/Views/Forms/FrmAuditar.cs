using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LD.Forms.Views.Dialogs;
using LD.Forms.Classes;

namespace LD.Forms.Views.Forms
{
    public partial class FrmAuditar : Form
    {
        private Formularios formularios;
        public FrmAuditar()
        {
            InitializeComponent();
        }
        public FrmAuditar(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevaAuditoria frmNuevo = new FrmNuevaAuditoria();
            frmNuevo.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmNuevoArticuloMasiva frmNuevoArticuloMasiva = new FrmNuevoArticuloMasiva();
            frmNuevoArticuloMasiva.ShowDialog();
        }
    }
}
