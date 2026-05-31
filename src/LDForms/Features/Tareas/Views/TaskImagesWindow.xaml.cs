using LD.Contracts.DTOs.OperationalTasks;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Tareas;

public partial class TaskImagesWindow : Window
{
    public Func<TaskImageItem, Task<byte[]>>? DownloadImageAsync { get; set; }

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

    private async void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        if (sender is not FrameworkElement { DataContext: TaskImageItem image })
            return;

        if (DownloadImageAsync is null)
            return;

        try
        {
            var bytes = await DownloadImageAsync(image);
            var filePath = await SaveTempImageAsync(image, bytes);

            Process.Start(new ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError($"No se pudo abrir la imagen. {ex.Message}");
        }
    }

    private static async Task<string> SaveTempImageAsync(TaskImageItem image, byte[] bytes)
    {
        var folder = Path.Combine(Path.GetTempPath(), "LD", "Tareas");
        Directory.CreateDirectory(folder);

        var extension = Path.GetExtension(image.RelativePath);
        if (string.IsNullOrWhiteSpace(extension))
            extension = ".jpg";

        var safeTitle = string.Concat(image.Title.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));
        var fileName = $"{safeTitle}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(folder, fileName);

        await File.WriteAllBytesAsync(filePath, bytes);
        return filePath;
    }
}

public sealed record TaskImageItem(string Title, string ImageUrl, string RelativePath, string Group);
