using LD.Contracts.DTOs.OperationalTasks;
using LD.FormsX.Features.Tareas.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Tareas;

public partial class TasksView : UserControl
{
    private bool _loaded;
    private readonly DataGridColumnFilterManager _tasksGridManager;
    private TasksViewModel ViewModel => (TasksViewModel)DataContext;

    public TasksView(TasksViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        DataGridFilterStyler.Apply(TasksGrid);
        _tasksGridManager = new DataGridColumnFilterManager(TasksGrid);
        _tasksGridManager.ApplyTo(ViewModel.TasksView);
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

        var window = new TaskImagesWindow(task, images);
        LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
            window,
            LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
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
        if (string.IsNullOrWhiteSpace(relativePath))
            return null;

        var imageUrl = ViewModel.GetImageUrl(relativePath);
        return string.IsNullOrWhiteSpace(imageUrl)
            ? null
            : new TaskImageItem(title, imageUrl);
    }

    private async void ExcelButton_Click(object sender, RoutedEventArgs e)
    {
        var task = ViewModel.SelectedTask;
        if (task is null)
        {
            DialogHelper.ShowWarning("Selecciona una tarea para exportar.", "Excel");
            return;
        }

        var saveDialog = new SaveFileDialog
        {
            Title = "Exportar tarea a Excel",
            Filter = "Archivo de Excel (*.xlsx)|*.xlsx",
            FileName = $"Tarea_{task.OperationalTaskId}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
            AddExtension = true,
            DefaultExt = ".xlsx"
        };

        if (saveDialog.ShowDialog(Window.GetWindow(this)) != true)
            return;

        var previousLoadingMessage = ViewModel.LoadingMessage;

        try
        {
            ViewModel.IsLoading = true;
            ViewModel.LoadingMessage = "Generando Excel...";

            await OperationalTaskExcelExporter.ExportAsync(task, saveDialog.FileName, ViewModel.GetImageBytesAsync);

            ToastHelper.ShowSuccess("Excel exportado correctamente.", "Excel");

            Process.Start(new ProcessStartInfo
            {
                FileName = saveDialog.FileName,
                UseShellExecute = true
            });
        }
        catch (IOException ex)
        {
            DialogHelper.ShowError($"No se pudo guardar el archivo. Verifica que no este abierto en Excel.\n{ex.Message}", "Excel");
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message, "Excel");
        }
        finally
        {
            ViewModel.IsLoading = false;
            ViewModel.LoadingMessage = previousLoadingMessage;
        }
    }
}

