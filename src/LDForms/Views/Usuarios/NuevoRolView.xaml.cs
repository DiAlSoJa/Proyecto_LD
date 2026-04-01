using LD.Client.Services;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LD.FormsX.Views.Usuarios
{
    public partial class NuevoRolView : Window
    {
        private readonly ModuleService _moduleService;
        private readonly RoleService _roleService;

        private RoleDto? _roleSelected;

        public NuevoRolView(ModuleService moduleService, RoleService roleService)
        {
            InitializeComponent();
            _moduleService = moduleService;
            _roleService = roleService;
        }

        public void SetRole(RoleDto role)
        {
            _roleSelected = role;
            txtHeaderTitle.Text = "Editar Rol";
        }

        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            await CargarModulosAsync();

            if (_roleSelected is not null)
                await CargarDatosAsync();
        }

        private async Task CargarModulosAsync()
        {
            var response = await _moduleService.GetModules();
            var allModules = response.Data ?? new List<ModuleAuthorizationDto>();

            treePermisos.Items.Clear();

            foreach (var module in allModules)
            {
                var moduleCheckBox = new CheckBox
                {
                    Content = module.ModuleName,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 13,
                    Foreground = new SolidColorBrush(Color.FromRgb(31, 41, 55)),
                    Margin = new Thickness(2, 2, 0, 2),
                };

                var moduleItem = new TreeViewItem
                {
                    Header = moduleCheckBox,
                    IsExpanded = true,
                };

                foreach (var perm in module.Permissions)
                {
                    var permCheckBox = new CheckBox
                    {
                        Content = perm.PermissionName,
                        Tag = perm.PermissionId,
                        FontSize = 12,
                        Foreground = new SolidColorBrush(Color.FromRgb(55, 65, 81)),
                        Margin = new Thickness(2, 1, 0, 1),
                    };

                    moduleItem.Items.Add(new TreeViewItem { Header = permCheckBox });
                }

                moduleCheckBox.Checked += (_, _) =>
                {
                    foreach (TreeViewItem permItem in moduleItem.Items)
                        if (permItem.Header is CheckBox cb) cb.IsChecked = true;
                };
                moduleCheckBox.Unchecked += (_, _) =>
                {
                    foreach (TreeViewItem permItem in moduleItem.Items)
                        if (permItem.Header is CheckBox cb) cb.IsChecked = false;
                };

                treePermisos.Items.Add(moduleItem);
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _roleService.GetRoleById(_roleSelected?.RoleId);
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message);
                    return;
                }

                var role = response.Data;
                txtRoleName.Text = role?.RoleName ?? "";

                var checkedIds = role?.Permissions
                    .Select(p => p.PermissionId)
                    .ToHashSet() ?? new HashSet<int?>();

                foreach (TreeViewItem moduleItem in treePermisos.Items)
                {
                    foreach (TreeViewItem permItem in moduleItem.Items)
                    {
                        if (permItem.Header is CheckBox cb && cb.Tag is int permId)
                            cb.IsChecked = checkedIds.Contains(permId);
                    }
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private List<PermissionDto> GetPermissions()
        {
            var permissions = new List<PermissionDto>();

            foreach (TreeViewItem moduleItem in treePermisos.Items)
            {
                foreach (TreeViewItem permItem in moduleItem.Items)
                {
                    if (permItem.Header is CheckBox cb && cb.IsChecked == true && cb.Tag is int permId)
                    {
                        permissions.Add(new PermissionDto
                        {
                            PermissionId = permId,
                            PermissionName = cb.Content?.ToString(),
                        });
                    }
                }
            }

            return permissions;
        }

        private RoleRequest BuildRequest() => new()
        {
            RoleName = txtRoleName.Text.Trim(),
            Permissions = GetPermissions(),
        };

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;
                var request = BuildRequest();

                var result = _roleSelected is not null
                    ? await _roleService.UpdateRol(_roleSelected.RoleId, request)
                    : await _roleService.CreateRol(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                    DialogResult = true;
                }
                else
                {
                    DialogHelper.ShowError(result.Message);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
