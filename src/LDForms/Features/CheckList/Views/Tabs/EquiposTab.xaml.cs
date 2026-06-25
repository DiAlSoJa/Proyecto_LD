using LD.Contracts.Equipment;
using LD.FormsX.Features.CheckList.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList.Tabs
{
    public partial class EquiposTab : UserControl
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _loaded;

        private EquiposTabViewModel ViewModel => (EquiposTabViewModel)DataContext;

        public EquiposTab(EquiposTabViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            DataContext = viewModel;
            _serviceProvider = serviceProvider;
            viewModel.OnEquiposLoaded += data => dgEquipo.ItemsSource = data;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            await ViewModel.CargarEquiposAsync();
        }

        private void dgEquipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedEquipment = dgEquipo.SelectedItem as EquipmentDto;
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.CargarEquiposAsync();
        }

        private async void BtnAsignarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedEquipment is null)
            {
                DialogHelper.ShowWarning("Selecciona un equipo para asignarle usuarios.");
                return;
            }
            var dialog = _serviceProvider.GetRequiredService<AsignarUsuarioEquipoView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetEquipment(ViewModel.SelectedEquipment);
            var result = dialog.ShowDialog();
            if (result == true)
                await ViewModel.CargarEquiposAsync();
        }

        private async void BtnNuevoEquipo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoCheckListView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            var result = dialog.ShowDialog();
            if (result == true)
                await ViewModel.CargarEquiposAsync();
        }

        private async void BtnEditarEquipo_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedEquipment is null)
            {
                DialogHelper.ShowWarning("Selecciona un equipo para editar.");
                return;
            }
            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoCheckListView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetEquipment(ViewModel.SelectedEquipment);
            var result = dialog.ShowDialog();
            if (result == true)
                await ViewModel.CargarEquiposAsync();
        }
    }
}

