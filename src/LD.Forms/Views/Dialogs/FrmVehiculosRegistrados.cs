using LD.Client.Services;
using LD.Contracts.Product;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmVehiculosRegistrados : DraggableForm
    {
        private bool mouseDown;
        private Point lastLocation;

        private readonly ProductService _itemService;
        private BindingSource _itemsBinding = new();
        private ProductDto? selectedItem { get; set; }
        private readonly DialogFormService _dialogFormService;




        public FrmVehiculosRegistrados(ProductService itemService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _itemService = itemService;
            _dialogFormService = dialogFormService;
        }

        private void cargaInfoPruebas()
        {
            dataGridView1.Rows.Add("26-02-2026","19:00:00","ABC123","Juan Manuel Pavol");
            dataGridView1.Rows.Add("26-02-2026","19:08:00","SDC123","Maria Hernandez");
            dataGridView1.Rows.Add("26-02-2026","19:09:00","FFSD55","Jose Sanchez");
            dataGridView1.Rows.Add("26-02-2026","20:35:00","PROF665","Ignacio Gonzalez");
            dataGridView1.Rows.Add("26-02-2026","23:40:00","VKKC89","Francisco Garciar");
            
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

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmVehiculosRegistrados_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            cargaInfoPruebas();
        }
    }
}
