using LD.Contracts.Client;
using LD.Contracts.User;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Forms
{
    public partial class FrmUsuarios : Form
    {
        private readonly DialogFormService _dialogFormService;
        private readonly UserService _userService;



        private GridFilter<UserDto> _gridFilter;
        private BindingSource _userBinding = new();
        private UserDto? userSelected { get; set; }

        public FrmUsuarios(DialogFormService dialogFormService, UserService userService)
        {
            InitializeComponent();
            _dialogFormService = dialogFormService;
            _userService = userService;

            dataGridView1.DataSource = _userBinding;
            _gridFilter = new GridFilter<UserDto>(dataGridView1, _userBinding);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoUsuario>();
            if (form.ResponseForm) await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoUsuario>(config =>
            {
                config.SetUser(userSelected);
            });
            if (form.ResponseForm) await LoaderManager.Run(panelContainer,  CargarDatosAsync, "Trayendo usuarios");
        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(panelContainer,  CargarDatosAsync, "Trayendo usuarios");

        }
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await LoaderManager.Run(panelContainer, CargarDatosAsync, "Trayendo usuarios");
        }
        private async Task CargarDatosAsync()
        {
            var result = await _userService.GetUsers();

            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }
            _userBinding.DataSource = result.Data;

            _gridFilter.SetData(result.Data);
            dataGridView1 = _gridFilter.BuildFilterColumns();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                var user = dataGridView1.CurrentRow.DataBoundItem as UserDto;

                if (user == null)
                    return;

                userSelected = user;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
