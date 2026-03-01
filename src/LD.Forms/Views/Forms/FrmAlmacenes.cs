using LD.Contracts.Client;
using LD.Contracts.Warehouse;
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
    public partial class FrmAlmacenes : Form
    {
        private readonly WarehouseService _warehouseService;
        private readonly DialogFormService _dialogFormService;

        private BindingSource _warehousesBinding = new();
        private GridFilter<WarehouseDto> _gridFilter;

        private WarehouseDto? selectedWarehouse { get; set; }
        public FrmAlmacenes(WarehouseService warehouseService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
            _dialogFormService = dialogFormService;
            dataGridView1.DataSource = _warehousesBinding;
            _gridFilter = new GridFilter<WarehouseDto>(dataGridView1, _warehousesBinding);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var form=_dialogFormService.ShowDialog<FrmNuevoAlmacen>();
            if (form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo almacenes");
        }

        private async void EditBtn_Click(object sender, EventArgs e)
        {
            var form =_dialogFormService.ShowDialog<FrmNuevoAlmacen>(config =>
            {
                config.SetWarehouse(selectedWarehouse);
            });
            if (form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo almacenes");
        }

        private async Task CargarDatosAsync()
        {
            var result = await _warehouseService.GetWarehouses();

            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }
            _warehousesBinding.DataSource = result.Data;

            _gridFilter.SetData(result.Data);
            dataGridView1 = _gridFilter.BuildFilterColumns();

        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo almacenes");

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                var cliente = dataGridView1.CurrentRow.DataBoundItem as WarehouseDto;

                if (cliente == null)
                    return;

                selectedWarehouse = cliente;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private async void btn_actualizar_Click(object sender, EventArgs e)
        {
            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo almacenes");
        }
    }
}
