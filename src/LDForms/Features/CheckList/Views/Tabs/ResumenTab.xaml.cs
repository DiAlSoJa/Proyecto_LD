using LD.Contracts.Checklist;
using LD.FormsX.Features.CheckList.ViewModels;
using LD.FormsX.Helpers;
using System.Diagnostics;
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

        // Descarga las fotos del checklist seleccionado y las abre con el visor de imágenes del SO.
        private async void BtnVerImagenes_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.DetalleActual?.Photos is null || ViewModel.DetalleActual.Photos.Count == 0)
            {
                DialogHelper.ShowInfo("Este checklist no tiene fotos registradas.");
                return;
            }

            try
            {
                BtnVerImagenes.IsEnabled = false;
                var rutas = await ViewModel.DescargarFotosAsync();

                if (rutas.Count == 0)
                {
                    DialogHelper.ShowInfo("No se pudieron descargar las fotos.");
                    return;
                }

                foreach (var ruta in rutas)
                    Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError($"Error al abrir fotos: {ex.Message}");
            }
            finally
            {
                BtnVerImagenes.IsEnabled = true;
            }
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

            // Agregar fotos como filas informativas (thumbnail o enlace)
            foreach (var photo in detail.Photos.OrderBy(p => p.Order))
                rows.Add(new
                {
                    Tipo  = $"📷 Foto {photo.Order} ({photo.Side})",
                    Valor = photo.RelativePath
                });

            dgResumenDetalle.ItemsSource      = rows;
            txtObservacionesResumen.Text      = detail.Observaciones ?? string.Empty;
            BtnVerImagenes.IsEnabled          = detail.Photos.Count > 0;
        }

        private void LimpiarDetalle()
        {
            dgResumenDetalle.ItemsSource      = null;
            txtObservacionesResumen.Text       = string.Empty;
            canvasIzq.Children.Clear();
            canvasDer.Children.Clear();
            BtnVerImagenes.IsEnabled           = false;
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
