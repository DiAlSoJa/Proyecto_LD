using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.Contracts.User;
using LD.Contracts.Warehouse;
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
    public partial class UsuariosView : UserControl
    {
        private readonly UserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<UserDto> _gridFilter;

        private List<GetUserDto> _allUsers = new();
        private GetUserDto? _selectedUser;
        private bool _loaded;

        public UsuariosView(UserService userService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _userService = userService;
            _serviceProvider = serviceProvider;
            _gridFilter = new WpfGridFilter<UserDto>(dgUsuarios, txtBuscar);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo", 70 },
                { "Id",     120 },
                { "Nombre", 220 },
                { "UserName", 180 },
                { "Rol",    140 },
            });
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            await CargarDatosConLoaderAsync("Trayendo usuarios...");
        }

        private async Task CargarDatosConLoaderAsync(string mensaje)
        {
            try
            {
                MostrarLoader(true, mensaje);
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                MostrarLoader(false);
            }
        }

        private void MostrarLoader(bool mostrar, string mensaje = "Cargando...")
        {
            TxtLoading.Text = mensaje;
            LoadingOverlay.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task CargarDatosAsync()
        {
            var result = await _userService.GetUsers();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _allUsers = result.Data ?? new List<GetUserDto>();

            var users = _allUsers
                .Where(x => x.User != null)
                .Select(x => x.User!)
                .ToList();

            _gridFilter.SetData(users);
            _selectedUser = null;
            txtStatus.Text = $"Registros: {users.Count}";
            ActualizarPanelDetalle(null);
        }

        private void ActualizarPanelDetalle(GetUserDto? user)
        {
            dgPermisos.ItemsSource = user?.Permissions ?? new List<PermissionDto>();
            dgAlmacenes.ItemsSource = user?.Warehouse ?? new List<WarehouseDto>();
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await CargarDatosConLoaderAsync("Trayendo usuarios...");
        }

        private void DgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedDto = _gridFilter.SelectedItem;
                _selectedUser = _allUsers.FirstOrDefault(x => x.User?.Id == selectedDto?.Id);
                ActualizarPanelDetalle(_selectedUser);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoUsuarioView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            if (result == true)
                await CargarDatosConLoaderAsync("Trayendo usuarios...");
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser is null)
            {
                DialogHelper.ShowWarning("Selecciona un usuario para editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoUsuarioView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetUser(_selectedUser);

            var result = dialog.ShowDialog();

            if (result == true)
                await CargarDatosConLoaderAsync("Trayendo usuarios...");
        }

        private async void BtnAlmacenes_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser is null)
            {
                DialogHelper.ShowWarning("Selecciona un usuario para gestionar sus almacenes.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<UsuarioAlmacenView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetUser(_selectedUser);

            var result = dialog.ShowDialog();

            if (result == true)
                await CargarDatosConLoaderAsync("Trayendo usuarios...");
        }

        private void BtnRoles_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<RolesView>();
            var window = new Window
            {
                Title = "Roles",
                Content = dialog,
                Width = 1000,
                Height = 660,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Window.GetWindow(this),
                Background = System.Windows.Media.Brushes.White,
            };
            window.ShowDialog();
        }
    }
}

