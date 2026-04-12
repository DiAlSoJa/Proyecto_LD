namespace LD.Application.Common.Interfaces.Storage;

public interface IFileStorageService
{
    /// <summary>
    /// Guarda los bytes en disco y devuelve la ruta del archivo.
    /// Devuelve null si data es null o vacío.
    /// </summary>
    Task<string?> SaveAsync(byte[]? data, string subfolder, string prefix);
}
