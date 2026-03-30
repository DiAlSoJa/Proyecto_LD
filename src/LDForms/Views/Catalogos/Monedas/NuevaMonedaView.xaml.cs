using LD.Client.Services;
using LD.Contracts.Currency;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Monedas
{
    public partial class NuevaMonedaView : Window
    {
        private readonly CurrencyService _currencyService;
        private CurrencyDto? CurrencySelect;

        public bool ResponseForm { get; private set; }

        public NuevaMonedaView(CurrencyService currencyService)
        {
            InitializeComponent();
            _currencyService = currencyService;
        }

        public async void SetCurrency(CurrencyDto currency)
        {
            CurrencySelect = currency;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _currencyService.GetCurrencyById(CurrencySelect?.CurrencyIdS ?? "");

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la moneda.");
                    return;
                }

                var currencyI = response.Data;
                txtId.Text = currencyI.CurrencyIdS;
                txtNombre.Text = currencyI.Description;
                txtId.IsEnabled = CurrencySelect == null;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private CurrencyRequest BuildRequest()
        {
            return new CurrencyRequest
            {
                CurrencyIdS = CurrencySelect != null ? CurrencySelect.CurrencyIdS : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim()
            };
        }

        private Task<ApiResponseDto<string>> CreateCurrency(CurrencyRequest request) =>
            _currencyService.CreateCurrency(request);

        private Task<ApiResponseDto<string>> EditCurrency(string currencyId, CurrencyRequest request) =>
            _currencyService.UpdateCurrency(currencyId, request);

        private async Task<ApiResponseDto<string>> SaveCurrency(CurrencyRequest request)
        {
            return CurrencySelect != null
                ? await EditCurrency(CurrencySelect.CurrencyIdS ?? txtId.Text, request)
                : await CreateCurrency(request);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveCurrency(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                    ResponseForm = true;
                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
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