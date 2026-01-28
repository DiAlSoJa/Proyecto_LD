using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Dialogs
{
    public partial class FrmConfirm : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        private bool respuesta;
        public FrmConfirm()
        {
            InitializeComponent();
        }
        public FrmConfirm(String mensaje)
        {
            InitializeComponent();
            txtMensaje.Text = mensaje;
        }
        public void setRespuesta(bool respuesta)
        {
            this.respuesta = respuesta;
        }
        public bool getRespuesta()
        {
            return this.respuesta;
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
