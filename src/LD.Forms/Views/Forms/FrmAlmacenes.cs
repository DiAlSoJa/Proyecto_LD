using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    public partial class FrmAlmacenes : Form
    {
        private readonly WarehouseService _warehouseService;
        private Formularios formularios;

        public FrmAlmacenes(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
            _warehouseService = new();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoAlmacen frmNuevoCliente = new FrmNuevoAlmacen();
            frmNuevoCliente.ShowDialog();
        }
    }
}
