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
    public partial class FrmProyectos : Form
    {
        private Formularios formularios;
        public FrmProyectos()
        {
            InitializeComponent();
        }
        public FrmProyectos(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoProyecto frmNuevoCliente = new FrmNuevoProyecto();
            frmNuevoCliente.ShowDialog();
        }
    }
}
