using LD.Contracts.Checklist;
using LD.FormsX.Features.CheckList.ViewModels;
using LD.FormsX.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList.Tabs
{
    public partial class ResumenBateriasTab : UserControl
    {
        private ResumenBateriasTabViewModel ViewModel => (ResumenBateriasTabViewModel)DataContext;

        public ResumenBateriasTab(ResumenBateriasTabViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.OnChecklistsLoaded += checklists =>
            {
                dgBaterias.ItemsSource = checklists;
                LimpiarDetalle();
            };

            viewModel.OnChecklistDetailLoaded += detail =>
            {
                MostrarDetalle(detail);
            };
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnBuscar.IsEnabled = false;
                await ViewModel.BuscarAsync(dpBateriaDesde.SelectedDate, dpBateriaHasta.SelectedDate);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnBuscar.IsEnabled = true;
            }
        }

        private async void dgBaterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBaterias.SelectedItem is not ChecklistSummaryDto selected)
            {
                LimpiarDetalle();
                return;
            }

            try
            {
                await ViewModel.CargarDetalleAsync(selected.ChecklistId);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void MostrarDetalle(ChecklistDetailDto? detail)
        {
            if (detail is null)
            {
                LimpiarDetalle();
                return;
            }

            var rows = detail.Answers.Select(a => new
            {
                Tipo  = a.QuestionText,
                Valor = a.IsOk == true  ? $"✓ {a.AnswerText}"
                      : a.IsOk == false ? $"✗ {a.AnswerText}"
                      :                     a.AnswerText
            }).ToList<object>();

            dgBateriaDetalle.ItemsSource    = rows;
            txtObservacionesBateria.Text    = detail.Observaciones ?? string.Empty;
        }

        private void LimpiarDetalle()
        {
            dgBateriaDetalle.ItemsSource   = null;
            txtObservacionesBateria.Text    = string.Empty;
        }
    }
}
