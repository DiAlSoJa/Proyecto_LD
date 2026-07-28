using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.DTOs.WarehouseTasks;
using LD.Contracts.Requests;
using MauiAppLogin.Controls;
using Microsoft.Maui.Graphics.Platform;

namespace MauiAppLogin;

public partial class TaskResolve : ContentPage, IQueryAttributable
{
    private const float TaskPhotoMaxSize = 1920f;
    private const float TaskPhotoQuality = 0.86f;

    private readonly OperationalTaskService _operationalTaskService;
    private readonly WarehouseTaskService _warehouseTaskService;
    private readonly IDialogService _dialogService;
    private readonly ImageSource?[] _resolutionPhotos = new ImageSource?[4];
    private readonly string?[] _resolutionPhotoPaths = new string?[4];
    private readonly string?[] _existingResolutionPhotoPaths = new string?[4];
    private string _taskSource = "warehouse";
    private int _taskId;
    private bool _loaded;
    private bool _isCompleting;

    public TaskResolve(
        OperationalTaskService operationalTaskService,
        WarehouseTaskService warehouseTaskService,
        IDialogService dialogService)
    {
        InitializeComponent();
        _operationalTaskService = operationalTaskService;
        _warehouseTaskService = warehouseTaskService;
        _dialogService = dialogService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TaskId", out var taskIdValue))
        {
            _taskId = taskIdValue switch
            {
                int value => value,
                string value when int.TryParse(value, out var parsed) => parsed,
                _ => 0
            };

            _loaded = false;
        }

        if (query.TryGetValue("TaskSource", out var sourceValue) && sourceValue is string source && !string.IsNullOrWhiteSpace(source))
            _taskSource = source;

        Title = IsOperationalTask
            ? "Resolucion de Tareas Operativas"
            : "Resolucion de Tareas de Almacen";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
            return;

        _loaded = true;
        await LoadTaskAsync();
    }

    private bool IsOperationalTask
        => string.Equals(_taskSource, "operational", StringComparison.OrdinalIgnoreCase);

    private async Task LoadTaskAsync()
    {
        if (_taskId <= 0)
        {
            await DisplayAlertAsync("Tarea", "No se recibio la tarea seleccionada.", "OK");
            await Shell.Current.GoToAsync("..");
            return;
        }

        try
        {
            if (IsOperationalTask)
            {
                var response = await _operationalTaskService.GetTaskById(_taskId);
                if (!response.IsSuccess || response.Data is null)
                {
                    await DisplayAlertAsync("Tarea", response.Message ?? "No se pudo cargar la tarea.", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                ApplyTask(response.Data);
                return;
            }

            var warehouseResponse = await _warehouseTaskService.GetTaskByIdAsync(_taskId);
            if (!warehouseResponse.IsSuccess || warehouseResponse.Data is null)
            {
                await DisplayAlertAsync("Tarea", warehouseResponse.Message ?? "No se pudo cargar la tarea.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            ApplyTask(warehouseResponse.Data);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void ApplyTask(OperationalTaskDto task)
    {
        WarehouseLabel.Text = $"Almacen: {(string.IsNullOrWhiteSpace(task.WarehouseName) ? "Sin almacen" : task.WarehouseName)}";
        CategoryLabel.Text = $"Categoria: {task.Priority}";
        ActivityLabel.Text = $"Tarea: {task.Activity}";
        NameLabel.Text = $"Nombre: {task.Name}";
        DescriptionLabel.Text = $"Descripcion: {task.Description}";
        ResolutionObservationsEditor.Text = task.ResolutionObservations;

        SetOriginalImage(OriginalThumb1, task.Photo1Path);
        SetOriginalImage(OriginalThumb2, task.Photo2Path);
        SetOriginalImage(OriginalThumb3, task.Photo3Path);
        SetOriginalImage(OriginalThumb4, task.Photo4Path);

        SetOriginalImage(ResolutionThumb1, task.ResolvedPhoto1Path);
        SetOriginalImage(ResolutionThumb2, task.ResolvedPhoto2Path);
        SetOriginalImage(ResolutionThumb3, task.ResolvedPhoto3Path);
        SetOriginalImage(ResolutionThumb4, task.ResolvedPhoto4Path);

        _existingResolutionPhotoPaths[0] = task.ResolvedPhoto1Path;
        _existingResolutionPhotoPaths[1] = task.ResolvedPhoto2Path;
        _existingResolutionPhotoPaths[2] = task.ResolvedPhoto3Path;
        _existingResolutionPhotoPaths[3] = task.ResolvedPhoto4Path;
    }

    private void ApplyTask(WarehouseTaskDto task)
    {
        WarehouseLabel.Text = $"Almacen: {(string.IsNullOrWhiteSpace(task.WarehouseName) ? "Sin almacen" : task.WarehouseName)}";
        CategoryLabel.Text = $"Categoria: {task.Priority}";
        ActivityLabel.Text = $"Tarea: {task.Activity}";
        NameLabel.Text = $"Nombre: {task.Name}";
        DescriptionLabel.Text = $"Descripcion: {task.Description}";
        ResolutionObservationsEditor.Text = task.ResolutionObservations;

        SetOriginalImage(OriginalThumb1, task.Photo1Path);
        SetOriginalImage(OriginalThumb2, task.Photo2Path);
        SetOriginalImage(OriginalThumb3, task.Photo3Path);
        SetOriginalImage(OriginalThumb4, task.Photo4Path);

        SetOriginalImage(ResolutionThumb1, task.ResolvedPhoto1Path);
        SetOriginalImage(ResolutionThumb2, task.ResolvedPhoto2Path);
        SetOriginalImage(ResolutionThumb3, task.ResolvedPhoto3Path);
        SetOriginalImage(ResolutionThumb4, task.ResolvedPhoto4Path);

        _existingResolutionPhotoPaths[0] = task.ResolvedPhoto1Path;
        _existingResolutionPhotoPaths[1] = task.ResolvedPhoto2Path;
        _existingResolutionPhotoPaths[2] = task.ResolvedPhoto3Path;
        _existingResolutionPhotoPaths[3] = task.ResolvedPhoto4Path;
    }

    private string GetImageUrl(string relativePath)
        => IsOperationalTask
            ? _operationalTaskService.GetImageUrl(relativePath)
            : _warehouseTaskService.GetImageUrl(relativePath);

    private void SetOriginalImage(Image image, string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            image.Source = null;
            image.Opacity = 0.35;
            return;
        }

        image.Opacity = 1;
        image.Source = ImageSource.FromUri(new Uri(GetImageUrl(relativePath)));
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        if (_isCompleting)
            return;

        var confirmMessage = IsOperationalTask
            ? "Se van a subir las fotos de resolucion y cerrar la tarea operativa. Deseas continuar?"
            : "Se van a subir las fotos de resolucion y cerrar la tarea de almacen. Deseas continuar?";

        var confirmed = await _dialogService.ShowWarningAsync("Terminar tarea", confirmMessage);
        if (!confirmed)
            return;

        _isCompleting = true;
        _dialogService.ShowBlocking(
            IsOperationalTask ? "Terminando tarea operativa" : "Terminando tarea",
            "Subiendo fotos y guardando la resolucion...");

        string? errorMessage = null;
        var completed = false;

        try
        {
            if (IsOperationalTask)
            {
                var request = await BuildOperationalCompletionRequestAsync();

                var response = await _operationalTaskService.CompleteTask(_taskId, request);
                if (!response.IsSuccess)
                {
                    errorMessage = response.Message ?? "No se pudo terminar la tarea.";
                }
                else
                {
                    completed = true;
                }
            }
            else
            {
                var warehouseRequest = await BuildWarehouseCompletionRequestAsync();

                var warehouseResponse = await _warehouseTaskService.CompleteTaskAsync(_taskId, warehouseRequest);
                if (!warehouseResponse.IsSuccess)
                {
                    errorMessage = warehouseResponse.Message ?? "No se pudo terminar la tarea.";
                }
                else
                {
                    completed = true;
                }
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        finally
        {
            _dialogService.HideBlocking();
            _isCompleting = false;
        }

        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            await DisplayAlertAsync("Tarea", errorMessage, "OK");
            return;
        }

        if (completed)
        {
            await DisplayAlertAsync("Tarea", "Tarea terminada.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task<CompleteOperationalTaskRequest> BuildOperationalCompletionRequestAsync()
    {
        return new CompleteOperationalTaskRequest
        {
            CompletedBy = GetCompletedByDisplayName(),
            ResolutionObservations = ResolutionObservationsEditor.Text?.Trim(),
            ResolvedPhoto1Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[0], 1) ?? _existingResolutionPhotoPaths[0],
            ResolvedPhoto2Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[1], 2) ?? _existingResolutionPhotoPaths[1],
            ResolvedPhoto3Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[2], 3) ?? _existingResolutionPhotoPaths[2],
            ResolvedPhoto4Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[3], 4) ?? _existingResolutionPhotoPaths[3]
        };
    }

    private async Task<CompleteWarehouseTaskRequest> BuildWarehouseCompletionRequestAsync()
    {
        return new CompleteWarehouseTaskRequest
        {
            CompletedByName = GetCompletedByDisplayName(),
            ResolutionObservations = ResolutionObservationsEditor.Text?.Trim(),
            ResolvedPhoto1Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[0], 1) ?? _existingResolutionPhotoPaths[0],
            ResolvedPhoto2Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[1], 2) ?? _existingResolutionPhotoPaths[1],
            ResolvedPhoto3Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[2], 3) ?? _existingResolutionPhotoPaths[2],
            ResolvedPhoto4Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[3], 4) ?? _existingResolutionPhotoPaths[3]
        };
    }

    private static string GetCompletedByDisplayName()
        => (UserData.UserName ?? UserData.Name ?? UserData.Id ?? string.Empty).Trim();

    private async void OnCapturarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlertAsync("Camara", "Este dispositivo no soporta captura de fotos.", "OK");
                return;
            }

            var slot = Array.FindIndex(_resolutionPhotoPaths, string.IsNullOrWhiteSpace);
            if (slot < 0)
            {
                await DisplayAlertAsync("Fotos", "Ya capturaste las 4 fotos de resolucion.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo == null)
                return;

            var photoPath = await CompressPhotoAsync(photo);
            await using var stream = File.OpenRead(photoPath);
            var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            mem.Position = 0;

            var img = ImageSource.FromStream(() => new MemoryStream(mem.ToArray()));
            PreviewImage.Source = img;

            _resolutionPhotos[slot] = img;
            _resolutionPhotoPaths[slot] = photoPath;
            SetResolutionThumbnail(slot, img);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private static async Task<string> CompressPhotoAsync(FileResult photo)
    {
        try
        {
            await using var input = await photo.OpenReadAsync();
            using var image = PlatformImage.FromStream(input);
            if (image is null)
                return photo.FullPath;

            using var resized = image.Downsize(TaskPhotoMaxSize);
            var outputPath = Path.Combine(
                FileSystem.CacheDirectory,
                $"operational-task-resolved-{Guid.NewGuid():N}.jpg");

            await using var output = File.Create(outputPath);
            resized.Save(output, ImageFormat.Jpeg, TaskPhotoQuality);

            return outputPath;
        }
        catch
        {
            return photo.FullPath;
        }
    }

    private void SetResolutionThumbnail(int slot, ImageSource source)
    {
        switch (slot)
        {
            case 0:
                ResolutionThumb1.Source = source;
                break;
            case 1:
                ResolutionThumb2.Source = source;
                break;
            case 2:
                ResolutionThumb3.Source = source;
                break;
            case 3:
                ResolutionThumb4.Source = source;
                break;
        }
    }

    private async Task<string?> UploadResolutionPhotoAsync(string? photoPath, int photoNumber)
    {
        if (string.IsNullOrWhiteSpace(photoPath) || !File.Exists(photoPath))
            return null;

        if (IsOperationalTask)
        {
            var operationalResponse = await _operationalTaskService.UploadImage(photoPath, photoNumber);
            if (!operationalResponse.IsSuccess || operationalResponse.Data is null || string.IsNullOrWhiteSpace(operationalResponse.Data.RelativePath))
                throw new InvalidOperationException(operationalResponse.Message ?? $"No se pudo cargar la foto resuelta {photoNumber}.");

            return operationalResponse.Data.RelativePath;
        }

        var warehouseResponse = await _warehouseTaskService.UploadImageAsync(photoPath, photoNumber);
        if (!warehouseResponse.IsSuccess || warehouseResponse.Data is null || string.IsNullOrWhiteSpace(warehouseResponse.Data.RelativePath))
            throw new InvalidOperationException(warehouseResponse.Message ?? $"No se pudo cargar la foto resuelta {photoNumber}.");

        return warehouseResponse.Data.RelativePath;
    }
}
