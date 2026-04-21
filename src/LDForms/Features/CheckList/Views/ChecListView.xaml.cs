using LD.Client.Services;
using LD.Contracts.EquipmentQuestion;
using LD.Contracts.Equipment;
using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList
{
    public partial class CheckListView : UserControl
    {
        private readonly EquipmentService _equipmentService;
        private readonly EquipmentTypeService _equipmentTypeService;
        private readonly EquipmentQuestionService _equipmentQuestionService;
        private readonly IServiceProvider _serviceProvider;
        private bool _loaded;
        private EquipmentQuestionDto? _selectedQuestion;

        private EquipmentDto? SelectedEquipment => dgEquipo.SelectedItem as EquipmentDto;

        public CheckListView(
            EquipmentService equipmentService,
            EquipmentTypeService equipmentTypeService,
            EquipmentQuestionService equipmentQuestionService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _equipmentService = equipmentService;
            _equipmentTypeService = equipmentTypeService;
            _equipmentQuestionService = equipmentQuestionService;
            _serviceProvider = serviceProvider;

            btnEditar.Click += BtnEditarEquipo_Click;
            Loaded += UserControl_Loaded;
            cmbTipoPregunta.SelectionChanged += cmbTipoPregunta_SelectionChanged;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
                return;

            _loaded = true;
            await CargarEquiposAsync();
            await CargarTiposPreguntaAsync();
        }

        private void BtnActualizarEquipo_Click(object sender, RoutedEventArgs e) { }
        private void BtnAsignarUsuario_Click(object sender, RoutedEventArgs e) { }

        private async void BtnNuevoEquipo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoCheckListView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();
            if (result == true)
                await CargarEquiposAsync();
        }

        private async void BtnEditarEquipo_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedEquipment is null)
            {
                DialogHelper.ShowWarning("Selecciona un equipo para editar.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoCheckListView>();
            dialog.Owner = Window.GetWindow(this);
            dialog.SetEquipment(SelectedEquipment);

            var result = dialog.ShowDialog();
            if (result == true)
                await CargarEquiposAsync();
        }

        private void BtnEliminarEquipo_Click(object sender, RoutedEventArgs e) { }
        private void dgEquipo_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void BtnBuscarResumen_Click(object sender, RoutedEventArgs e) { }
        private void dgResumen_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void BtnVerImagenes_Click(object sender, RoutedEventArgs e) { }

        private void BtnBuscarBaterias_Click(object sender, RoutedEventArgs e) { }
        private void dgBaterias_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private async void BtnGuardarPregunta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btndccNuevo.IsEnabled = false;

                if (cmbTipoPregunta.SelectedValue is not int equipmentTypeId || equipmentTypeId <= 0)
                {
                    DialogHelper.ShowWarning("Selecciona un tipo de equipo.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPregunta.Text))
                {
                    DialogHelper.ShowWarning("Captura una pregunta.");
                    return;
                }

                var isYesNo = rbSiNo.IsChecked == true;
                var options = isYesNo ? null : txtOpciones.Text;

                if (!isYesNo && string.IsNullOrWhiteSpace(options))
                {
                    DialogHelper.ShowWarning("Captura al menos una opción de respuesta.");
                    return;
                }

                var request = new EquipmentQuestionRequest
                {
                    EquipmentQuestionDetId = _selectedQuestion?.EquipmentQuestionDetId ?? 0,
                    EquipmentTypeId = equipmentTypeId,
                    QuestionText = txtPregunta.Text.Trim(),
                    IsYesNo = isYesNo,
                    OptionAnswerText = options
                };

                var response = await _equipmentQuestionService.Save(request);
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo guardar la pregunta.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Pregunta guardada correctamente.");
                LimpiarFormularioPreguntas();
                await CargarPreguntasAsync(equipmentTypeId);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btndccNuevo.IsEnabled = true;
            }
        }

        private void BtnEditarPregunta_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedQuestion is null)
            {
                DialogHelper.ShowWarning("Selecciona una pregunta para editar.");
                return;
            }

            cmbTipoPregunta.SelectedValue = _selectedQuestion.EquipmentTypeId;
            txtPregunta.Text = _selectedQuestion.QuestionText;
            rbSiNo.IsChecked = _selectedQuestion.IsYesNo;
            rbOpciones.IsChecked = !_selectedQuestion.IsYesNo;
            txtOpciones.Text = _selectedQuestion.OptionAnswerText ?? string.Empty;
            txtOpciones.IsEnabled = !_selectedQuestion.IsYesNo;
            btndccNuevo.Content = "Actualizar";
        }

        private async void BtnEliminarPregunta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedQuestion is null)
                {
                    DialogHelper.ShowWarning("Selecciona una pregunta para eliminar.");
                    return;
                }

                BtnEliminarFF.IsEnabled = false;

                var response = await _equipmentQuestionService.Delete(_selectedQuestion.EquipmentQuestionDetId);
                if (!response.IsSuccess)
                {
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudo eliminar la pregunta.");
                    return;
                }

                DialogHelper.ShowSuccess(response.Message ?? "Pregunta eliminada correctamente.");
                var equipmentTypeId = _selectedQuestion.EquipmentTypeId;
                LimpiarFormularioPreguntas();
                await CargarPreguntasAsync(equipmentTypeId);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                BtnEliminarFF.IsEnabled = true;
            }
        }

        private void dgPreguntas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedQuestion = dgPreguntas.SelectedItem as EquipmentQuestionDto;
            btnEditarFF.IsEnabled = _selectedQuestion is not null;
            BtnEliminarFF.IsEnabled = _selectedQuestion is not null;
        }

        private void TipoRespuesta_Checked(object sender, RoutedEventArgs e)
        {
            if (txtOpciones is null)
                return;

            var usesOptions = rbOpciones.IsChecked == true;
            txtOpciones.IsEnabled = usesOptions;

            if (!usesOptions)
                txtOpciones.Text = string.Empty;
        }

        private async Task CargarEquiposAsync()
        {
            try
            {
                var response = await _equipmentService.GetEquipments();

                if (!response.IsSuccess || response.Data is null)
                {
                    dgEquipo.ItemsSource = null;
                    txtStatusEquipo.Text = response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los equipos.";
                    return;
                }

                dgEquipo.ItemsSource = response.Data;
                txtStatusEquipo.Text = response.Data.Count > 0
                    ? $"{response.Data.Count} equipo(s) cargado(s)"
                    : "Sin equipos para mostrar";
            }
            catch (Exception ex)
            {
                dgEquipo.ItemsSource = null;
                txtStatusEquipo.Text = "No se pudieron cargar los equipos.";
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void cmbTipoPregunta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded || cmbTipoPregunta.SelectedValue is not int equipmentTypeId || equipmentTypeId <= 0)
                return;

            LimpiarFormularioPreguntas();
            await CargarPreguntasAsync(equipmentTypeId);
        }

        private async Task CargarTiposPreguntaAsync()
        {
            try
            {
                var response = await _equipmentTypeService.GetEquipmentTypes();

                if (!response.IsSuccess || response.Data is null)
                {
                    cmbTipoPregunta.ItemsSource = null;
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los tipos de equipo.");
                    return;
                }

                var data = response.Data.OrderBy(x => x.EquipmentName).ToList();
                cmbTipoPregunta.ItemsSource = data;
                cmbTipoPregunta.DisplayMemberPath = nameof(EquipmentTypeDto.EquipmentName);
                cmbTipoPregunta.SelectedValuePath = nameof(EquipmentTypeDto.EquipmentTypeId);

                if (data.Count > 0)
                {
                    cmbTipoPregunta.SelectedIndex = 0;
                    await CargarPreguntasAsync(data[0].EquipmentTypeId);
                }

                btnEditarFF.IsEnabled = false;
                BtnEliminarFF.IsEnabled = false;
                txtOpciones.IsEnabled = false;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async Task CargarPreguntasAsync(int equipmentTypeId)
        {
            var response = await _equipmentQuestionService.GetByEquipmentType(equipmentTypeId);

            if (!response.IsSuccess || response.Data is null)
            {
                dgPreguntas.ItemsSource = null;
                return;
            }

            dgPreguntas.ItemsSource = response.Data.Select(x => new EquipmentQuestionDto
            {
                EquipmentQuestionId = x.EquipmentQuestionId,
                EquipmentQuestionDetId = x.EquipmentQuestionDetId,
                EquipmentTypeId = x.EquipmentTypeId,
                EquipmentTypeName = x.EquipmentTypeName,
                QuestionText = x.QuestionText,
                OptionAnswerText = x.IsYesNo ? "Si / No" : (x.OptionAnswerText ?? string.Empty),
                IsYesNo = x.IsYesNo
            }).ToList();
        }

        private void LimpiarFormularioPreguntas()
        {
            _selectedQuestion = null;
            dgPreguntas.SelectedItem = null;
            txtPregunta.Text = string.Empty;
            txtOpciones.Text = string.Empty;
            rbSiNo.IsChecked = true;
            rbOpciones.IsChecked = false;
            txtOpciones.IsEnabled = false;
            btndccNuevo.Content = "Guardar";
            btnEditarFF.IsEnabled = false;
            BtnEliminarFF.IsEnabled = false;
        }
    }
}
