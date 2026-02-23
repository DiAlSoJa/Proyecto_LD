
using LD.Contracts.Client;
using LD.Contracts.Item;
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
    public partial class FrmArticulos : Form
    {
        private readonly ItemService _itemService;
        private BindingSource _itemsBinding = new();
        private ItemDto? selectedItem { get; set; }
        private GridFilter<ItemDto> _gridFilter;
        private readonly DialogFormService _dialogFormService;
        public FrmArticulos(ItemService itemService,DialogFormService dialogFormService)
        {
            InitializeComponent();
            _itemService = itemService;
            dataGridView1.DataSource = _itemsBinding;
            _dialogFormService = dialogFormService;
            _gridFilter = new GridFilter<ItemDto>(dataGridView1, _itemsBinding);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoArticulo>();
       
        }
        private void EditBtn_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevoArticulo>(config =>
            {

            });
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
            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo Articulos");
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

                var item = dataGridView1.CurrentRow.DataBoundItem as ItemDto;

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

    
    }
}
