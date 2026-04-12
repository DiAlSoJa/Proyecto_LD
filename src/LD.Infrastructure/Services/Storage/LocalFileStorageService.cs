using LD.Application.Common.Interfaces.Storage;

namespace LD.Infrastructure.Services.Storage;

public class LocalFileStorageService : IFileStorageService
{

    private static readonly string BaseDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "LD_Security");

    public async Task<string?> SaveAsync(byte[]? data, string subfolder, string prefix)
    {
        if (data is null || data.Length == 0)
            return null;

        var folder = Path.Combine(BaseDir, subfolder);
        Directory.CreateDirectory(folder);

        var fileName = $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmssfff}.jpg";
        var fullPath = Path.Combine(folder, fileName);

        await File.WriteAllBytesAsync(fullPath, data);

        return fullPath;
    }
}
