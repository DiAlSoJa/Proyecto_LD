using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmUserWarehouse : DraggableForm
    {
        private readonly WarehouseService _warehouseService;
        private readonly UserService _userService;

        private GridFilter<WarehouseDto> _addedFilter;
        private BindingSource _addedBinding = new();

        private GridFilter<WarehouseDto> _availableFilter;
        private BindingSource _availableBinding = new();

        private GetUserDto? _selectedUser { get; set; }

        private List<WarehouseDto> _allWarehouses = new();
        private List<WarehouseDto> _addedWarehouses = new();
        private List<WarehouseDto> _availableWarehouses = new();

        private WarehouseDto? _selectedAdded { get; set; }
        private WarehouseDto? _selectedAvailable { get; set; }

        public FrmUserWarehouse(WarehouseService warehouseService, UserService userService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
            _userService = userService;

            EnableDrag(panel2);
            EnableDrag(panel1);
            gridWarehouseAdded .DataSource = _addedBinding;
            gridWarehouseFaltantes.DataSource = _availableBinding;

            _addedFilter = new GridFilter<WarehouseDto>(gridWarehouseAdded, _addedBinding);
            _availableFilter = new GridFilter<WarehouseDto>(gridWarehouseFaltantes, _availableBinding);
        }

        public void SetUser(GetUserDto user)
        {
            _selectedUser = user;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo almacenes");
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _warehouseService.GetWarehouses();

                if (response.IsFailure)
                {
                    MessageBox.Show(response.Message);
                    return;
                }

                _allWarehouses = response.Data ?? new List<WarehouseDto>();

                var assignedIds = _selectedUser?.Warehouse?
                    .Select(w => w.Id)
                    .ToHashSet() ?? new HashSet<int>();

                _addedWarehouses     = _allWarehouses.Where(w =>  assignedIds.Contains(w.Id)).ToList();
                _availableWarehouses = _allWarehouses.Where(w => !assignedIds.Contains(w.Id)).ToList();

                RefreshGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshGrids()
        {
            _addedBinding.DataSource = _addedWarehouses.ToList();
            _addedFilter.SetData(_addedWarehouses.ToList());
            gridWarehouseAdded = _addedFilter.BuildFilterColumns();

            _availableBinding.DataSource = _availableWarehouses.ToList();
            _availableFilter.SetData(_availableWarehouses.ToList());
            gridWarehouseFaltantes = _availableFilter.BuildFilterColumns();
        }

        private async Task SaveAsync()
        {
            if (_selectedUser?.User?.Id is null) return;

            var request = new UserWarehouseRequest
            {
                WarehouseIds = _addedWarehouses.Select(w => w.Id).ToList()
            };

            var result = await _userService.AssignWarehouses(_selectedUser.User.Id, request);

            if (result.IsFailure)
                MessageBox.Show(result.Message);
            ResponseForm=true;
        }

        // << agregar almacén al usuario
        private async void addWBtn_Click(object sender, EventArgs e)
        {
            if (_selectedAvailable is null) return;

            _addedWarehouses.Add(_selectedAvailable);
            _availableWarehouses.Remove(_selectedAvailable);
            _selectedAvailable = null;

            RefreshGrids();
            await SaveAsync();
        }

        // >> quitar almacén del usuario
        private async void removeBtn_Click(object sender, EventArgs e)
        {
            if (_selectedAdded is null) return;

            _availableWarehouses.Add(_selectedAdded);
            _addedWarehouses.Remove(_selectedAdded);
            _selectedAdded = null;

            RefreshGrids();
            await SaveAsync();
        }

        private void gridWarehouseAdded_SelectionChanged(object sender, EventArgs e)
        {
            _selectedAdded = gridWarehouseAdded.CurrentRow?.DataBoundItem as WarehouseDto;
        }

        private void gridWarehouseFaltantes_SelectionChanged(object sender, EventArgs e)
        {
            _selectedAvailable = gridWarehouseFaltantes.CurrentRow?.DataBoundItem as WarehouseDto;
        }

        private void btnAceptar_Click(object sender, EventArgs e)    => this.Close();
        private void pictureBox2_Click(object sender, EventArgs e)   => this.Close();
        private void button2_Click(object sender, EventArgs e)       => this.Close();
        private void panel2_DoubleClick(object sender, EventArgs e)  { }
    }
}

