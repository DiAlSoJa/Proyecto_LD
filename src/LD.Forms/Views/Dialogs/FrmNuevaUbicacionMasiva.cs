using LD.Client.Services;
using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevaUbicacionMasiva : DraggableForm
    {
        private bool mouseDown;
        private Point lastLocation;
        private LocationDto? LocationSelected;
        private LocationService _locationService;
        private LookupService _lookupService;

        public FrmNuevaUbicacionMasiva(LocationService locationService, LookupService lookupService)
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
            if (LocationSelected != null)
                await SetDataAsync();
        }
        public async void SetLocation(LocationDto? location)
        {
            LocationSelected = location;

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

                cmbAlmacenN.SelectedValue = location.WarehouseId.ToString();

                txtNombreUbicacion.Text = location.LocationName;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private async Task SetCombos()
        {
            var response = await _lookupService.GetWarehouseLookup();
            if (response.IsFailure)
            {
                return;
            }
            cmbAlmacenN.DataSource = response.Data;
            cmbAlmacenN.DisplayMember = "Value";
            cmbAlmacenN.ValueMember = "Key";
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

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveLocation(request);

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

        private LocationRequest BuildRequest()
        {
            return new LocationRequest
            {               
                WarehouseId = int.TryParse(cmbAlmacenN.SelectedValue?.ToString(), out int warehouseid) ? warehouseid : 0,
                Rack = txtRack.Text,
                FromW = int.TryParse(txtDesde.Text, out int fromW) ? fromW : 0,
                ToW = int.TryParse(txtHasta.Text, out int toW) ? toW : 0,
                Leves = int.TryParse(txtNiveles.Text, out int leves) ? leves : 0,
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

        private async Task<ApiResponseDto<string>> SaveLocation(LocationRequest request)
        {
            return await CreateLocation(request);
        }
        private Task<ApiResponseDto<string>> CreateLocation(LocationRequest request) =>
                _locationService.CreateLocationRange(request);

        private void ShowResult(ApiResponseDto<string> result)
        {
            MessageBox.Show(
                result.IsSuccess ? result.Data : result.Message,
                result.IsSuccess ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }


    }
}
