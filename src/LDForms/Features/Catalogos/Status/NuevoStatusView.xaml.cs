using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Status
{
    public partial class NuevoStatusView : Window
    {
        private readonly InventaryStatusService _statusService;
        private readonly LookupService _lookupService;
        private InventaryStatusDto? StatusSelected;
        private bool _loaded;

        public bool ResponseForm { get; private set; }

        public NuevoStatusView(InventaryStatusService statusService, LookupService lookupService)
        {
            InitializeComponent();
            _statusService = statusService;
            _lookupService = lookupService;

            cmbCliente.DisplayMemberPath = nameof(DropDownDto.Value);
            cmbCliente.SelectedValuePath = nameof(DropDownDto.Key);
            cmbProyecto.DisplayMemberPath = nameof(DropDownDto.Value);
            cmbProyecto.SelectedValuePath = nameof(DropDownDto.Key);
        }

        public void SetInventaryStatus(InventaryStatusDto inventaryStatus)
        {
            StatusSelected = inventaryStatus;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
            {
                return;
            }

            _loaded = true;

            try
            {
                await CargarLookupsAsync();

                if (StatusSelected is not null)
                {
                    await CargarDatosAsync();
                }
                else
                {
                    PrepararParaNuevo();
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task CargarLookupsAsync()
        {
            var clientTask = _lookupService.GetClientLookup();
            var projectTask = _lookupService.GetProjectLookup();

            await Task.WhenAll(clientTask, projectTask);

            var clients = clientTask.Result.Data ?? new List<DropDownDto>();
            var projects = projectTask.Result.Data ?? new List<DropDownDto>();

            cmbCliente.ItemsSource = clients
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .OrderBy(x => x.Value)
                .ToList();

            cmbProyecto.ItemsSource = projects
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .OrderBy(x => x.Value)
                .ToList();
        }

        private async Task CargarDatosAsync()
        {
            var statusId = StatusSelected?.StatusId ?? string.Empty;
            var clientId = StatusSelected?.ClientId ?? 0;
            var projectId = StatusSelected?.ProjectId ?? 0;
            var response = await _statusService.GetStatusById(statusId, clientId, projectId);

            if (!response.IsSuccess || response.Data is null)
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo cargar el status.");
                return;
            }

            var statusI = response.Data;
            txtStatus.Text = statusI.InventoryStatusIdS;
            txtNombreStatus.Text = statusI.FullName;
            chkDisponible.IsChecked = statusI.IsAvailable;

            cmbCliente.SelectedValue = statusI.ClientId.ToString();
            cmbProyecto.SelectedValue = statusI.ProjectId.ToString();

            txtStatus.IsEnabled = false;
        }

        private void PrepararParaNuevo()
        {
            txtStatus.Text = string.Empty;
            txtNombreStatus.Text = string.Empty;
            chkDisponible.IsChecked = false;
            cmbCliente.SelectedIndex = -1;
            cmbProyecto.SelectedIndex = -1;
            txtStatus.IsEnabled = true;
        }

        private InventaryStatusRequest BuildRequest()
        {
            return new InventaryStatusRequest
            {
                InventoryStatusIdS = StatusSelected != null ? StatusSelected.StatusId : txtStatus.Text.Trim(),
                FullName = txtNombreStatus.Text.Trim(),
                ClientId = ReadSelectedId(cmbCliente),
                ProjectId = ReadSelectedId(cmbProyecto),
                IsAvailable = chkDisponible.IsChecked ?? false
            };
        }

        private static int ReadSelectedId(System.Windows.Controls.ComboBox comboBox)
        {
            var value = comboBox.SelectedValue?.ToString() ?? comboBox.Text?.Trim();
            if (int.TryParse(value, out var parsed))
            {
                return parsed;
            }

            return 0;
        }

        private Task<ApiResponseDto<string>> CreateStatus(InventaryStatusRequest request) =>
            _statusService.CreateInventaryStatus(request);

        private Task<ApiResponseDto<string>> EditStatus(string statusId, int clientId, int projectId, InventaryStatusRequest request) =>
            _statusService.UpdateInventaryStatus(statusId, clientId, projectId, request);

        private async Task<ApiResponseDto<string>> SaveStatus(InventaryStatusRequest request)
        {
            return StatusSelected != null
                ? await EditStatus(StatusSelected.StatusId ?? txtStatus.Text, StatusSelected.ClientId, StatusSelected.ProjectId, request)
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
                    DialogResult = true;
                    Close();
                }
                else
                {
                    DialogHelper.ShowError(result.ErrorMessage ?? result.Message ?? "Hubo un error al guardar.");
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
