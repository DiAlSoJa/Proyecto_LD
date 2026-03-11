using LD.Forms.Classes;
using LD.Forms.Configuration;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    public partial class FrmMenu : Form
    {
        private readonly NavigationService _navigationService;
        private readonly TabService _tabService;
        public FrmMenu(NavigationService navigationService, TabService tabService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _tabService = tabService;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Clientes);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Proyectos);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Almacenes);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Ubicaciones);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Articulos);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Movimientos);

        }

        private void button16_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Inventario);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Aleatorio);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Usuarios);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Auditar);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.ASN);
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.CheckListMontacargas);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _tabService.Open(AppRoutes.Catalogos);
        }
    }
}
