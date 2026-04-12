using MauiAppLogin.ViewModels;
using Microsoft.Maui.Layouts;

namespace MauiAppLogin;

public partial class ForkliftChecklistPage : ContentPage
{
    private readonly List<Label> _leftMarks = new();
    private readonly List<Label> _rightMarks = new();
    private ImageSource? _foto1;
    private ImageSource? _foto2;

    public ForkliftChecklistPage()
    {
        InitializeComponent();
        FechaPicker.Date = DateTime.Today;
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

            // cambiar color visual
            var parent = btn.Parent as Grid;

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

            // Preview grande
            PreviewImage.Source = img;

            // Guardar en slots de miniaturas (2 fotos)
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
        // Limpia el preview (o navega atrás, tú decides)
        PreviewImage.Source = null;
        // Si quieres regresar:
        // await Navigation.PopAsync();
    }
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ForkliftChecklistViewModel;
        if (vm == null)
            return;

        var pendientes = vm.Sections
            .SelectMany(s => s.Questions)
            .Where(q => string.IsNullOrWhiteSpace(q.SelectedOption))
            .ToList();

        if (pendientes.Any())
        {
            await DisplayAlertAsync("Faltan datos", "Debes contestar todas las preguntas del checklist.", "OK");
            return;
        }

        string detalle = string.Join("\n",
            vm.Sections.SelectMany(s => s.Questions)
                       .Select(q => $"{q.Label}: {q.SelectedOption}"));

        await DisplayAlertAsync(
            "Checklist guardado",
            $"Operador: {OperadorEntry.Text}\n" +
            $"Equipo: {EquipoEntry.Text}\n" +
            $"Turno: {TurnoPicker.SelectedItem}\n" +
            $"Fecha: {FechaPicker.Date:dd/MM/yyyy}\n" +
            $"Horómetro: {HorometroEntry.Text}\n\n" +
            detalle,
            "OK");
    }
}