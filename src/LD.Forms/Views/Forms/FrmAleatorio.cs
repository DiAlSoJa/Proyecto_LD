using LD.Forms.Classes;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace LD.Forms.Views.Forms;

public partial class FrmAleatorio : Form
{
    private readonly DialogFormService _dialogFormService;
    public FrmAleatorio(DialogFormService dialogFormService)
    {
        InitializeComponent();
        _dialogFormService = dialogFormService;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        _dialogFormService.ShowDialog<FrmNuevoCliente>();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        _dialogFormService.ShowDialog<FrmNuevoArticuloMasiva>();
    }
    
}
