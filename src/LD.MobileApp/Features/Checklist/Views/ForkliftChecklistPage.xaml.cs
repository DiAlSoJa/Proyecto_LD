using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Equipment;
using MauiAppLogin.Models;
using MauiAppLogin.ViewModels;
using Microsoft.Maui.Layouts;

namespace MauiAppLogin;

[QueryProperty(nameof(Equipment), "Equipment")]
public partial class ForkliftChecklistPage : ContentPage
{
    private readonly List<Label> _leftMarks = new();
    private readonly List<Label> _rightMarks = new();
    private readonly EquipmentService _equipmentService;
    private ImageSource? _foto1;
    private ImageSource? _foto2;

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
        BindingContext = viewModel;
        _equipmentService = equipmentService;
        FechaPicker.Date = DateTime.Today;
    }

    private async Task InicializarConEquipoAsync(EquipmentDto equipment)
    {
        await ViewModel.InicializarAsync(equipment);

        // Cargar imagen base del equipo desde el servidor
        await CargarImagenEquipoAsync(equipment);

        // Precompletar nombre del equipo
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
            Text = "X",
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Red,
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
            mem.Position = 0;

            var img = ImageSource.FromStream(() => new MemoryStream(mem.ToArray()));
            PreviewImage.Source = img;

            if (_foto1 == null)
            {
                _foto1 = img;
                Thumb1.Source = _foto1;
            }
            else
            {
                _foto2 = img;
                Thumb2.Source = _foto2;
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
        var vm = BindingContext as ForkliftChecklistViewModel;
        if (vm == null) return;

        var pendientes = vm.Sections
            .SelectMany(s => s.Questions)
            .Where(q => string.IsNullOrWhiteSpace(q.SelectedOption))
            .ToList();

        if (pendientes.Any())
        {
            await DisplayAlertAsync("Faltan datos", "Debes contestar todas las preguntas del checklist.", "OK");
            return;
        }

        // Pendiente: implementar envío al endpoint de checklists cuando exista.
        string detalle = string.Join("\n",
            vm.Sections.SelectMany(s => s.Questions)
                       .Select(q => $"{q.Label}: {q.SelectedOption}"));

        await DisplayAlertAsync(
            "Checklist guardado (simulado)",
            $"Operador: {OperadorEntry.Text}\n" +
            $"Equipo: {EquipoEntry.Text}\n" +
            $"Turno: {TurnoPicker.SelectedItem}\n" +
            $"Fecha: {FechaPicker.Date:dd/MM/yyyy}\n" +
            $"Horómetro: {HorometroEntry.Text}\n\n" +
            detalle,
            "OK");
    }
}
