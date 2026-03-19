
using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Product;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    public partial class FrmASN : Form
    {
        private readonly ProductService _itemService;
        private BindingSource _itemsBinding = new();
        private ProductDto? selectedItem { get; set; }
        private GridFilter<ProductDto> _gridFilter;
        private readonly DialogFormService _dialogFormService;
        public FrmASN(ProductService itemService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _itemService = itemService;
            dataGridView1.DataSource = _itemsBinding;
            _dialogFormService = dialogFormService;
            _gridFilter = new GridFilter<ProductDto>(dataGridView1, _itemsBinding);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoASN>();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmNuevoArticuloMasiva frmNuevoArticuloMasiva = new FrmNuevoArticuloMasiva();
            frmNuevoArticuloMasiva.ShowDialog();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo Articulos");

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var result = await _itemService.GetItems();

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                _itemsBinding.DataSource = result.Data;

                _gridFilter.SetData(result.Data);
                dataGridView1 = _gridFilter.BuildFilterColumns();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void reloadBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void gridContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                var item = dataGridView1.CurrentRow.DataBoundItem as ProductDto;

                if (item == null)
                    return;

                selectedItem = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoASNEscaneo>();
        }
    }
}
