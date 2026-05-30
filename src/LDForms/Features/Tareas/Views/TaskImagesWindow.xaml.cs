using LD.Contracts.DTOs.OperationalTasks;
using System.Collections.Generic;
using System.Windows;

namespace LD.FormsX.Views.Tareas;

public partial class TaskImagesWindow : Window
{
    public TaskImagesWindow(OperationalTaskDto task, IReadOnlyList<TaskImageItem> images)
    {
        InitializeComponent();

        TitleText.Text = $"Tarea #{task.OperationalTaskId} - {task.Name}";
        SubtitleText.Text = $"{task.Activity} | {task.WarehouseName ?? "Sin almacen"}";
        ImagesList.ItemsSource = images;
    }
}

public sealed record TaskImageItem(string Title, string ImageUrl);
