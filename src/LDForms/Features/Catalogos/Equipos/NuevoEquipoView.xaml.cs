using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using LD.FormsX.Features.Catalogos.Equipos.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Equipos
{
    public partial class NuevoEquipoView : Window
    {
        private NuevoEquipoViewModel ViewModel => (NuevoEquipoViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevoEquipoView(NuevoEquipoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetEquipmentType(EquipmentTypeDto equipmentType)
        {
            ViewModel.SetEquipmentType(equipmentType);
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (ViewModel.SelectedEquipmentType is not null)
                await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            var equipmentType = await ViewModel.GetEquipmentTypeAsync();
            if (equipmentType is null)
                return;

            txtId.Text = equipmentType.EquipmentTypeId.ToString();
            txtNombre.Text = equipmentType.EquipmentName;
            chkIsBattery.IsChecked = equipmentType.IsBattery;
        }

        private EquipmentTypeRequest BuildRequest()
        {
            return new EquipmentTypeRequest
            {
                EquipmentTypeId = ViewModel.SelectedEquipmentType?.EquipmentTypeId ?? 0,
                EquipmentName = txtNombre.Text.Trim(),
                IsBattery = chkIsBattery.IsChecked ?? false
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                await ViewModel.SaveAsync(request, ViewModel.SelectedEquipmentType?.EquipmentTypeId ?? 0);
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

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
