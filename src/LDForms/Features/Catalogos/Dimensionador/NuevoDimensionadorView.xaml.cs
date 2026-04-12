using LD.Contracts.Dimensioner;
using LD.Contracts.Requests;
using LD.FormsX.Features.Catalogos.Dimensionador.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Dimensionador
{
    public partial class NuevoDimensionadorView : Window
    {
        private NuevoDimensionadorViewModel ViewModel => (NuevoDimensionadorViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevoDimensionadorView(NuevoDimensionadorViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetDimensioner(DimensionerDto dimensioner)
        {
            ViewModel.SetDimensioner(dimensioner);
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (ViewModel.SelectedDimensioner is not null)
                await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            var item = await ViewModel.GetDimensionerAsync();
            if (item is null)
                return;

            txtId.Text = item.DimensionerId ?? string.Empty;
            txtNombre.Text = item.Description ?? string.Empty;
            txtAlto.Text = item.Height.ToString();
            txtLargo.Text = item.Length.ToString();
            txtAncho.Text = item.Width.ToString();
            txtPeso.Text = item.Weight.ToString();
            txtId.IsEnabled = ViewModel.SelectedDimensioner == null;
        }

        private DimensionerRequest BuildRequest()
        {
            return new DimensionerRequest
            {
                DimensionerId = ViewModel.SelectedDimensioner != null
                    ? ViewModel.SelectedDimensioner.DimensionerId
                    : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim(),
                Height = ViewModel.ParseDecimal(txtAlto.Text),
                Length = ViewModel.ParseDecimal(txtLargo.Text),
                Width = ViewModel.ParseDecimal(txtAncho.Text),
                Weight = ViewModel.ParseDecimal(txtPeso.Text)
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                await ViewModel.SaveAsync(request, ViewModel.SelectedDimensioner?.DimensionerId ?? txtId.Text);
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
