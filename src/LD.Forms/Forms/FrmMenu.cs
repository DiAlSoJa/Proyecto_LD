using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LD.Forms.Classes;

namespace LD.Forms
{
    public partial class FrmMenu : Form
    {
        private Formularios formularios;
        public FrmMenu()
        {
            InitializeComponent();
        }
        public FrmMenu(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Clientes");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Proyectos");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Almacenes");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Ubicaciones");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Articulos");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Movimientos");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Inventario");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Aleatorio");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Usuarios");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.formularios.openChildForm("Auditar");
        }
    }
}
