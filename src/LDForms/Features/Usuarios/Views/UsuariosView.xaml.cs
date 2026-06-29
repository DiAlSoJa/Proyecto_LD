using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LD.Contracts.User;
using LD.FormsX.Features.Usuarios.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Usuarios
{
    public partial class UsuariosView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<UserDto> _gridFilter;
        private bool _loaded;

        private UsuariosViewModel ViewModel => (UsuariosViewModel)DataContext;

        public UsuariosView(UsuariosViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;

            _gridFilter = new WpfGridFilter<UserDto>(dgUsuarios, ldBuscar.TextBoxElement);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>
            {
                { "Activo", 70 },
                { "Id",     120 },
                { "Nombre", 220 },
                { "UserName", 180 },
                { "Rol",    140 },
            });

            viewModel.OnDataLoaded += data => _gridFilter.SetData(data);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;

            await ViewModel.CargarDatosAsync();
        }

        private void DgUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectUserByDto(_gridFilter.SelectedItem);
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoUsuarioView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoUsuarioView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetUser(ViewModel.SelectedUser);

            var selectedUserId = ViewModel.SelectedUser.User?.Id;
            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosConSeleccionAsync(selectedUserId);
        }

        private async void BtnAlmacenes_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<UsuarioAlmacenView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetUser(ViewModel.SelectedUser);

            if (dialog.ShowDialog() == true)
                await ViewModel.CargarDatosAsync();
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
                Background = System.Windows.Media.Brushes.White,
            };
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
                window,
                LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            window.ShowDialog();
        }
    }
}

