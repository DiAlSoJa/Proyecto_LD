using LD.Forms.Classes;
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
    private Formularios formularios;
    public FrmAleatorio()
    {
        InitializeComponent();
    }
    public FrmAleatorio(Formularios f)
    {
        InitializeComponent();
        this.formularios = f;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        FrmNuevoArticulo frmNuevoCliente = new FrmNuevoArticulo();
        frmNuevoCliente.ShowDialog();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        FrmNuevoArticuloMasiva frmNuevoArticuloMasiva = new FrmNuevoArticuloMasiva();
        frmNuevoArticuloMasiva.ShowDialog();
    }
    
}
