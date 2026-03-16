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
    public partial class FrmRoles : DraggableForm
    {

        private readonly DialogFormService _dialogFormService;
        private readonly RoleService _roleService;

        private GridFilter<RoleDto> _gridRoleFilter;
        private BindingSource _rolesBinding = new();

        private GridFilter<PermissionDto> _gridPermissionFilter;
        private BindingSource _permissionsBinding = new();

        private RoleRequest? selectedRole { get; set; }

        private List<RolePermissionDto>? _allRoles { get; set; }
        public FrmRoles(DialogFormService dialogFormService, RoleService roleService)
        {
            InitializeComponent();
            _dialogFormService = dialogFormService;
            _roleService = roleService;

            EnableDrag(panel2);
            EnableDrag(panel1);

            gridRoles.DataSource = _rolesBinding;
            gridPermisos.DataSource = _permissionsBinding;

            _gridRoleFilter = new GridFilter<RoleDto>(gridRoles, _rolesBinding);
            _gridPermissionFilter = new GridFilter<PermissionDto>(gridPermisos, _permissionsBinding);

        }



        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);


            await CargarDatosAsync();

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
                gridRoles = _gridRoleFilter.BuildFilterColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void gridRoles_SelectionChanged(object sender, EventArgs e)
        {
            if (gridRoles.CurrentRow?.DataBoundItem is not RoleDto roleDto)
            {
                selectedRole = null;
                _gridPermissionFilter.SetData(new List<PermissionDto>());
                gridPermisos = _gridPermissionFilter.BuildFilterColumns();
                return;
            }

            var selectedRoleData = _allRoles?
                .FirstOrDefault(r => string.Equals(r.Id, roleDto.RoleId, StringComparison.OrdinalIgnoreCase));

            var permissions = selectedRoleData?.Permissions ?? new List<PermissionDto>();

            selectedRole = new RoleRequest
            {
                RoleName = selectedRoleData?.RoleName,
                Permissions = permissions.ToList()
            };

            _gridPermissionFilter.SetData(permissions.ToList());
            gridPermisos = _gridPermissionFilter.BuildFilterColumns();
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


        private void addBtn_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoRol>();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoRol>();
        }

       
    }
}
