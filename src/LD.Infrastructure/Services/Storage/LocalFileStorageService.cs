using LD.Application.Common.Interfaces.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace LD.Infrastructure.Services.Storage;

public class LocalFileStorageService : IFileStorageService
{
    private static readonly string BaseDir = @"C:\LD_Security";

/*    private static readonly string BaseDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "LD_Security");*/

    // Reduce dimensions to 80% of original (−20 %)
    private const double ResizeFactor = 0.8;

    public async Task<string?> SaveAsync(byte[]? data, string subfolder, string prefix)
    {
        if (data is null || data.Length == 0)
            return null;

        var folder = Path.Combine(BaseDir, subfolder);
        Directory.CreateDirectory(folder);

        var fileName = $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}.jpg";
        var fullPath = Path.Combine(folder, fileName);

        var resized = await ResizeAsync(data);
        await File.WriteAllBytesAsync(fullPath, resized);

        return fullPath;
    }

    private static async Task<byte[]> ResizeAsync(byte[] data)
    {
        using var image = Image.Load(data);
        var newWidth  = (int)(image.Width  * ResizeFactor);
        var newHeight = (int)(image.Height * ResizeFactor);
        image.Mutate(ctx => ctx.Resize(newWidth, newHeight));

        using var ms = new MemoryStream();
        await image.SaveAsJpegAsync(ms, new JpegEncoder { Quality = 85 });
        return ms.ToArray();
    }
}
