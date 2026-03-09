using LD.Client.Services;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevaUbicacion : DraggableForm
    {

        private LocationDto? LocationSelected;
        private LocationService _locationService;
        private LookupService _lookupService;

        public FrmNuevaUbicacion(LocationService locationService, LookupService lookupService)
        {
            InitializeComponent();
            EnableDrag(panel1);
            EnableDrag(panel2);
            _locationService = locationService;
            _lookupService = lookupService;
        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await SetCombos();
            if(LocationSelected!=null)
                await SetDataAsync();
        }
        public async void SetLocation(LocationDto? location)
        {
            LocationSelected = location;
   
        }

        private async Task SetCombos()
        {
            var response = await _lookupService.GetWarehouseLookup();
            if (response.IsFailure)
            { 
                return;
            }
            cmbAlmacen.DataSource = response.Data;
            cmbAlmacen.DisplayMember = "Value";
            cmbAlmacen.ValueMember = "Key";
        }

        private async Task SetDataAsync()
        {
            try
            {
                var response = await _locationService.GetLocationById(LocationSelected?.LocationId ?? 0);


                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var location = response.Data;

                cmbAlmacen.SelectedValue = location.WarehouseId?.ToString();

                txtNombreUbicacion.Text = location.LocationName;

                checkIsActive.Checked = location.IsActive;
                checkIsFiscal.Checked = location.IsFiscal;
                checkTemperatura.Checked = location.HasControlledTemperature;

                txtAltoCm.Text = location.Height?.ToString();
                txtAnchoCm.Text = location.Width?.ToString();
                txtProfundidadCm.Text = location.Depth?.ToString();

                radioRack.Checked = location.IsRack;
                radioCompartidoType.Checked = location.IsCompartidoType;

                radioGeneral.Checked = location.IsGeneral;
                radioCuarentena.Checked = location.IsCuarentena;
                radioEmbarque.Checked = location.IsEmbarque;
                radioCompartido.Checked = location.IsCompartido;
                radioReciboEmbarque.Checked = location.IsReciboYEmbarque;

                radioDoble.Checked = location.IsDoble;
                radioSencillo.Checked = location.IsSencillo;

                checkPaso.Checked = location.HasPaso;
                checkCortina.Checked = location.HasCortina;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Task<ApiResponseDto<string>> CreateClient(LocationRequest request) =>
   _locationService.CreateLocation(request);

        private Task<ApiResponseDto<string>> EditClient(int locationId, LocationRequest request) =>
            _locationService.UpdateLocation(locationId, request);
        private async Task<ApiResponseDto<string>> SaveClient(LocationRequest request)
        {
            return LocationSelected != null
                ? await EditClient(LocationSelected?.LocationId ?? 0, request)
                : await CreateClient(request);
        }
        private LocationRequest BuildRequest()
        {
            return new LocationRequest
            {
                LocationId =  LocationSelected != null ? LocationSelected.LocationId : 0,
                WarehouseId = int.TryParse( cmbAlmacen.SelectedValue?.ToString(),out int warehouseid)?warehouseid:null ,
                LocationName = txtNombreUbicacion.Text,
                IsActive = checkIsActive.Checked,
                IsFiscal = checkIsFiscal.Checked,
                HasControlledTemperature = checkTemperatura.Checked,

                Height = decimal.TryParse(txtAltoCm.Text, out decimal alto) ? alto : null,
                Width = decimal.TryParse(txtAnchoCm.Text, out decimal ancho) ? ancho : null,
                Depth = decimal.TryParse(txtProfundidadCm.Text, out decimal profundidad) ? profundidad : null,

                IsRack = radioRack.Checked,
                IsCompartidoType = radioCompartidoType.Checked,

                IsGeneral = radioGeneral.Checked,
                IsCuarentena = radioCuarentena.Checked,
                IsEmbarque = radioEmbarque.Checked,
                IsCompartido = radioCompartido.Checked,
                IsReciboYEmbarque = radioReciboEmbarque.Checked,

                IsDoble = radioDoble.Checked,
                IsSencillo = radioSencillo.Checked,

                HasPaso = checkPaso.Checked,
                HasCortina = checkCortina.Checked
            };
        }
        private void ShowResult(ApiResponseDto<string> result)
        {
            MessageBox.Show(
                result.IsSuccess ? result.Data : result.Message,
                result.IsSuccess ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveClient(request);

                ShowResult(result);
                ResponseForm = result.IsSuccess;
                if (result.IsSuccess)
                    this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error inesperado: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

    }
}
