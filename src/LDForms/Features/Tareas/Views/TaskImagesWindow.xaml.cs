using LD.Contracts.DTOs.OperationalTasks;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Tareas;

public partial class TaskImagesWindow : Window
{
    public TaskImagesWindow(OperationalTaskDto task, IReadOnlyList<TaskImageItem> images)
    {
        InitializeComponent();

        TitleText.Text = $"Tarea #{task.OperationalTaskId} - {task.Name}";
        SubtitleText.Text = $"{task.Activity} | {task.WarehouseName ?? "Sin almacen"}";
        PriorityText.Text = string.IsNullOrWhiteSpace(task.Priority) ? "Sin prioridad" : task.Priority;
        CreatedText.Text = task.CreatedAt.ToString("dd/MM/yyyy HH:mm");
        StatusText.Text = task.Completed ? "Finalizado" : "Pendiente";
        CompletedByText.Text = string.IsNullOrWhiteSpace(task.CompletedBy) ? "Sin usuario" : task.CompletedBy;
        DescriptionText.Text = string.IsNullOrWhiteSpace(task.Description) ? "Sin descripcion" : task.Description;
        ObservationsText.Text = string.IsNullOrWhiteSpace(task.ResolutionObservations) ? "Sin observaciones" : task.ResolutionObservations;
        CompletedAtText.Text = task.CompletedAt.HasValue
            ? task.CompletedAt.Value.ToString("dd/MM/yyyy HH:mm")
            : "Sin finalizar";

        var initialImages = images.Where(x => x.Group == "Inicial").ToList();
        var finalImages = images.Where(x => x.Group == "Final").ToList();

        InitialImagesList.ItemsSource = initialImages;
        FinalImagesList.ItemsSource = finalImages;
        NoInitialImagesText.Visibility = initialImages.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        NoFinalImagesText.Visibility = finalImages.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private void BtnCerrar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }

    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        if (sender is not FrameworkElement { DataContext: TaskImageItem image })
            return;

        try
        {
            Process.Start(new ProcessStartInfo(image.ImageUrl)
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"No se pudo abrir la imagen. {ex.Message}");
        }
    }
}

public sealed record TaskImageItem(string Title, string ImageUrl, string Group);
