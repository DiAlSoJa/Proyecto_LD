using LD.Contracts.EquipmentQuestion;
using LD.Contracts.EquipmentType;
using LD.FormsX.Features.CheckList.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList.Tabs
{
    public partial class ConfiguracionPreguntasTab : UserControl
    {
        private bool _loaded;
        private ConfiguracionPreguntasTabViewModel ViewModel => (ConfiguracionPreguntasTabViewModel)DataContext;

        public ConfiguracionPreguntasTab(ConfiguracionPreguntasTabViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.OnTiposLoaded += tipos =>
            {
                cmbTipoPregunta.ItemsSource = tipos;
                cmbTipoPregunta.DisplayMemberPath = nameof(EquipmentTypeDto.EquipmentName);
                cmbTipoPregunta.SelectedValuePath = nameof(EquipmentTypeDto.EquipmentTypeId);
                if (tipos.Count > 0)
                    cmbTipoPregunta.SelectedIndex = 0;
                btnEditarPregunta.IsEnabled = false;
                btnEliminarPregunta.IsEnabled = false;
                txtOpciones.IsEnabled = false;
            };

            viewModel.OnPreguntasLoaded += preguntas =>
            {
                dgPreguntas.ItemsSource = preguntas;
                ViewModel.SetSelectedQuestion(null);
                SyncBotones();
            };

            viewModel.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(viewModel.CanEditQuestion) or nameof(viewModel.CanDeleteQuestion))
                    SyncBotones();
            };
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            await ViewModel.CargarTiposAsync();
        }

        private async void cmbTipoPregunta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded || cmbTipoPregunta.SelectedValue is not int equipmentTypeId || equipmentTypeId <= 0)
                return;
            LimpiarFormulario();
            await ViewModel.CargarPreguntasAsync(equipmentTypeId);
        }

        private void dgPreguntas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SetSelectedQuestion(dgPreguntas.SelectedItem as EquipmentQuestionDto);
            SyncBotones();
        }

        private void TipoRespuesta_Checked(object sender, RoutedEventArgs e)
        {
            if (txtOpciones is null) return;
            var usesOptions = rbOpciones.IsChecked == true;
            txtOpciones.IsEnabled = usesOptions;
            if (!usesOptions)
                txtOpciones.Text = string.Empty;
        }

        private async void BtnGuardarPregunta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnGuardarPregunta.IsEnabled = false;

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

                var ok = await ViewModel.GuardarPreguntaAsync(equipmentTypeId, txtPregunta.Text.Trim(), isYesNo, options);
                if (ok)
                {
                    LimpiarFormulario();
                    await ViewModel.CargarPreguntasAsync(equipmentTypeId);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnGuardarPregunta.IsEnabled = true;
            }
        }

        private void BtnEditarPregunta_Click(object sender, RoutedEventArgs e)
        {
            var q = ViewModel.SelectedQuestion;
            if (q is null)
            {
                DialogHelper.ShowWarning("Selecciona una pregunta para editar.");
                return;
            }
            cmbTipoPregunta.SelectedValue = q.EquipmentTypeId;
            txtPregunta.Text = q.QuestionText;
            rbSiNo.IsChecked = q.IsYesNo;
            rbOpciones.IsChecked = !q.IsYesNo;
            txtOpciones.Text = q.OptionAnswerText ?? string.Empty;
            txtOpciones.IsEnabled = !q.IsYesNo;
            txtBtnGuardar.Text = "Actualizar";
        }

        private async void BtnEliminarPregunta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnEliminarPregunta.IsEnabled = false;
                if (ViewModel.SelectedQuestion is null)
                {
                    DialogHelper.ShowWarning("Selecciona una pregunta para eliminar.");
                    return;
                }
                var typeId = ViewModel.SelectedQuestion.EquipmentTypeId;
                var ok = await ViewModel.EliminarPreguntaAsync();
                if (ok)
                {
                    LimpiarFormulario();
                    await ViewModel.CargarPreguntasAsync(typeId);
                }
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnEliminarPregunta.IsEnabled = ViewModel.CanDeleteQuestion;
            }
        }

        private void LimpiarFormulario()
        {
            ViewModel.SetSelectedQuestion(null);
            dgPreguntas.SelectedItem = null;
            txtPregunta.Text = string.Empty;
            txtOpciones.Text = string.Empty;
            rbSiNo.IsChecked = true;
            rbOpciones.IsChecked = false;
            txtOpciones.IsEnabled = false;
            txtBtnGuardar.Text = "Guardar";
            SyncBotones();
        }

        private void SyncBotones()
        {
            btnEditarPregunta.IsEnabled = ViewModel.CanEditQuestion;
            btnEliminarPregunta.IsEnabled = ViewModel.CanDeleteQuestion;
        }
    }
}
