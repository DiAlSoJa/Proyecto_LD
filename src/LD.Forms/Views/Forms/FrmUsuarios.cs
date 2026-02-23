using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LD.Forms.Views.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services.FormServices;

namespace LD.Forms.Views.Forms
{
    public partial class FrmUsuarios : Form
    {
        private readonly DialogFormService _dialogFormService;
        public FrmUsuarios(DialogFormService dialogFormService)
        {
            InitializeComponent();
            _dialogFormService = dialogFormService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoUsuario>();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoUsuario>(config=>{

            });
            
        }
    }
}
