using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Usuarios
{
    public partial class NuevoUsuarioView : Window
    {
        private readonly UserService _userService;
        private readonly LookupService _lookupService;

        private GetUserDto? _userSelected;
        public bool ResponseForm { get; private set; }

        public NuevoUsuarioView(UserService userService, LookupService lookupService)
        {
            InitializeComponent();
            _userService = userService;
            _lookupService = lookupService;
        }

        public async void SetUser(GetUserDto? user)
        {
            _userSelected = user;
            txtHeaderTitle.Text = "Editar usuario";
            await CargarDatosAsync();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarCombosAsync();
        }

        private async Task CargarCombosAsync()
        {
            var response = await _lookupService.GetRoleLookup();
            if (response.IsSuccess)
                cmbRol.ItemsSource = response.Data;
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _userService.GetUserById(_userSelected?.User?.Id ?? "");

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message);
                    return;
                }

                var user = response.Data;
                txtUsername.Text = user.Username;
                txtName.Text = user.Name;
                isActive.IsChecked = user.IsActive;

                if (user.Role is not null)
                    cmbRol.SelectedValue = user.Role;
                else
                    cmbRol.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private UserRequest BuildRequest() => new()
        {
            Username = txtUsername.Text.Trim(),
            Name = txtName.Text.Trim(),
            Password = txtPassword.Password,
            ConfirmPassword = txtConfirmPassword.Password,
            Role = cmbRol.SelectedValue?.ToString(),
            IsActive = isActive.IsChecked ?? false,
        };

        private Task<ApiResponseDto<string>> CreateUser(UserRequest request) =>
            _userService.CreateUser(request);

        private Task<ApiResponseDto<string>> EditUser(string userId, UserRequest request) =>
            _userService.UpdateUser(userId, request);

        private Task<ApiResponseDto<string>> SaveUser(UserRequest request) =>
            _userSelected != null
                ? EditUser(_userSelected.User?.Id ?? "", request)
                : CreateUser(request);

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;
                var request = BuildRequest();
                var result = await SaveUser(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                    ResponseForm = true;
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
