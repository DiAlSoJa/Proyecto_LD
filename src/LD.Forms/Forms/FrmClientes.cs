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
    public partial class FrmClientes : Form
    {
        private Formularios formularios;
        public FrmClientes()
        {
            InitializeComponent();
        }
        public FrmClientes(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoCliente frmNuevoCliente = new FrmNuevoCliente();
            frmNuevoCliente.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}
