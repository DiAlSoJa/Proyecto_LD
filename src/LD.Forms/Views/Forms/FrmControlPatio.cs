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

public partial class FrmControlPatio : Form
{
    private readonly DialogFormService _dialogFormService;
    public FrmControlPatio(DialogFormService dialogFormService)
    {
        InitializeComponent();
        _dialogFormService = dialogFormService;
    }
    private void actualizarTemp()
    {
        // Simulación de actualización de datos        
        dataGridView1.Rows.Clear();
        dataGridView2.Rows.Clear();
        dataGridView3.Rows.Clear();
        dataGridView4.Rows.Clear();
        dataGridView1.Rows.Add(1, DateTime.Now, "Carga", "", "Monterrey", "Camioneta", "1546", "Asignar cortina 525");
        dataGridView2.Rows.Add(562, DateTime.Now, "", "", "Mover caja 285 a patio");
        dataGridView2.Rows.Add(563, DateTime.Now, "", "Tracto Caja", "");

        dataGridView3.Rows.Add(1, DateTime.Now, "Carga", "", "Monterrey", "Camioneta", "1546", "En cortina", "525", "", "", "✏", "❌");
        dataGridView3.Rows.Add(1, DateTime.Now, "Descarga", "Tracto Caja", "Laredo", "Caja Seca", "ND525222", "", "", "", "", "✏", "❌");

        dataGridView4.Rows.Add(563, DateTime.Now, "Jose Francisco", "489", "", "✏", "❌");
        dataGridView4.Rows.Add(124, DateTime.Now, "Pedro", "2547", "", "✏", "❌");

    }

    private void reloadBtn_Click(object sender, EventArgs e)
    {
        actualizarTemp();
    }

    private void FrmControlPatio_Load(object sender, EventArgs e)
    {
        actualizarTemp();
    }
}
