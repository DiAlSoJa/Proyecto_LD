using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Checklist;
using LD.Contracts.Equipment;
using MauiAppLogin.Models;
using MauiAppLogin.ViewModels;
using Microsoft.Maui.Layouts;

namespace MauiAppLogin;

[QueryProperty(nameof(Equipment), "Equipment")]
public partial class ForkliftChecklistPage : ContentPage
{
    private readonly List<Label> _leftMarks  = new();
    private readonly List<Label> _rightMarks = new();
    private readonly EquipmentService _equipmentService;

    // Bytes de las fotos capturadas en memoria (para subir sin escribir a disco)
    private byte[]? _foto1Bytes;
    private byte[]? _foto2Bytes;

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

    private ForkliftChecklistViewModel ViewModel => (ForkliftChecklistViewModel)BindingContext;

    public ForkliftChecklistPage(ForkliftChecklistViewModel viewModel, EquipmentService equipmentService)
    {
        InitializeComponent();
        BindingContext    = viewModel;
        _equipmentService = equipmentService;
        FechaPicker.Date  = DateTime.Today;
    }

    private async Task InicializarConEquipoAsync(EquipmentDto equipment)
    {
        await ViewModel.InicializarAsync(equipment);
        await CargarImagenEquipoAsync(equipment);
        EquipoEntry.Text = equipment.NoEquipo;
    }

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
                await DisplayAlertAsync("Cámara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null) return;

            await using var stream = await photo.OpenReadAsync();
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();

            var img = ImageSource.FromStream(() => new MemoryStream(bytes));
            PreviewImage.Source = img;

            if (_foto1Bytes == null)
            {
                _foto1Bytes  = bytes;
                Thumb1.Source = img;
            }
            else
            {
                _foto2Bytes  = bytes;
                Thumb2.Source = img;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var vm = ViewModel;

        // Validar que todas las preguntas estén contestadas
        var pendientes = vm.Sections
            .SelectMany(s => s.Questions)
            .Where(q => string.IsNullOrWhiteSpace(q.SelectedOption))
            .ToList();

        if (pendientes.Any())
        {
            await DisplayAlertAsync("Faltan datos", "Debes contestar todas las preguntas del checklist.", "OK");
            return;
        }

        if (_equipment is null)
        {
            await DisplayAlertAsync("Error", "No se detectó el equipo asignado.", "OK");
            return;
        }

        try
        {
            // Subir fotos antes de enviar el checklist
            var photos = new List<ChecklistPhotoDto>();
            if (_foto1Bytes is { Length: > 0 })
            {
                var uploadResult = await vm.UploadPhotoAsync(_foto1Bytes, "foto1.jpg", "left");
                if (uploadResult.IsSuccess && uploadResult.Data is not null)
                    photos.Add(new ChecklistPhotoDto
                    {
                        RelativePath = uploadResult.Data.RelativePath,
                        Side         = "left",
                        Order        = 1
                    });
            }

            if (_foto2Bytes is { Length: > 0 })
            {
                var uploadResult = await vm.UploadPhotoAsync(_foto2Bytes, "foto2.jpg", "right");
                if (uploadResult.IsSuccess && uploadResult.Data is not null)
                    photos.Add(new ChecklistPhotoDto
                    {
                        RelativePath = uploadResult.Data.RelativePath,
                        Side         = "right",
                        Order        = 2
                    });
            }

            // Convertir marcas de píxeles a porcentaje (0..1) usando las dimensiones del host
            var defectMarks = new List<ChecklistDefectMarkDto>();
            defectMarks.AddRange(ExtractMarks(_leftMarks,  LeftImageHost,  "left"));
            defectMarks.AddRange(ExtractMarks(_rightMarks, RightImageHost, "right"));

            // Construir respuestas del checklist
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

            var request = new SubmitChecklistRequest
            {
                EquipmentId  = _equipment.EquipmentId,
                UserName     = OperadorEntry.Text?.Trim() ?? string.Empty,
                Turno        = TurnoPicker.SelectedItem?.ToString() ?? string.Empty,
                Observaciones = ObservacionesEditor.Text?.Trim(),
                Answers      = answers,
                DefectMarks  = defectMarks,
                Photos       = photos
            };

            var (ok, message) = await vm.SubmitAsync(request);

            if (ok)
            {
                await DisplayAlertAsync("Listo", message, "OK");
                await Shell.Current.GoToAsync("//dashboard");
            }
            else
            {
                await DisplayAlertAsync("Error al guardar", message, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
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
            // El Label se posiciona con offset -10 / -14; sumamos de vuelta para obtener el punto de toque.
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
