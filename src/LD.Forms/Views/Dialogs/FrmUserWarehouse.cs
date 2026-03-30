using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Contracts.Warehouse;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using LD.Forms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;   
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmUserWarehouse : DraggableForm
    {

        private readonly DialogFormService _dialogFormService;
        private readonly RoleService _roleService;

        private GridFilter<RoleDto> _gridRoleFilter;
        private BindingSource _rolesBinding = new();

        private GridFilter<PermissionDto> _gridPermissionFilter;
        private BindingSource _permissionsBinding = new();


        private RoleDto? selectedRole { get; set; }
        private List<RolePermissionDto>? _allRoles { get; set; }
        public FrmUserWarehouse(DialogFormService dialogFormService, RoleService roleService)
        {
            InitializeComponent();
            _dialogFormService = dialogFormService;
            _roleService = roleService;

            EnableDrag(panel2);
            EnableDrag(panel1);

            gridWarehouseAdded.DataSource = _rolesBinding;
            gridWarehouseFaltantes.DataSource = _permissionsBinding;

            _gridRoleFilter = new GridFilter<RoleDto>(gridWarehouseAdded, _rolesBinding);
            _gridPermissionFilter = new GridFilter<PermissionDto>(gridWarehouseFaltantes, _permissionsBinding);

        }



        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(panelContainer, async () => await CargarDatosAsync(), "Trayendo roles");



        }



        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _roleService.GetRols();

                if (response.IsFailure)
                {
                    MessageBox.Show(response.Message);
                    return;
                }

                _allRoles = response.Data;

                _rolesBinding.DataSource = _allRoles;
                var roleDtos = _allRoles.Select(r => new RoleDto { RoleId = r.Id, RoleName = r.RoleName }).ToList();
                _gridRoleFilter.SetData(roleDtos);
                gridWarehouseAdded = _gridRoleFilter.BuildFilterColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void gridRoles_SelectionChanged(object sender, EventArgs e)
        {
            if (gridWarehouseAdded.CurrentRow?.DataBoundItem is not RoleDto roleDto)
            {
                selectedRole = null;
                _gridPermissionFilter.SetData(new List<PermissionDto>());
                gridWarehouseFaltantes = _gridPermissionFilter.BuildFilterColumns();
                return;
            }


            var selectedRoleData = _allRoles?
                .FirstOrDefault(r => string.Equals(r.Id, roleDto.RoleId, StringComparison.OrdinalIgnoreCase));

            var permissions = selectedRoleData?.Permissions ?? new List<PermissionDto>();

            selectedRole = roleDto;

            _gridPermissionFilter.SetData(permissions.ToList());
            gridWarehouseFaltantes = _gridPermissionFilter.BuildFilterColumns();
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


        private async void addBtn_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoRol>();
            if (form.ResponseForm) await LoaderManager.Run(panelContainer, async () => await CargarDatosAsync(), "Trayendo roles");
        }

        private async void editBtn_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoRol>(config =>
            {
                config.SetRole(selectedRole);
            });
            if (form.ResponseForm) await LoaderManager.Run(panelContainer, async () => await CargarDatosAsync(), "Trayendo roles");
        }

        private void gridRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addWBtn_Click(object sender, EventArgs e)
        {

        }

        private void removeBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
