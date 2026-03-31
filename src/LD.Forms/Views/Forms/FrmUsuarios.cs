using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.DTOs.User;
using LD.Contracts.User;
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
    public partial class FrmUsuarios : Form
    {
        private readonly DialogFormService _dialogFormService;
        private readonly UserService _userService;

        private GridFilter<PermissionDto> _permissionFilter;
        private BindingSource _permissionSource = new();

        private GridFilter<WarehouseDto> _warehouseFilter;
        private BindingSource _warehouseSource = new();

        private GridFilter<UserDto> _gridFilter;
        private BindingSource _userBinding = new();
        private GetUserDto? userSelected { get; set; }

        private List<GetUserDto> _allUsers = new();

        public FrmUsuarios(DialogFormService dialogFormService, UserService userService)
        {
            InitializeComponent();
            _dialogFormService = dialogFormService;
            _userService = userService;

            usersGrid.DataSource = _userBinding;
            _gridFilter = new GridFilter<UserDto>(usersGrid, _userBinding);

            permissionGrid.DataSource = _permissionSource;
            _permissionFilter = new GridFilter<PermissionDto>(permissionGrid, _permissionSource);

            warehouseGrid.DataSource = _warehouseSource;
            _warehouseFilter = new GridFilter<WarehouseDto>(warehouseGrid, _warehouseSource);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoUsuario>();
            if (form.ResponseForm) await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoUsuario>(config =>
            {
                config.SetUser(userSelected);
            });
            if (form.ResponseForm) await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");
        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");

        }
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");
        }
        private async Task CargarDatosAsync()
        {
            var result = await _userService.GetUsers();

            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }

            _allUsers = result.Data ?? new List<GetUserDto>();

            var users = _allUsers
                .Where(x => x.User != null)
                .Select(x => x.User!)
                .ToList();

            _gridFilter.SetData(users);
            usersGrid = _gridFilter.BuildFilterColumns();

            _permissionFilter.SetData(new List<PermissionDto>());
            permissionGrid = _permissionFilter.BuildFilterColumns();

            _warehouseFilter.SetData(new List<WarehouseDto>());
            warehouseGrid = _warehouseFilter.BuildFilterColumns();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (usersGrid.CurrentRow?.DataBoundItem is not UserDto userDto)
                    return;

                userSelected = _allUsers.FirstOrDefault(x => x.User?.Id == userDto.Id);

                var permissions = userSelected?.Permissions ?? new List<PermissionDto>();
                _permissionFilter.SetData(permissions);
                permissionGrid = _permissionFilter.BuildFilterColumns();

                var warehouses = userSelected?.Warehouse ?? new List<WarehouseDto>();
                _warehouseFilter.SetData(warehouses);
                warehouseGrid = _warehouseFilter.BuildFilterColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewRol_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoRol>();
        }

        private void btnEditRol_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoRol>();
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmRoles>();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {

        }

        private async void warehouseBtn_Click(object sender, EventArgs e)
        {

            var form = _dialogFormService.ShowDialog<FrmUserWarehouse>(config =>
            {
                config.SetUser(userSelected);
            });
            
            if(form.ResponseForm) await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");

        }
    }
}
