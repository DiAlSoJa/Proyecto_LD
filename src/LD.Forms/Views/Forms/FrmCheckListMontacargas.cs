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

public partial class FrmCheckListMontacargas : Form
{
    private readonly DialogFormService _dialogFormService;
    public FrmCheckListMontacargas(DialogFormService dialogFormService)
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

    private void button1_Click_1(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmNuevoMontacargas>();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmNuevoMontacargas>();
    }

    private void button4_Click_1(object sender, EventArgs e)
    {
        var form = _dialogFormService.ShowDialog<FrmChooseUser>();
    }
}
