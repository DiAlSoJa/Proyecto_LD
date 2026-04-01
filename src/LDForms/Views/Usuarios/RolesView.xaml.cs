using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.Usuarios
{
    public partial class RolesView : UserControl
    {
        private readonly RoleService _roleService;
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<RoleDto> _gridFilter;

        private List<RolePermissionDto> _allRoles = new();
        private RoleDto? _selectedRole;
        private bool _loaded;

        public RolesView(RoleService roleService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _roleService = roleService;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<RoleDto>(dgRoles, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "RoleId",   160 },
                { "RoleName", 240 },
            });
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            await CargarDatosConLoaderAsync("Trayendo roles...");
        }

        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
            txtLoadingMsg.Text = mensaje;
            LoadingOverlay.Visibility = Visibility.Visible;
            try { await CargarDatosAsync(); }
            finally { LoadingOverlay.Visibility = Visibility.Collapsed; }
        }

        private async Task CargarDatosAsync()
        {
            var response = await _roleService.GetRols();
            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.Message);
                return;
            }

            _allRoles = response.Data ?? new List<RolePermissionDto>();
            var roles = _allRoles
                .Select(r => new RoleDto { RoleId = r.Id, RoleName = r.RoleName })
                .ToList();
            _gridFilter.SetData(roles);
            dgPermisos.ItemsSource = null;
            txtPermisosHeader.Text = "Permisos del Rol";
        }

        private void DgRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedRole = _gridFilter.SelectedItem;

            if (_selectedRole is null)
            {
                dgPermisos.ItemsSource = null;
                txtPermisosHeader.Text = "Permisos del Rol";
                return;
            }

            var found = _allRoles.FirstOrDefault(r =>
                string.Equals(r.Id, _selectedRole.RoleId, StringComparison.OrdinalIgnoreCase));

            dgPermisos.ItemsSource = found?.Permissions;
            txtPermisosHeader.Text = $"Permisos — {_selectedRole.RoleName}";
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoRolView>();
            if (dialog.ShowDialog() == true)
                await CargarDatosConLoaderAsync("Actualizando...");
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRole is null)
            {
                DialogHelper.ShowWarning("Selecciona un rol para editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoRolView>();
            dialog.SetRole(_selectedRole);
            if (dialog.ShowDialog() == true)
                await CargarDatosConLoaderAsync("Actualizando...");
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Actualizando...");
        }
    }
}
