using LD.Contracts.DTOs.OperationalTasks;
using LD.FormsX.Features.Tareas.ViewModels;
using LD.FormsX.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Tareas;

public partial class TasksView : UserControl
{
    private bool _loaded;
    private TasksViewModel ViewModel => (TasksViewModel)DataContext;

    public TasksView(TasksViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded) return;
        _loaded = true;

        await ViewModel.InicializarAsync();
    }

    private void TasksGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (TasksGrid.SelectedItem is not OperationalTaskDto task)
            return;

        var images = BuildImageList(task);
        if (images.Count == 0)
        {
            DialogHelper.ShowInfo("La tarea seleccionada no tiene imagenes cargadas.");
            return;
        }

        var window = new TaskImagesWindow(task, images)
        {
            Owner = Window.GetWindow(this)
        };
        window.ShowDialog();
    }

    private List<TaskImageItem> BuildImageList(OperationalTaskDto task)
    {
        return new[]
        {
            CreateImage("Foto inicial 1", task.Photo1Path),
            CreateImage("Foto inicial 2", task.Photo2Path),
            CreateImage("Foto inicial 3", task.Photo3Path),
            CreateImage("Foto inicial 4", task.Photo4Path),
            CreateImage("Foto final 1", task.ResolvedPhoto1Path),
            CreateImage("Foto final 2", task.ResolvedPhoto2Path),
            CreateImage("Foto final 3", task.ResolvedPhoto3Path),
            CreateImage("Foto final 4", task.ResolvedPhoto4Path)
        }
        .Where(x => x is not null)
        .Select(x => x!)
        .ToList();
    }

    private TaskImageItem? CreateImage(string title, string? relativePath)
    {
        return string.IsNullOrWhiteSpace(relativePath)
            ? null
            : new TaskImageItem(title, ViewModel.GetImageUrl(relativePath));
    }
}
