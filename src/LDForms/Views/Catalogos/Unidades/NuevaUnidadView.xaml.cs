using LD.Client.Services;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.Units;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Unidades
{
    public partial class NuevaUnidadView : Window
    {
        private readonly UnitService _unitService;
        private UnitDto? UnitSelected;

        public bool ResponseForm { get; private set; }

        public NuevaUnidadView(UnitService unitService)
        {
            InitializeComponent();
            _unitService = unitService;
        }

        public async void SetUnit(UnitDto unit)
        {
            UnitSelected = unit;
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _unitService.GetUnitById(UnitSelected?.Unidad ?? "");

                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? "No se pudo cargar la unidad.");
                    return;
                }

                var unitI = response.Data;
                txtId.Text = unitI.UnitIdS;
                txtNombre.Text = unitI.Description;
                txtId.IsEnabled = UnitSelected == null;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private UnitRequest BuildRequest()
        {
            return new UnitRequest
            {
                UnitIdS = UnitSelected != null ? UnitSelected.Unidad : txtId.Text.Trim(),
                Description = txtNombre.Text.Trim()
            };
        }

        private Task<ApiResponseDto<string>> CreateUnit(UnitRequest request) =>
            _unitService.CreateUnit(request);

        private Task<ApiResponseDto<string>> EditUnit(string unitId, UnitRequest request) =>
            _unitService.UpdateUnit(unitId, request);

        private async Task<ApiResponseDto<string>> SaveUnit(UnitRequest request)
        {
            return UnitSelected != null
                ? await EditUnit(UnitSelected.Unidad ?? txtId.Text, request)
                : await CreateUnit(request);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                var result = await SaveUnit(request);

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