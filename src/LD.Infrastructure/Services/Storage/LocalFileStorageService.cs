using LD.Application.Common.Interfaces.Storage;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace LD.Infrastructure.Services.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private const double ResizeFactor = 0.8;
    private readonly string _rootPath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _rootPath =
            configuration["FileStorage:LocalRootPath"]
            ?? Path.Combine(AppContext.BaseDirectory, "local-storage");
    }

    public async Task<string?> SaveAsync(byte[]? data, string subfolder, string prefix, string? extension = null)
    {
        if (data is null || data.Length == 0)
            return null;

        var safeSubfolder = NormalizeSegment(subfolder);
        var safeExtension = NormalizeExtension(extension);
        var relativePath = BuildRelativePath(safeSubfolder, prefix, safeExtension);
        var fullPath = GetFullPath(relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        await File.WriteAllBytesAsync(fullPath, data);

        return relativePath;
    }

    public async Task<string?> SaveJpegAsync(byte[]? data, string subfolder, string prefix)
    {
        if (data is null || data.Length == 0)
            return null;

        var safeSubfolder = NormalizeSegment(subfolder);
        var relativePath = BuildRelativePath(safeSubfolder, prefix, ".jpg");
        var fullPath = GetFullPath(relativePath);

        var resized = await ResizeAsync(data);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        await File.WriteAllBytesAsync(fullPath, resized);

        return relativePath;
    }

    public async Task<Stream?> OpenReadAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var fullPath = GetFullPath(path);
        if (!File.Exists(fullPath))
            return null;

        return File.OpenRead(fullPath);
    }

    private string GetFullPath(string relativePath)
    {
        var normalized = NormalizeBlobPath(relativePath);
        var fullPath = Path.GetFullPath(Path.Combine(_rootPath, normalized));
        var rootFullPath = Path.GetFullPath(_rootPath);

        if (!fullPath.StartsWith(rootFullPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(fullPath, rootFullPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Ruta de almacenamiento local inválida.");
        }

        return fullPath;
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

    private static string BuildRelativePath(string subfolder, string prefix, string extension)
    {
        var safePrefix = string.Join("_",
            (prefix ?? string.Empty)
                .Replace('\\', '_')
                .Replace('/', '_')
                .Split('_', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        var fileName = $"{safePrefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";

        if (string.IsNullOrWhiteSpace(subfolder))
            return $"uploads/{fileName}";

        return $"uploads/{subfolder}/{fileName}";
    }

    private static string NormalizeBlobPath(string path)
    {
        var normalized = path.Replace('\\', '/').Trim('/');
        if (normalized.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            normalized.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        return normalized;
    }

    private static async Task<byte[]> ResizeAsync(byte[] data)
    {
        using var image = Image.Load(data);
        var newWidth = Math.Max(1, (int)(image.Width * ResizeFactor));
        var newHeight = Math.Max(1, (int)(image.Height * ResizeFactor));
        image.Mutate(ctx => ctx.Resize(newWidth, newHeight));

        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 85 });
        return ms.ToArray();
    }
}
