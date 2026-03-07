using LD.Client.Services;
using LD.Forms.Classes;
using LD.Forms.Configuration;
using LD.Forms.Properties;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    public partial class FrmPrincipal : DraggableForm
    {
        private readonly NavigationService _navigation;
        private readonly ApiService _apiService;

        private readonly TabService _tabService;
        public event EventHandler? LogoutRequested;
        public FrmPrincipal(NavigationService navigation, TabService tabService,ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
            _navigation = navigation;
            _tabService = tabService;
            _tabService.Initialize(flowLayoutPest, lblTitle);
            _navigation.Initialize(pCenter);

            //formularios.inicia(this.pCenter, this, this.lblTitle, this.flowLayoutPest);
            this.Text = String.Empty;
            this.ControlBox = false;
            iniciarMenu();
            Rectangle sc = Screen.FromHandle(this.Handle).WorkingArea;
            sc.Height = sc.Height + 20;
            sc.Width = sc.Width + 20;
            sc.X = -10;
            sc.Y = -10;
            this.MaximizedBounds = sc;

            lblUser.Text = UserData.UserName;
            EnableDrag(panelTop);
        }

 
        private void actualizaIcono()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.btnFrmMaximizar.Image = Resources.max1;
                Rectangle sc = Screen.FromHandle(this.Handle).WorkingArea;
                sc.Height = sc.Height + 20;
                sc.Width = sc.Width + 20;
                sc.X = -10;
                sc.Y = -10;
                this.MaximizedBounds = sc;
            }
            else
            {
                this.btnFrmMaximizar.Image = Resources.max2;
            }
            // pMenu.Size = new System.Drawing.Size(286, this.Height - 100);
        }


        public void iniciarMenu()
        {
            _tabService.Open(AppRoutes.Menu);
         

        }

        private void btnFrmMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
            actualizaIcono();
        }

        private void btnFrmClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFrmMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FrmPrincipal_Resize(object sender, EventArgs e)
        {
            actualizaIcono();
        }

        private void panelTop_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
            actualizaIcono();
        }

        private void lblUser_Click(object sender, EventArgs e)
        {


            menuUser.Show(panelUser, new Point(0, panelUser.Height + 5));
        }

        private void label1_Click(object sender, EventArgs e)
        {
            menuUser.Show(panelUser, new Point(0, panelUser.Height + 5));
        }

        private void lblUser_MouseEnter(object sender, EventArgs e)
        {
            panelUser.BackColor = Color.FromArgb(34, 74, 154);
        }

        private void lblUser_MouseLeave(object sender, EventArgs e)
        {
            panelUser.BackColor = Color.FromArgb(20, 41, 84);
        }

        private void btnFrmClose_MouseEnter(object sender, EventArgs e)
        {
            btnFrmClose.BackColor = System.Drawing.Color.LightCoral;
        }

        private void btnFrmClose_MouseLeave(object sender, EventArgs e)
        {
            btnFrmClose.BackColor = Color.FromArgb(20, 41, 84);
        }

        private void btnFrmMinimizar_MouseHover(object sender, EventArgs e)
        {
            btnFrmMinimizar.BackColor = Color.FromArgb(34, 74, 154);
        }

        private void btnFrmMinimizar_MouseLeave(object sender, EventArgs e)
        {
            btnFrmMinimizar.BackColor = Color.FromArgb(20, 41, 84);
        }

        private void btnFrmMaximizar_MouseHover(object sender, EventArgs e)
        {
            btnFrmMaximizar.BackColor = Color.FromArgb(34, 74, 154);
        }

        private void btnFrmMaximizar_MouseLeave(object sender, EventArgs e)
        {
            btnFrmMaximizar.BackColor = Color.FromArgb(20, 41, 84);
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserSession.LogOut();
            _apiService.ClearToken();
            LogoutRequested?.Invoke(this, EventArgs.Empty);
           
        }
    }
}
