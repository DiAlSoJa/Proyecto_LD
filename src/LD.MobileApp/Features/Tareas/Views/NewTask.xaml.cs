using LD.Client.Services;
using LD.Contracts.Requests;
using Microsoft.Maui.Graphics.Platform;

namespace MauiAppLogin;

public partial class NewTask : ContentPage, IQueryAttributable
{
    private const float TaskPhotoMaxSize = 1920f;
    private const float TaskPhotoQuality = 0.86f;

    private readonly OperationalTaskService _operationalTaskService;
    private readonly string?[] _photoPaths = new string?[4];
    private readonly ImageSource?[] _photos = new ImageSource?[4];

    public string? TextInformation { get; set; }

    public NewTask(OperationalTaskService operationalTaskService)
    {
        InitializeComponent();
        _operationalTaskService = operationalTaskService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("TextInformation", out var value))
            TextInformation = value as string;
    }

    private async void OnAtrasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnSiguienteClicked(object sender, EventArgs e)
    {
        await SaveTaskAsync();
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

            var slot = Array.FindIndex(_photoPaths, string.IsNullOrWhiteSpace);
            if (slot < 0)
            {
                await DisplayAlertAsync("Fotos", "Ya capturaste las 4 fotos permitidas.", "OK");
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

            _photos[slot] = img;
            _photoPaths[slot] = photoPath;
            SetThumbnail(slot, img);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
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
                $"operational-task-{Guid.NewGuid():N}.jpg");

            await using var output = File.Create(outputPath);
            resized.Save(output, ImageFormat.Jpeg, TaskPhotoQuality);

            return outputPath;
        }
        catch
        {
            return photo.FullPath;
        }
    }

    private void SetThumbnail(int slot, ImageSource source)
    {
        switch (slot)
        {
            case 0:
                Thumb1.Source = source;
                break;
            case 1:
                Thumb2.Source = source;
                break;
            case 2:
                Thumb3.Source = source;
                break;
            case 3:
                Thumb4.Source = source;
                break;
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        PreviewImage.Source = null;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        await SaveTaskAsync();
    }

    private async Task SaveTaskAsync()
    {
        if (EstadoPicker.SelectedItem is null)
        {
            await DisplayAlertAsync("Categoria", "Selecciona la categoria.", "OK");
            return;
        }

        if (EstadoPickewr.SelectedItem is null)
        {
            await DisplayAlertAsync("Tarea", "Selecciona la actividad.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlertAsync("Nombre", "Captura el nombre de la tarea.", "OK");
            return;
        }

        try
        {
            var request = new OperationalTaskRequest
            {
                Priority = EstadoPicker.SelectedItem.ToString() ?? string.Empty,
                Activity = EstadoPickewr.SelectedItem.ToString() ?? string.Empty,
                Name = NameEntry.Text.Trim(),
                Description = LicenciaEntry.Text?.Trim()
            };

            request.Photo1Path = await UploadPhotoAsync(_photoPaths[0], 1);
            request.Photo2Path = await UploadPhotoAsync(_photoPaths[1], 2);
            request.Photo3Path = await UploadPhotoAsync(_photoPaths[2], 3);
            request.Photo4Path = await UploadPhotoAsync(_photoPaths[3], 4);

            var response = await _operationalTaskService.CreateTask(request);
            if (!response.IsSuccess)
            {
                await DisplayAlertAsync("Tarea", response.Message ?? "No se pudo guardar la tarea.", "OK");
                return;
            }

            await DisplayAlertAsync("Tarea", "Tarea guardada.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async Task<string?> UploadPhotoAsync(string? photoPath, int photoNumber)
    {
        if (string.IsNullOrWhiteSpace(photoPath) || !File.Exists(photoPath))
            return null;

        var response = await _operationalTaskService.UploadImage(photoPath, photoNumber);
        if (!response.IsSuccess || response.Data is null || string.IsNullOrWhiteSpace(response.Data.RelativePath))
            throw new InvalidOperationException(response.Message ?? $"No se pudo cargar la foto {photoNumber}.");

        return response.Data.RelativePath;
    }
}
