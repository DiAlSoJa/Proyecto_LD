using LD.Contracts.Requests;
using LD.Contracts.Units;
using LD.FormsX.Features.Catalogos.Unidades.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Unidades
{
    public partial class NuevaUnidadView : Window
    {
        private NuevaUnidadViewModel ViewModel => (NuevaUnidadViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevaUnidadView(NuevaUnidadViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetUnit(UnitDto unit)
        {
            ViewModel.SetUnit(unit);
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (ViewModel.SelectedUnit is not null)
                await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            var unitI = await ViewModel.GetUnitAsync();
            if (unitI is null)
                return;

            txtId.Text = unitI.UnitIdS;
            txtNombre.Text = unitI.Description;
            txtId.IsEnabled = ViewModel.SelectedUnit == null;
        }

        private UnitRequest BuildRequest()
        {
            return new UnitRequest
            {
                UnitIdS = ViewModel.SelectedUnit != null ? ViewModel.SelectedUnit.Unidad : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim()
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                await ViewModel.SaveAsync(request, ViewModel.SelectedUnit?.Unidad ?? txtId.Text);
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
