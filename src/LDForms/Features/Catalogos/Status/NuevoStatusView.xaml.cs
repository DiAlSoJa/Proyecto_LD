using LD.Client.Services;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Status
{
    public partial class NuevoStatusView : Window
    {
        private readonly InventaryStatusService _statusService;
        private InventaryStatusDto? StatusSelected;

        public bool ResponseForm { get; private set; }

        public NuevoStatusView(InventaryStatusService statusService)
        {
            InitializeComponent();
            _statusService = statusService;
        }

        public async void SetInventaryStatus(InventaryStatusDto inventaryStatus)
        {
            StatusSelected = inventaryStatus;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _statusService.GetStatusById(StatusSelected?.StatusId ?? "");

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar el status.");
                    return;
                }

                var statusI = response.Data;
                txtStatus.Text = statusI.InventoryStatusIdS;
                txtNombreStatus.Text = statusI.FullName;
                chkDisponible.IsChecked = statusI.IsAvailable;

                txtStatus.IsEnabled = StatusSelected == null;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private InventaryStatusRequest BuildRequest()
        {
            return new InventaryStatusRequest
            {
                InventoryStatusIdS = StatusSelected != null ? StatusSelected.StatusId : txtStatus.Text.Trim(),
                FullName = txtNombreStatus.Text.Trim(),
                IsAvailable = chkDisponible.IsChecked ?? false
            };
        }

        private Task<ApiResponseDto<string>> CreateStatus(InventaryStatusRequest request) =>
            _statusService.CreateInventaryStatus(request);

        private Task<ApiResponseDto<string>> EditStatus(string statusId, InventaryStatusRequest request) =>
            _statusService.UpdateInventaryStatus(statusId, request);

        private async Task<ApiResponseDto<string>> SaveStatus(InventaryStatusRequest request)
        {
            return StatusSelected != null
                ? await EditStatus(StatusSelected.StatusId ?? txtStatus.Text, request)
                : await CreateStatus(request);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveStatus(request);

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