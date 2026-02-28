using LD.Forms.Services;
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
    public partial class FrmNuevoUsuario : DraggableForm, ICreateUserView
    {


        private readonly UserService _userService;
        public FrmNuevoUsuario(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
            EnableDrag(panel2);
            EnableDrag(panel1);

        }

        public event EventHandler CreateUser;
        public event EventHandler UpdateUser;
        public event EventHandler Exit;

        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;
        public string ConfirmPassword => txtConfirmPassword.Text;
        public bool IsActive => cckIsActive.Checked;

      

       

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
