using LD.Client.Services;
using LD.Contracts.Dimensioner;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Dimensionador
{
    public partial class NuevoDimensionadorView : Window
    {
        private readonly DimensionerService _dimensionerService;
        private DimensionerDto? DimensionerSelected;

        public bool ResponseForm { get; private set; }

        public NuevoDimensionadorView(DimensionerService dimensionerService)
        {
            InitializeComponent();
            _dimensionerService = dimensionerService;
        }

        public async void SetDimensioner(DimensionerDto dimensioner)
        {
            DimensionerSelected = dimensioner;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _dimensionerService.GetDimensionerById(DimensionerSelected?.DimensionerId ?? "");

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la dimensión.");
                    return;
                }

                var item = response.Data;

                txtId.Text = item.DimensionerId ?? string.Empty;
                txtNombre.Text = item.Description ?? string.Empty;
                txtAlto.Text = item.Height.ToString();
                txtLargo.Text = item.Length.ToString();
                txtAncho.Text = item.Width.ToString();
                txtPeso.Text = item.Weight.ToString();

                txtId.IsEnabled = DimensionerSelected == null;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private DimensionerRequest BuildRequest()
        {
            return new DimensionerRequest
            {
                DimensionerId = DimensionerSelected != null
                    ? DimensionerSelected.DimensionerId
                    : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim(),
                Height = ParseDecimal(txtAlto.Text),
                Length = ParseDecimal(txtLargo.Text),
                Width = ParseDecimal(txtAncho.Text),
                Weight = ParseDecimal(txtPeso.Text)
            };
        }

        private decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            if (decimal.TryParse(value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            if (decimal.TryParse(value.Trim(), NumberStyles.Any, new CultureInfo("es-MX"), out result))
                return result;

            throw new Exception($"El valor '{value}' no es numérico válido.");
        }

        private Task<ApiResponseDto<string>> CreateDimensioner(DimensionerRequest request) =>
            _dimensionerService.CreateDimensioner(request);

        private Task<ApiResponseDto<string>> EditDimensioner(string dimensionerId, DimensionerRequest request) =>
            _dimensionerService.UpdateDimensioner(dimensionerId, request);

        private async Task<ApiResponseDto<string>> SaveDimensioner(DimensionerRequest request)
        {
            return DimensionerSelected != null
                ? await EditDimensioner(DimensionerSelected.DimensionerId ?? txtId.Text, request)
                : await CreateDimensioner(request);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveDimensioner(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                    ResponseForm = true;
                    DialogResult = true;
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