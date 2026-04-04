using LD.Client.Services;
using LD.Contracts.DTOs.Family;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Familias
{
    public partial class NuevaFamiliaView : Window
    {
        private readonly FamilyService _familyService;
        private readonly LookupService _lookupService;

        private FamilyDto? FamilySelect;
        private bool _cargandoDatos = false;

        public bool ResponseForm { get; private set; }

        public NuevaFamiliaView(FamilyService familyService, LookupService lookupService)
        {
            InitializeComponent();
            _familyService = familyService;
            _lookupService = lookupService;
        }

        public async void SetFamily(FamilyDto family)
        {
            FamilySelect = family;
            await CargarDatosInicialesAsync();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (cmbCliente.Items.Count == 0)
                await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                _cargandoDatos = true;

                await SetCombos();

                if (FamilySelect != null)
                    await CargarDatosAsync();
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private async Task SetCombos()
        {
            var clientes = await _lookupService.GetClientLookup();

            if (clientes.IsSuccess && clientes.Data != null)
            {
                cmbCliente.ItemsSource = clientes.Data;
                cmbCliente.DisplayMemberPath = "Value";
                cmbCliente.SelectedValuePath = "Key";
                cmbCliente.SelectedIndex = -1;
            }
        }

        private async Task SetCombosProjects(string projectSel = "")
        {
            if (cmbCliente.SelectedValue == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            var proyectos = await _lookupService.GetProjectClientLookup(clienteId);

            if (!proyectos.IsSuccess || proyectos.Data == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            cmbProyecto.DisplayMemberPath = "Value";
            cmbProyecto.SelectedValuePath = "Key";
            cmbProyecto.ItemsSource = proyectos.Data;

            if (!string.IsNullOrWhiteSpace(projectSel))
                cmbProyecto.SelectedValue = projectSel;
            else if (proyectos.Data.Count > 1)
                cmbProyecto.SelectedIndex = -1;
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _cargandoDatos = true;

                var response = await _familyService.GetFamilyById(FamilySelect?.FamiliaId ?? 0);

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la familia.");
                    return;
                }

                var item = response.Data;

                txtNombre.Text = item.FamilyName;
                cmbCliente.SelectedValue = item.ClientId.ToString();

                await SetCombosProjects(item.ProjectId.ToString());
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private FamilyRequest BuildRequest()
        {
            return new FamilyRequest
            {
                FamilyName = txtNombre.Text.Trim(),
                ClientId = int.TryParse(cmbCliente.SelectedValue?.ToString(), out int clienteId) ? clienteId : 0,
                ProjectId = int.TryParse(cmbProyecto.SelectedValue?.ToString(), out int projectId) ? projectId : 0
            };
        }

        private Task<ApiResponseDto<string>> CreateFamily(FamilyRequest request) =>
            _familyService.CreateFamily(request);

        private Task<ApiResponseDto<string>> EditFamily(int familyId, FamilyRequest request) =>
            _familyService.UpdateFamily(familyId, request);

        private async Task<ApiResponseDto<string>> SaveFamily(FamilyRequest request)
        {
            return FamilySelect != null
                ? await EditFamily(FamilySelect.FamiliaId, request)
                : await CreateFamily(request);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveFamily(request);

                if (result.IsSuccess)
                {
                    DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                    ResponseForm = true;
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

        private async void cmbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargandoDatos)
                return;

            await SetCombosProjects();
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