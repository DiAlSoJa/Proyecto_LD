using LD.Contracts.Location;
using LD.Contracts.Requests;
using LD.Forms.Classes.DTOs;
using LD.Forms.Services;
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
    public partial class FrmNuevaUbicacion : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        private LocationDto? LocationSelected;
        private LocationService _locationService;
        public FrmNuevaUbicacion(LocationService locationService)
        {
            InitializeComponent();
            _locationService = locationService;
        }
       
        public async void SetLocation(LocationDto? location)
        {
            LocationSelected = location;
            await SetDataAsync();
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
                var client = response.Data;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
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

        private Task<ApiResponseDto<string>> EditClient(int clientId, LocationRequest request) =>
            _locationService.UpdateLocation(clientId, request);
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
