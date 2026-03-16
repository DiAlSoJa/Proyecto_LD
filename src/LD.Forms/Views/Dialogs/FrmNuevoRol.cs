using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.DTOs.User;
using LD.Contracts.Enums;
using LD.Contracts.Requests;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoRol : DraggableForm
    {
        private readonly ModuleService _moduleService;
        private readonly RoleService _roleService;

        private RoleDto? RoleSelected { get; set; }
        private readonly DialogMessageService _dialogService;

        public FrmNuevoRol(ModuleService moduleService, RoleService roleService, DialogMessageService dialogService)
        {
            InitializeComponent();
            EnableDrag(panel2);
            EnableDrag(panel1);
            _moduleService = moduleService;
            _roleService = roleService;
            _dialogService = dialogService;

        }

        public void SetRole(RoleDto role)
        {
            RoleSelected = role;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            treePermissions.Nodes.Clear();

            var response = await _moduleService.GetModules();
            var allModules = response.Data ?? new List<ModuleAuthorizationDto>();

            foreach (var module in allModules)
            {
                TreeNode moduleNode = new TreeNode(module.ModuleName);

                foreach (var permission in module.Permissions)
                {
                    TreeNode permissionNode = new TreeNode(permission.PermissionName)
                    {
                        Tag = permission.PermissionId
                    };

                    moduleNode.Nodes.Add(permissionNode);
                }

                treePermissions.Nodes.Add(moduleNode);
            }

            if (RoleSelected != null)
                await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _roleService.GetRoleById(RoleSelected?.RoleId);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var role = response.Data;

                txtRoleName.Text = role?.RoleName ?? "";

                var permissions = role?.Permissions.Select(p => p.PermissionId).ToHashSet() ?? new HashSet<int?>();

                foreach (TreeNode moduleNode in treePermissions.Nodes)
                {
                    foreach (TreeNode permissionNode in moduleNode.Nodes)
                    {
                        if (permissionNode.Tag is int permId)
                            permissionNode.Checked = permissions.Contains(permId);
                    }
                }

                treePermissions.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        private Task<ApiResponseDto<string>> CreateRole(RoleRequest request) =>
            _roleService.CreateRol (request);

        private Task<ApiResponseDto<string>> UpdateRole(string roleId, RoleRequest request) =>
            _roleService.UpdateRol(roleId, request);
        private async Task<ApiResponseDto<string>> SaveClient(RoleRequest request)
        {
            return RoleSelected != null
                ? await UpdateRole(RoleSelected?.RoleId??"", request)
                : await CreateRole(request);
        }

        private RoleRequest BuildRequest()
        {
            return new RoleRequest
            {
                RoleName = txtRoleName.Text.Trim(),
                Permissions = GetPermissions()
            };
        }

        private void ShowResult(ApiResponseDto<string> result)
        {
            _dialogService.Show(
                 result.IsSuccess ? result.Data ?? "" : $"Hubo un error: {Environment.NewLine}{result.ErrorMessage ?? ""}",
                  result.IsSuccess ? DialogMessageEnum.Info : DialogMessageEnum.Error
                );


        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                ApiResponseDto<string> result;
                if (RoleSelected != null )
                    result = await _roleService.UpdateRol(RoleSelected.RoleId, request);
                else
                    result = await _roleService.CreateRol(request);

                ShowResult(result);

                ResponseForm = result.IsSuccess;
                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                _dialogService.Show($"Hubo un error: {Environment.NewLine}{ex.Message}", DialogMessageEnum.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private List<PermissionDto> GetPermissions()
        {
            List<PermissionDto> permissions = new();

            foreach (TreeNode module in treePermissions.Nodes)
            {
                foreach (TreeNode permission in module.Nodes)
                {
                    if (permission.Checked)
                    {
                        permissions.Add(new PermissionDto
                        {
                            PermissionId = int.TryParse(permission?.Tag?.ToString(), out int p) ? p : 0,
                            PermissionName = permission?.Text
                        });
                    }
                }
            }
            return permissions;
        }
        private void treePermissions_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }
    }
}
