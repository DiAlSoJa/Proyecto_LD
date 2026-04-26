using LD.Contracts.Checklist;
using LD.FormsX.Features.CheckList.ViewModels;
using LD.FormsX.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LD.FormsX.Views.CheckList.Tabs
{
    public partial class ResumenTab : UserControl
    {
        private ResumenTabViewModel ViewModel => (ResumenTabViewModel)DataContext;

        private List<ChecklistDefectMarkDto>? _ultimasMarcas;

        public ResumenTab(ResumenTabViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.OnChecklistsLoaded += checklists =>
            {
                dgResumen.ItemsSource = checklists;
                LimpiarDetalle();
            };

            viewModel.OnChecklistDetailLoaded += detail =>
            {
                MostrarDetalle(detail);
                _ultimasMarcas = detail?.DefectMarks;
                RenderizarMarcas(_ultimasMarcas);
            };

            // Redibujar marcas al redimensionar la ventana o el canvas
            canvasIzq.SizeChanged += (_, _) => RenderizarMarcas(_ultimasMarcas);
            canvasDer.SizeChanged += (_, _) => RenderizarMarcas(_ultimasMarcas);
        }

        private async void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnBuscar.IsEnabled = false;
                await ViewModel.BuscarAsync(dpResumenDesde.SelectedDate, dpResumenHasta.SelectedDate);
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

        private async void dgResumen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgResumen.SelectedItem is not ChecklistSummaryDto selected)
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

        private void BtnVerImagenes_Click(object sender, RoutedEventArgs e)
        {
            // Pendiente: abrir galería de fotos del checklist seleccionado.
        }

        // ──────────── Detalle ────────────

        private void MostrarDetalle(ChecklistDetailDto? detail)
        {
            if (detail is null)
            {
                LimpiarDetalle();
                return;
            }

            // Proyectar respuestas como pares Pregunta/Respuesta para el grid de detalle
            var rows = detail.Answers.Select(a => new
            {
                Tipo  = a.QuestionText,
                Valor = a.IsOk == true  ? $"✓ {a.AnswerText}"
                      : a.IsOk == false ? $"✗ {a.AnswerText}"
                      :                     a.AnswerText
            }).ToList<object>();

            dgResumenDetalle.ItemsSource = rows;
            txtObservacionesResumen.Text = detail.Observaciones ?? string.Empty;
        }

        private void LimpiarDetalle()
        {
            dgResumenDetalle.ItemsSource      = null;
            txtObservacionesResumen.Text       = string.Empty;
            canvasIzq.Children.Clear();
            canvasDer.Children.Clear();
        }

        // ──────────── Canvas de marcas X ────────────

        private void RenderizarMarcas(List<ChecklistDefectMarkDto>? marks)
        {
            canvasIzq.Children.Clear();
            canvasDer.Children.Clear();

            if (marks is null || marks.Count == 0) return;

            foreach (var mark in marks)
            {
                var canvas = mark.Side.Equals("left", StringComparison.OrdinalIgnoreCase)
                    ? canvasIzq
                    : canvasDer;

                var w = canvas.ActualWidth;
                var h = canvas.ActualHeight;

                // Canvas aún no medido; se redibujará cuando SizeChanged lo notifique
                if (w <= 0 || h <= 0) continue;

                var x = (double)mark.XPercent * w - 10;
                var y = (double)mark.YPercent * h - 14;

                var lbl = new TextBlock
                {
                    Text       = "X",
                    FontSize   = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Red
                };

                Canvas.SetLeft(lbl, x);
                Canvas.SetTop(lbl, y);
                canvas.Children.Add(lbl);
            }
        }
    }
}
