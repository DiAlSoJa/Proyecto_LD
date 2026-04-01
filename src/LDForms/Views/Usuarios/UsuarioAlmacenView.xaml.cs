using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Warehouse;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Usuarios
{
    public partial class UsuarioAlmacenView : Window
    {
        private readonly WarehouseService _warehouseService;
        private readonly UserService _userService;

        private GetUserDto? _selectedUser;

        private List<WarehouseDto> _allWarehouses = new();
        private List<WarehouseDto> _addedWarehouses = new();
        private List<WarehouseDto> _availableWarehouses = new();

        private WarehouseDto? _selectedAdded;
        private WarehouseDto? _selectedAvailable;

        public bool ResponseForm { get; private set; }

        public UsuarioAlmacenView(WarehouseService warehouseService, UserService userService)
        {
            InitializeComponent();
            _warehouseService = warehouseService;
            _userService = userService;
        }

        public void SetUser(GetUserDto user)
        {
            _selectedUser = user;
            if (user.User?.Nombre is not null)
                txtHeaderTitle.Text = $"Almacenes — {user.User.Nombre}";
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _warehouseService.GetWarehouses();

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowWarning(response.Message);
                    return;
                }

                _allWarehouses = response.Data ?? new List<WarehouseDto>();

                var assignedIds = _selectedUser?.Warehouse?
                    .Select(w => w.Id)
                    .ToHashSet() ?? new HashSet<int>();

                _addedWarehouses = _allWarehouses.Where(w => assignedIds.Contains(w.Id)).ToList();
                _availableWarehouses = _allWarehouses.Where(w => !assignedIds.Contains(w.Id)).ToList();

                RefreshGrids();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void RefreshGrids()
        {
            dgDisponibles.ItemsSource = null;
            dgDisponibles.ItemsSource = _availableWarehouses.ToList();

            dgAsignados.ItemsSource = null;
            dgAsignados.ItemsSource = _addedWarehouses.ToList();
        }

        private async Task SaveAsync()
        {
            if (_selectedUser?.User?.Id is null) return;

            var request = new UserWarehouseRequest
            {
                WarehouseIds = _addedWarehouses.Select(w => w.Id).ToList()
            };

            var result = await _userService.AssignWarehouses(_selectedUser.User.Id, request);

            if (!result.IsSuccess)
                DialogHelper.ShowError(result.Message);
            else
                ResponseForm = true;
        }

        private async void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAvailable is null) return;

            _addedWarehouses.Add(_selectedAvailable);
            _availableWarehouses.Remove(_selectedAvailable);
            _selectedAvailable = null;

            RefreshGrids();
            await SaveAsync();
        }

        private async void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAdded is null) return;

            _availableWarehouses.Add(_selectedAdded);
            _addedWarehouses.Remove(_selectedAdded);
            _selectedAdded = null;

            RefreshGrids();
            await SaveAsync();
        }

        private void DgAsignados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedAdded = dgAsignados.SelectedItem as WarehouseDto;
        }

        private void DgDisponibles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedAvailable = dgDisponibles.SelectedItem as WarehouseDto;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
