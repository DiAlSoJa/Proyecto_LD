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
    public partial class FrmSuccess : DraggableForm, IBaseMessageDialog
    {
        public FrmSuccess()
        {
            InitializeComponent();
            EnableDrag(panel1);
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
