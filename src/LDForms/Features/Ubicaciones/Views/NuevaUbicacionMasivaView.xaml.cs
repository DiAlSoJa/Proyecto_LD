using LD.Client.Services;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Ubicaciones
{
    public partial class NuevaUbicacionMasivaView : Window
    {
        private readonly LocationService _locationService;
        private readonly LookupService _lookupService;

        private LocationDto? _locationSelected;

        public bool ResponseForm { get; private set; }

        public NuevaUbicacionMasivaView(LocationService locationService, LookupService lookupService)
        {
            InitializeComponent();
            _locationService = locationService;
            _lookupService = lookupService;
        }

        public void SetLocation(LocationDto? location)
        {
            _locationSelected = location;
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            await SetCombosAsync();

            if (_locationSelected != null)
                await SetDataAsync();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async Task SetCombosAsync()
        {
            try
            {
                var response = await _lookupService.GetWarehouseLookup();

                if (response.IsFailure || response.Data == null)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudieron cargar los almacenes.");
                    return;
                }

                cmbAlmacenN.ItemsSource = response.Data;
                cmbAlmacenN.DisplayMemberPath = "Value";
                cmbAlmacenN.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError($"Ocurrió un error al cargar los almacenes: {ex.Message}");
            }
        }

        private async Task SetDataAsync()
        {
            try
            {
                var response = await _locationService.GetLocationById(_locationSelected?.LocationId ?? 0);

                if (!response.IsSuccess || response.Data == null)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la ubicación.");
                    return;
                }

                var location = response.Data;

                cmbAlmacenN.SelectedValue = location.WarehouseId.ToString();
                txtRack.Text = location.Rack ?? string.Empty;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError($"Ocurrió un error al cargar la información: {ex.Message}");
            }
        }

        private LocationRequest BuildRequest()
        {
            return new LocationRequest
            {
                WarehouseId = int.TryParse(cmbAlmacenN.SelectedValue?.ToString(), out int warehouseId) ? warehouseId : 0,
                Rack = txtRack.Text?.Trim() ?? string.Empty,

                FromW = int.TryParse(txtDesde.Text, out int fromW) ? fromW : 0,
                ToW = int.TryParse(txtHasta.Text, out int toW) ? toW : 0,
                Leves = int.TryParse(txtNiveles.Text, out int levels) ? levels : 0,

                IsActive = checkIsActive.IsChecked == true,
                IsFiscal = checkIsFiscal.IsChecked == true,
                HasControlledTemperature = checkTemperatura.IsChecked == true,

                Height = decimal.TryParse(txtAltoCm.Text, out decimal alto) ? alto : null,
                Width = decimal.TryParse(txtAnchoCm.Text, out decimal ancho) ? ancho : null,
                Depth = decimal.TryParse(txtProfundidadCm.Text, out decimal profundidad) ? profundidad : null,

                IsRack = radioRack.IsChecked == true,
                IsCompartidoType = radioCompartidoType.IsChecked == true,

                IsGeneral = radioGeneral.IsChecked == true,
                IsCuarentena = radioCuarentena.IsChecked == true,
                IsEmbarque = radioEmbarque.IsChecked == true,
                IsCompartido = radioCompartido.IsChecked == true,
                IsReciboYEmbarque = radioReciboEmbarque.IsChecked == true,

                IsDoble = radioDoble.IsChecked == true,
                IsSencillo = radioSencillo.IsChecked == true,

                HasPaso = checkPaso.IsChecked == true,
                HasCortina = checkCortina.IsChecked == true
            };
        }

        private bool ValidateForm()
        {
            if (cmbAlmacenN.SelectedValue == null)
            {
                DialogHelper.ShowWarning("Selecciona un almacén.");
                cmbAlmacenN.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRack.Text))
            {
                DialogHelper.ShowWarning("Captura el rack.");
                txtRack.Focus();
                return false;
            }

            if (!int.TryParse(txtDesde.Text, out int desde) || desde <= 0)
            {
                DialogHelper.ShowWarning("Captura una posición inicial válida.");
                txtDesde.Focus();
                return false;
            }

            if (!int.TryParse(txtHasta.Text, out int hasta) || hasta <= 0)
            {
                DialogHelper.ShowWarning("Captura una posición final válida.");
                txtHasta.Focus();
                return false;
            }

            if (hasta < desde)
            {
                DialogHelper.ShowWarning("La posición final no puede ser menor que la inicial.");
                txtHasta.Focus();
                return false;
            }

            if (!int.TryParse(txtNiveles.Text, out int niveles) || niveles < 1 || niveles > 36)
            {
                DialogHelper.ShowWarning("Los niveles deben estar entre 1 y 36.");
                txtNiveles.Focus();
                return false;
            }

            return true;
        }

        private Task<ApiResponseDto<string>> CreateLocationRangeAsync(LocationRequest request) =>
            _locationService.CreateLocationRange(request);

        private async Task<ApiResponseDto<string>> SaveLocationAsync(LocationRequest request)
        {
            return await CreateLocationRangeAsync(request);
        }

        private void ShowResult(ApiResponseDto<string> result)
        {
            if (result.IsSuccess)
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
            else
                DialogHelper.ShowError(result.Message ?? "Ocurrió un error al guardar las ubicaciones.");
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveLocationAsync(request);

                ShowResult(result);

                ResponseForm = result.IsSuccess;

                if (result.IsSuccess)
                {
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError($"Error inesperado: {ex.Message}");
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }
    }
}