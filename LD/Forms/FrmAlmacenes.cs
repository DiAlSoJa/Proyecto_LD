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
    public partial class FrmAlmacenes : Form
    {
        private Formularios formularios;
        public FrmAlmacenes()
        {
            InitializeComponent();
        }
        public FrmAlmacenes(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoAlmacen frmNuevoCliente = new FrmNuevoAlmacen();
            frmNuevoCliente.ShowDialog();
        }
    }
}
