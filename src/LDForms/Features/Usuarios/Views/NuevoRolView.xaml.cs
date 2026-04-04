using LD.Client.Services;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
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
        private bool _updatingChecks;

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
                var moduleItem = CrearModuloItem(module);

                if (module.SubModules is { Count: > 0 })
                {
                    foreach (var sub in module.SubModules)
                    {
                        var subItem = CrearModuloItem(sub);
                        moduleItem.Items.Add(subItem);
                    }
                }

                treePermisos.Items.Add(moduleItem);
            }
        }

        private TreeViewItem CrearModuloItem(ModuleAuthorizationDto module)
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

                permCheckBox.Checked += (_, _) => ActualizarCheckPadre(moduleItem);
                permCheckBox.Unchecked += (_, _) => ActualizarCheckPadre(moduleItem);

                moduleItem.Items.Add(new TreeViewItem { Header = permCheckBox });
            }

            moduleCheckBox.Checked += (_, _) => MarcarTodosHijos(moduleItem, true);
            moduleCheckBox.Unchecked += (_, _) => MarcarTodosHijos(moduleItem, false);

            return moduleItem;
        }

        private void MarcarTodosHijos(TreeViewItem parentItem, bool isChecked)
        {
            if (_updatingChecks) return;
            _updatingChecks = true;

            try
            {
                foreach (TreeViewItem child in parentItem.Items)
                {
                    if (child.Header is CheckBox cb)
                    {
                        if (cb.Tag is int)
                            cb.IsChecked = isChecked;
                        else
                        {
                            cb.IsChecked = isChecked;
                            MarcarTodosHijosInterno(child, isChecked);
                        }
                    }
                }
            }
            finally
            {
                _updatingChecks = false;
            }
        }

        private void MarcarTodosHijosInterno(TreeViewItem parentItem, bool isChecked)
        {
            foreach (TreeViewItem child in parentItem.Items)
            {
                if (child.Header is CheckBox cb)
                {
                    cb.IsChecked = isChecked;

                    if (cb.Tag is not int)
                        MarcarTodosHijosInterno(child, isChecked);
                }
            }
        }

        private void ActualizarCheckPadre(TreeViewItem moduleItem)
        {
            if (_updatingChecks) return;
            _updatingChecks = true;

            try
            {
                ActualizarCheckPadreInterno(moduleItem);

                if (moduleItem.Parent is TreeViewItem grandParent
                    && grandParent.Header is CheckBox)
                {
                    ActualizarCheckPadreInterno(grandParent);
                }
            }
            finally
            {
                _updatingChecks = false;
            }
        }

        private void ActualizarCheckPadreInterno(TreeViewItem moduleItem)
        {
            if (moduleItem.Header is not CheckBox moduleCb) return;

            bool allChecked = true;
            bool hasChildren = false;

            foreach (TreeViewItem child in moduleItem.Items)
            {
                if (child.Header is CheckBox cb)
                {
                    hasChildren = true;
                    if (cb.IsChecked != true)
                    {
                        allChecked = false;
                        break;
                    }
                }
            }

            if (hasChildren)
                moduleCb.IsChecked = allChecked;
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

                MarcarPermisosRecursivo(treePermisos.Items, checkedIds);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void MarcarPermisosRecursivo(ItemCollection items, HashSet<int?> checkedIds)
        {
            _updatingChecks = true;
            try
            {
                foreach (TreeViewItem item in items)
                {
                    if (item.Header is CheckBox cb && cb.Tag is int permId)
                        cb.IsChecked = checkedIds.Contains(permId);

                    if (item.Items.Count > 0)
                        MarcarPermisosRecursivo(item.Items, checkedIds);
                }

                foreach (TreeViewItem item in items)
                {
                    if (item.Header is CheckBox cb && cb.Tag is not int && item.Items.Count > 0)
                        ActualizarCheckPadreInterno(item);
                }
            }
            finally
            {
                _updatingChecks = false;
            }
        }

        private List<PermissionDto> GetPermissions()
        {
            var permissions = new List<PermissionDto>();
            RecolectarPermisosRecursivo(treePermisos.Items, permissions);
            return permissions;
        }

        private void RecolectarPermisosRecursivo(ItemCollection items, List<PermissionDto> permissions)
        {
            foreach (TreeViewItem item in items)
            {
                if (item.Header is CheckBox cb && cb.IsChecked == true && cb.Tag is int permId)
                {
                    permissions.Add(new PermissionDto
                    {
                        PermissionId = permId,
                        PermissionName = cb.Content?.ToString(),
                    });
                }

                if (item.Items.Count > 0)
                    RecolectarPermisosRecursivo(item.Items, permissions);
            }
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
