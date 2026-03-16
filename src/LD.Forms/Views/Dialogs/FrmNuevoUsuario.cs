
using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Requests;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Contracts.User;
using LD.Contracts.Warehouse;
using LD.Forms.Services;
using LD.Forms.Views.Common;
using LD.Forms.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoUsuario : DraggableForm//, ICreateUserView
    {


        private readonly UserService _userService;
        private readonly LookupService _lookupService;

        private UserDto? UserSelected{ get; set; }
        public FrmNuevoUsuario(UserService userService, LookupService lookupService)
        {
            InitializeComponent();
            _userService = userService;
            _lookupService = lookupService;
            EnableDrag(panel2);
            EnableDrag(panel1);
        }

        //public event EventHandler CreateUser;
        //public event EventHandler UpdateUser;
        //public event EventHandler Exit;

        public string Username => txtUsername.Text;
        public string Password => txtPassword.Text;
        public string ConfirmPassword => txtConfirmPassword.Text;
        public bool IsActive => cckIsActive.Checked;

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await SetCombos();

            if(UserSelected!=null)
                await CargarDatosAsync();   

        }
        private async Task SetCombos()
        {
            var response = await _lookupService.GetRoleLookup();
            if (response.IsFailure)
            {
                return;
            }
            cmbRol.DataSource = response.Data;
            cmbRol.DisplayMember = "Value";
            cmbRol.ValueMember = "Key";
        }
        public async void SetUser(UserDto? user)
        {
            UserSelected = user;
          
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _userService.GetUserById(UserSelected?.Id??"");

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var user = response.Data;

                txtUsername.Text = user.Username;
                txtName.Text=user.Name;
                cckIsActive.Checked = user.IsActive;
                if(user.Role is not null)
                    cmbRol.SelectedValue = user.Role;
                else cmbRol.SelectedIndex = -1;


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
        private UserRequest BuildRequest()
        {

            return new UserRequest
            {
               Username = txtUsername.Text,
               Name = txtName.Text,
               Email = null,
               Password = txtPassword.Text,
               ConfirmPassword = txtConfirmPassword.Text,
               Role= cmbRol.SelectedValue?.ToString(),
                IsActive = cckIsActive.Checked,
            };
        }
       
        private void ShowResult(ApiResponseDto<string> result)
        {
            MessageBox.Show(
                result.IsSuccess ? result.Data : result.Message,
                result.IsSuccess ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private Task<ApiResponseDto<string>> CreateUser(UserRequest request) =>
            _userService.CreateUser(request);

        private Task<ApiResponseDto<string>> EditUser(string clientId, UserRequest request) =>
            _userService.UpdateUser(clientId, request);

        private async Task<ApiResponseDto<string>> SaveUser(UserRequest request)
        {
            return UserSelected != null
                ? await EditUser(UserSelected?.Id, request)
                : await CreateUser(request);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveUser(request);

                ShowResult(result);

                ResponseForm = result.IsSuccess;
                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }
    }
}
