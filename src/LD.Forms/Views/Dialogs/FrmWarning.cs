using LD.Forms.Views.Common;
using LD.Forms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmWarning : DraggableForm, IBaseMessageDialog
    {
        public FrmWarning()
        {
            InitializeComponent();
            EnableDrag(panel1);
            EnableDrag(panel2);

        }

        public void SetMessage(string message)
        {
            txtMensaje.Text = message;
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
