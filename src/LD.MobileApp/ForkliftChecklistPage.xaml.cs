using MauiAppLogin.ViewModels;
using Microsoft.Maui.Layouts;

namespace MauiAppLogin;

public partial class ForkliftChecklistPage : ContentPage
{
    private readonly List<Label> _leftMarks = new();
    private readonly List<Label> _rightMarks = new();

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
            await DisplayAlert("Faltan datos", "Debes contestar todas las preguntas del checklist.", "OK");
            return;
        }

        string detalle = string.Join("\n",
            vm.Sections.SelectMany(s => s.Questions)
                       .Select(q => $"{q.Label}: {q.SelectedOption}"));

        await DisplayAlert(
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