using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using MauiAppLogin.Common.Imaging;
using MauiAppLogin.Controls;
using MauiAppLogin.Models;
using MauiAppLogin.ViewModels;
using Microsoft.Maui.Layouts;

namespace MauiAppLogin;

[QueryProperty(nameof(Equipment),   "Equipment")]
[QueryProperty(nameof(IsMandatory), "IsMandatory")]
public partial class ForkliftChecklistPage : ContentPage
{
    private readonly List<Label> _leftMarks  = new();
    private readonly List<Label> _rightMarks = new();
    private readonly EquipmentService  _equipmentService;
    private readonly ChecklistService  _checklistService;
    private readonly IDialogService    _dialogService;

    // Cuando es true el usuario DEBE completar el checklist antes de salir
    private bool _isMandatory;

    private EquipmentDto? _equipment;
    public EquipmentDto? Equipment
    {
        get => _equipment;
        set
        {
            _equipment = value;
            if (value is not null)
                _ = InicializarConEquipoAsync(value);
        }
    }

    public bool IsMandatory
    {
        get => _isMandatory;
        set => _isMandatory = value;
    }

    private ForkliftChecklistViewModel ViewModel => (ForkliftChecklistViewModel)BindingContext;

    public ForkliftChecklistPage(
        ForkliftChecklistViewModel viewModel,
        EquipmentService equipmentService,
        ChecklistService checklistService,
        IDialogService dialogService)
    {
        InitializeComponent();
        BindingContext     = viewModel;
        _equipmentService = equipmentService;
        _checklistService = checklistService;
        _dialogService    = dialogService;
        FechaPicker.Date  = DateTime.Today;
    }

    // Bloquea el botón Atrás del dispositivo cuando el checklist es obligatorio.
    protected override bool OnBackButtonPressed()
    {
        if (_isMandatory)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await _dialogService.ShowInfoAsync(
                    "Checklist requerido",
                    "Debes completar el checklist de tu equipo antes de continuar.");
            });
            return true; // consume el evento y bloquea la navegación
        }
        return base.OnBackButtonPressed();
    }

    private async Task InicializarConEquipoAsync(EquipmentDto equipment)
    {
        await ViewModel.InicializarAsync(equipment);
        await CargarImagenEquipoAsync(equipment);
        ViewModel.Equipo = equipment.NoEquipo;

        // La verificación de checklist diario ahora ocurre antes de navegar (DashboardViewModel.NavigateToChecklist)
        // por lo que no es necesario repetirla aquí.
    }

    /* FUERA DE USO — Sprint 3 Hotfix (2026-05-14)
     * La verificación se movió a DashboardViewModel.NavigateToChecklist(),
     * que se ejecuta antes de navegar a esta página.
     * Se conserva comentado como referencia.
     *
     * private async Task VerificarChecklistDiarioAsync()
     * {
     *     try
     *     {
     *         var response = await _checklistService.GetDailyStatusAsync();
     *         if (!response.IsSuccess || response.Data is null) return;
     *         if (!response.Data.HasCompletedToday) return;
     *         var lastAt = response.Data.LastChecklistAt;
     *         var hora = lastAt.HasValue ? lastAt.Value.ToLocalTime().ToString("HH:mm") : "hoy";
     *         var confirmado = await _dialogService.ShowWarningAsync(
     *             "Ya registraste un checklist hoy",
     *             $"El último fue registrado a las {hora}. ¿Deseas registrar uno adicional?");
     *         if (!confirmado)
     *             await Shell.Current.GoToAsync("..");
     *     }
     *     catch (Exception ex)
     *     {
     *         System.Diagnostics.Debug.WriteLine($"[VerificarChecklist] Error: {ex}");
     *     }
     * }
     */

    private async Task CargarImagenEquipoAsync(EquipmentDto equipment)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(equipment.ImagePathLeft))
            {
                var bytes = await _equipmentService.DownloadImage(equipment.EquipmentId, "left");
                if (bytes?.Length > 0)
                    LeftForkliftImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            if (!string.IsNullOrWhiteSpace(equipment.ImagePathRight))
            {
                var bytes = await _equipmentService.DownloadImage(equipment.EquipmentId, "right");
                if (bytes?.Length > 0)
                    RightForkliftImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
        }
        catch { /* imagen no disponible, se queda la imagen por defecto */ }
    }

    private void OnLeftImageTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image img)
        {
            var point = e.GetPosition(img);
            if (point != null)
                ToggleMark(LeftImageHost, _leftMarks, point.Value);
        }
    }

    private void OnRightImageTapped(object sender, TappedEventArgs e)
    {
        if (sender is Image img)
        {
            var point = e.GetPosition(img);
            if (point != null)
                ToggleMark(RightImageHost, _rightMarks, point.Value);
        }
    }

    private void ToggleMark(AbsoluteLayout host, List<Label> marks, Point point)
    {
        const double tolerance = 20;

        var existing = marks.FirstOrDefault(m =>
        {
            var bounds = AbsoluteLayout.GetLayoutBounds(m);
            return Math.Abs(bounds.X - point.X) <= tolerance &&
                   Math.Abs(bounds.Y - point.Y) <= tolerance;
        });

        if (existing != null)
        {
            host.Children.Remove(existing);
            marks.Remove(existing);
            return;
        }

        var mark = new Label
        {
            Text             = "X",
            FontSize         = 28,
            FontAttributes   = FontAttributes.Bold,
            TextColor        = Colors.Red,
            InputTransparent = true
        };

        AbsoluteLayout.SetLayoutBounds(mark, new Rect(point.X - 10, point.Y - 14, 30, 30));
        AbsoluteLayout.SetLayoutFlags(mark, AbsoluteLayoutFlags.None);

        host.Children.Add(mark);
        marks.Add(mark);
    }

    private void OnClearLeftMarksClicked(object sender, EventArgs e)
    {
        foreach (var mark in _leftMarks.ToList())
            LeftImageHost.Children.Remove(mark);
        _leftMarks.Clear();
    }

    private void OnClearRightMarksClicked(object sender, EventArgs e)
    {
        foreach (var mark in _rightMarks.ToList())
            RightImageHost.Children.Remove(mark);
        _rightMarks.Clear();
    }

    private void OnOptionSelected(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Models.ChecklistQuestion question)
        {
            question.SelectedOption = btn.Text;
            var parent = btn.Parent as Grid;
            if (parent is null) return;
            foreach (var child in parent.Children)
            {
                if (child is Button b)
                    b.BackgroundColor = Colors.LightGray;
            }
            btn.BackgroundColor = Colors.LightGreen;
        }
    }

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await _dialogService.ShowInfoAsync("Cámara", "Este dispositivo no soporta captura de fotos.");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var side = ViewModel.Photos.Count == 0 ? "left" : (ViewModel.Photos.Count == 1 ? "right" : "custom");
            var order = ViewModel.Photos.Count;
            var img = ImageSource.FromStream(() => new MemoryStream(bytes));
            PreviewImage.Source = img;

            ViewModel.Photos.Add(new ChecklistPhotoItem
            {
                Bytes  = bytes,
                Order  = order,
                Side   = side,
                Source = img,
            });
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var vm = ViewModel;

        var pendientes = vm.Sections
            .SelectMany(s => s.Questions)
            .Where(q => string.IsNullOrWhiteSpace(q.SelectedOption))
            .ToList();

        if (pendientes.Any())
        {
            await _dialogService.ShowErrorAsync("Faltan datos", "Debes contestar todas las preguntas del checklist.");
            return;
        }

        if (_equipment is null)
        {
            await _dialogService.ShowErrorAsync("Error", "No se detectó el equipo asignado.");
            return;
        }

        try
        {
            vm.IsSaving = true;

            // Comprimir y subir fotos en serie mostrando progreso
            var photos = new List<ChecklistPhotoDto>();
            for (int i = 0; i < vm.Photos.Count; i++)
            {
                var item = vm.Photos[i];
                vm.StatusSubida = $"Subiendo foto {i + 1} de {vm.Photos.Count}...";

                byte[] bytesFinales;
                try
                {
                    bytesFinales = await ImageCompressor.ComprimirAsync(item.Bytes);
                }
                catch
                {
                    bytesFinales = item.Bytes;
                }

                var uploadResult = await vm.UploadPhotoAsync(bytesFinales, $"foto{i + 1}.jpg", item.Side);
                if (uploadResult.IsSuccess && uploadResult.Data is not null)
                    photos.Add(new ChecklistPhotoDto
                    {
                        RelativePath = uploadResult.Data.RelativePath,
                        Side         = item.Side,
                        Order        = item.Order
                    });
            }

            // Convertir marcas de píxeles a porcentaje (0..1) usando las dimensiones del host
            var defectMarks = new List<ChecklistDefectMarkDto>();
            defectMarks.AddRange(ExtractMarks(_leftMarks,  LeftImageHost,  "left"));
            defectMarks.AddRange(ExtractMarks(_rightMarks, RightImageHost, "right"));

            var answers = vm.Sections
                .SelectMany(s => s.Questions)
                .Select(q => new ChecklistAnswerDto
                {
                    QuestionId   = q.QuestionId,
                    QuestionText = q.Label,
                    AnswerText   = q.SelectedOption ?? string.Empty,
                    IsOk         = q.SelectedOption switch
                    {
                        "Sí" => true,
                        "No" => false,
                        _    => (bool?)null
                    }
                })
                .ToList();

            decimal? horometro = null;
            var horometroText  = vm.Horometro?.Trim();
            if (!string.IsNullOrWhiteSpace(horometroText) &&
                decimal.TryParse(horometroText,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var horoParsed))
                horometro = horoParsed;

            var request = new SubmitChecklistRequest
            {
                EquipmentId   = _equipment.EquipmentId,
                UserName      = vm.Operador?.Trim() ?? string.Empty,
                Turno         = TurnoPicker.SelectedItem?.ToString() ?? string.Empty,
                Horometro     = horometro,
                Observaciones = ObservacionesEditor.Text?.Trim(),
                Answers       = answers,
                DefectMarks   = defectMarks,
                Photos        = photos
            };

            vm.StatusSubida = "Guardando checklist...";
            var (ok, message) = await vm.SubmitAsync(request);

            if (ok)
            {
                // Si era obligatorio, el popup de bloqueo ya no aplica (checklist completado)
                _isMandatory = false;
                await _dialogService.ShowSuccessAsync("Listo", message);
                await Shell.Current.GoToAsync("//dashboard");
            }
            else
            {
                await _dialogService.ShowErrorAsync("Error al guardar", message);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Error", ex.Message);
        }
        finally
        {
            vm.IsSaving     = false;
            vm.StatusSubida = string.Empty;
        }
    }

    // Convierte las posiciones de los Labels de marcas a porcentajes (0..1) respecto al host.
    private static List<ChecklistDefectMarkDto> ExtractMarks(
        List<Label> marks, AbsoluteLayout host, string side)
    {
        var hostW = host.Width;
        var hostH = host.Height;
        if (hostW <= 0 || hostH <= 0)
            return new List<ChecklistDefectMarkDto>();

        return marks.Select(m =>
        {
            var bounds = AbsoluteLayout.GetLayoutBounds(m);
            var tapX = Math.Clamp((bounds.X + 10) / hostW, 0, 1);
            var tapY = Math.Clamp((bounds.Y + 14) / hostH, 0, 1);
            return new ChecklistDefectMarkDto
            {
                Side     = side,
                XPercent = (decimal)tapX,
                YPercent = (decimal)tapY
            };
        }).ToList();
    }
}
