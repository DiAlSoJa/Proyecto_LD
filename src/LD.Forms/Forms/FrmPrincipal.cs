using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using LD.Forms.Classes;
using LD.Forms.Properties;

namespace LD.Forms
{
    public partial class FrmPrincipal : Form
    {
        private Formularios formularios = new Formularios();
        public FrmPrincipal()
        {
            InitializeComponent();

            formularios.inicia(this.pCenter, this, this.lblTitle, this.flowLayoutPest);
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
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int Msg, int wParam, int lParam);


        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
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
            //openChildForm("Menu");
            formularios.openChildForm("Menu");

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
            //var login = new FrmLogin();
            //login.Show();

            this.Hide();
        }
    }
}
