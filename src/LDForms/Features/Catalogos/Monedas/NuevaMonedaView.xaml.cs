using LD.Contracts.Currency;
using LD.Contracts.Requests;
using LD.FormsX.Features.Catalogos.Monedas.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Monedas
{
    public partial class NuevaMonedaView : Window
    {
        private NuevaMonedaViewModel ViewModel => (NuevaMonedaViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevaMonedaView(NuevaMonedaViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetCurrency(CurrencyDto currency)
        {
            ViewModel.SetCurrency(currency);
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (ViewModel.SelectedCurrency is not null)
                await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            var currencyI = await ViewModel.GetCurrencyAsync();
            if (currencyI is null)
                return;

            txtId.Text = currencyI.CurrencyIdS;
            txtNombre.Text = currencyI.Description;
            txtId.IsEnabled = ViewModel.SelectedCurrency == null;
        }

        private CurrencyRequest BuildRequest()
        {
            return new CurrencyRequest
            {
                CurrencyIdS = ViewModel.SelectedCurrency != null ? ViewModel.SelectedCurrency.CurrencyIdS : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim()
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                await ViewModel.SaveAsync(request, ViewModel.SelectedCurrency?.CurrencyIdS ?? txtId.Text);
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
