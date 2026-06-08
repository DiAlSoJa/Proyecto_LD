using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LD.Application.Common.Interfaces.Storage;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace LD.Infrastructure.Services.Storage;

public class AzureBlobFileStorageService : IFileStorageService
{
    private const double ResizeFactor = 0.8;
    private readonly BlobContainerClient _containerClient;
    private readonly string _containerName;

    public AzureBlobFileStorageService(IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("BlobStorage")
            ?? configuration["BlobStorage:ConnectionString"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Missing BlobStorage connection string. Configure ConnectionStrings:BlobStorage in appsettings or Azure App Service.");
        }

        _containerName = configuration["BlobStorage:ContainerName"] ?? "uploads";
        _containerClient = new BlobContainerClient(connectionString, _containerName);
    }

    public async Task<string?> SaveAsync(byte[]? data, string subfolder, string prefix, string? extension = null)
    {
        if (data is null || data.Length == 0)
            return null;

        await EnsureContainerAsync();

        var safeSubfolder = NormalizeSegment(subfolder);
        var safeExtension = NormalizeExtension(extension);
        var blobName = BuildBlobName(safeSubfolder, prefix, safeExtension);
        var blobClient = _containerClient.GetBlobClient(blobName);

        await using var ms = new MemoryStream(data);
        await blobClient.UploadAsync(ms, overwrite: true);
        await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders
        {
            ContentType = GetContentType(safeExtension)
        });

        return $"{_containerName}/{blobName}";
    }

    public async Task<string?> SaveJpegAsync(byte[]? data, string subfolder, string prefix)
    {
        if (data is null || data.Length == 0)
            return null;

        await EnsureContainerAsync();

        var safeSubfolder = NormalizeSegment(subfolder);
        var blobName = BuildBlobName(safeSubfolder, prefix, ".jpg");
        var blobClient = _containerClient.GetBlobClient(blobName);

        var resized = await ResizeAsync(data);
        await using var ms = new MemoryStream(resized);
        await blobClient.UploadAsync(ms, overwrite: true);
        await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders
        {
            ContentType = "image/jpeg"
        });

        return $"{_containerName}/{blobName}";
    }

    public async Task<Stream?> OpenReadAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var blobName = NormalizeBlobName(path);
        if (string.IsNullOrWhiteSpace(blobName))
            return null;

        await EnsureContainerAsync();

        var blobClient = _containerClient.GetBlobClient(blobName);
        if (!await blobClient.ExistsAsync())
            return null;

        var download = await blobClient.DownloadContentAsync();
        return new MemoryStream(download.Value.Content.ToArray(), writable: false);
    }

    private async Task EnsureContainerAsync()
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
    }

    private static string NormalizeSegment(string value)
        => string.Join(
            "/",
            (value ?? string.Empty)
                .Replace('\\', '/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(segment => segment.Trim()));

    private static string NormalizeExtension(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return ".jpg";

        var normalized = extension.Trim();
        if (!normalized.StartsWith('.'))
            normalized = "." + normalized;

        return normalized.ToLowerInvariant();
    }

    private static string BuildBlobName(string subfolder, string prefix, string extension)
    {
        var safePrefix = string.Join("_",
            (prefix ?? string.Empty)
                .Replace('\\', '_')
                .Replace('/', '_')
                .Split('_', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        var fileName = $"{safePrefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";

        if (string.IsNullOrWhiteSpace(subfolder))
            return fileName;

        return $"{subfolder}/{fileName}";
    }

    private static string NormalizeBlobName(string path)
    {
        var normalized = path.Replace('\\', '/').Trim('/');
        if (normalized.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            normalized.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        return normalized;
    }

    private static string GetContentType(string extension)
        => extension.ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".jpeg" => "image/jpeg",
            ".jpg" => "image/jpeg",
            _ => "application/octet-stream"
        };

    private static async Task<byte[]> ResizeAsync(byte[] data)
    {
        using var image = Image.Load(data);
        var newWidth = (int)(image.Width * ResizeFactor);
        var newHeight = (int)(image.Height * ResizeFactor);
        image.Mutate(ctx => ctx.Resize(newWidth, newHeight));

        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 85 });
        return ms.ToArray();
    }
}
