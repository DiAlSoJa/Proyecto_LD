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
    public partial class NuevaUbicacionView : Window
    {
        private readonly LocationService _locationService;
        private readonly LookupService _lookupService;

        private LocationDto? _locationSelected;

        public bool ResponseForm { get; private set; }

        public NuevaUbicacionView(LocationService locationService, LookupService lookupService)
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

                cmbAlmacen.ItemsSource = response.Data;
                cmbAlmacen.DisplayMemberPath = "Value";
                cmbAlmacen.SelectedValuePath = "Key";
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

                cmbAlmacen.IsEnabled = false;
                txtNombreUbicacion.IsEnabled = false;

                if (!response.IsSuccess || response.Data == null)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la ubicación.");
                    return;
                }

                var location = response.Data;

                cmbAlmacen.SelectedValue = location.WarehouseId.ToString();
                txtNombreUbicacion.Text = location.LocationName ?? string.Empty;

                checkIsActive.IsChecked = location.IsActive;
                checkIsFiscal.IsChecked = location.IsFiscal;
                checkTemperatura.IsChecked = location.HasControlledTemperature;

                txtAltoCm.Text = location.Height?.ToString() ?? string.Empty;
                txtAnchoCm.Text = location.Width?.ToString() ?? string.Empty;
                txtProfundidadCm.Text = location.Depth?.ToString() ?? string.Empty;

                radioRack.IsChecked = location.IsRack;
                radioCompartidoType.IsChecked = location.IsCompartidoType;

                radioGeneral.IsChecked = location.IsGeneral;
                radioCuarentena.IsChecked = location.IsCuarentena;
                radioEmbarque.IsChecked = location.IsEmbarque;
                radioCompartido.IsChecked = location.IsCompartido;
                radioReciboEmbarque.IsChecked = location.IsReciboYEmbarque;

                radioDoble.IsChecked = location.IsDoble;
                radioSencillo.IsChecked = location.IsSencillo;

                checkPaso.IsChecked = location.HasPaso;
                checkCortina.IsChecked = location.HasCortina;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError($"Ocurrió un error al cargar la información: {ex.Message}");
            }
        }

        private Task<ApiResponseDto<string>> CreateLocationAsync(LocationRequest request) =>
            _locationService.CreateLocation(request);

        private Task<ApiResponseDto<string>> EditLocationAsync(int locationId, LocationRequest request) =>
            _locationService.UpdateLocation(locationId, request);

        private async Task<ApiResponseDto<string>> SaveLocationAsync(LocationRequest request)
        {
            return _locationSelected != null
                ? await EditLocationAsync(_locationSelected.LocationId, request)
                : await CreateLocationAsync(request);
        }

        private LocationRequest BuildRequest()
        {
            var locationName = txtNombreUbicacion.Text?.Trim() ?? string.Empty;

            string level = string.Empty;
            string position = string.Empty;
            string rack = string.Empty;

            if (!string.IsNullOrWhiteSpace(locationName))
            {
                if (locationName.Length >= 1)
                    level = locationName.Substring(locationName.Length - 1, 1);

                if (locationName.Length >= 3)
                    position = locationName.Substring(locationName.Length - 3, 2);

                if (locationName.Length > 3)
                    rack = locationName.Substring(0, locationName.Length - 3);
                else
                    rack = locationName;
            }

            return new LocationRequest
            {
                LocationId = _locationSelected != null ? _locationSelected.LocationId : 0,
                WarehouseId = int.TryParse(cmbAlmacen.SelectedValue?.ToString(), out int warehouseId) ? warehouseId : 0,
                LocationName = locationName,

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
                HasCortina = checkCortina.IsChecked == true,

                Level = level,
                Position = position,
                Rack = rack
            };
        }

        private bool ValidateForm()
        {
            if (cmbAlmacen.SelectedValue == null)
            {
                DialogHelper.ShowWarning("Selecciona un almacén.");
                cmbAlmacen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNombreUbicacion.Text))
            {
                DialogHelper.ShowWarning("Captura el nombre de la ubicación.");
                txtNombreUbicacion.Focus();
                return false;
            }

            return true;
        }

        private void ShowResult(ApiResponseDto<string> result)
        {
            if (result.IsSuccess)
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
            else
                DialogHelper.ShowError(result.Message ?? "Ocurrió un error al guardar la ubicación.");
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