using LD.Client.Services;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Contracts.Requests;
using Microsoft.Maui.Graphics.Platform;

namespace MauiAppLogin;

public partial class TaskResolve : ContentPage, IQueryAttributable
{
    private const float TaskPhotoMaxSize = 1920f;
    private const float TaskPhotoQuality = 0.86f;

    private readonly OperationalTaskService _operationalTaskService;
    private readonly ImageSource?[] _resolutionPhotos = new ImageSource?[4];
    private readonly string?[] _resolutionPhotoPaths = new string?[4];
    private readonly string?[] _existingResolutionPhotoPaths = new string?[4];
    private int _taskId;
    private bool _loaded;

    public TaskResolve(OperationalTaskService operationalTaskService)
    {
        InitializeComponent();
        _operationalTaskService = operationalTaskService;
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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
            return;

        _loaded = true;
        await LoadTaskAsync();
    }

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
            var response = await _operationalTaskService.GetTaskById(_taskId);
            if (!response.IsSuccess || response.Data is null)
            {
                await DisplayAlertAsync("Tarea", response.Message ?? "No se pudo cargar la tarea.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            ApplyTask(response.Data);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private void ApplyTask(OperationalTaskDto task)
    {
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

    private void SetOriginalImage(Image image, string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            image.Source = null;
            image.Opacity = 0.35;
            return;
        }

        image.Opacity = 1;
        image.Source = ImageSource.FromUri(new Uri(_operationalTaskService.GetImageUrl(relativePath)));
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        try
        {
            var request = new CompleteOperationalTaskRequest
            {
                ResolutionObservations = ResolutionObservationsEditor.Text?.Trim(),
                ResolvedPhoto1Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[0], 1) ?? _existingResolutionPhotoPaths[0],
                ResolvedPhoto2Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[1], 2) ?? _existingResolutionPhotoPaths[1],
                ResolvedPhoto3Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[2], 3) ?? _existingResolutionPhotoPaths[2],
                ResolvedPhoto4Path = await UploadResolutionPhotoAsync(_resolutionPhotoPaths[3], 4) ?? _existingResolutionPhotoPaths[3]
            };

            var response = await _operationalTaskService.CompleteTask(_taskId, request);
            if (!response.IsSuccess)
            {
                await DisplayAlertAsync("Tarea", response.Message ?? "No se pudo terminar la tarea.", "OK");
                return;
            }

            await DisplayAlertAsync("Tarea", "Tarea terminada.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

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

        var response = await _operationalTaskService.UploadImage(photoPath, photoNumber);
        if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.RelativePath))
            throw new InvalidOperationException(response.Message ?? $"No se pudo cargar la foto resuelta {photoNumber}.");

        return response.Data.RelativePath;
    }
}
