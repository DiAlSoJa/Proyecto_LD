namespace LD.Application.Common.Interfaces.Storage;

public interface IFileStorageService
{
    /// <summary>
    /// Guarda los bytes y devuelve la ruta lógica del archivo.
    /// Devuelve null si data es null o vacio.
    /// </summary>
    Task<string?> SaveAsync(byte[]? data, string subfolder, string prefix, string? extension = null);

    /// <summary>
    /// Guarda una imagen optimizada en JPEG y devuelve la ruta lógica.
    /// Devuelve null si data es null o vacio.
    /// </summary>
    Task<string?> SaveJpegAsync(byte[]? data, string subfolder, string prefix);

    /// <summary>
    /// Abre el archivo indicado para lectura.
    /// Devuelve null si el archivo no existe.
    /// </summary>
    Task<Stream?> OpenReadAsync(string path);

    /// <summary>
    /// Elimina el archivo indicado si existe.
    /// </summary>
    Task<bool> DeleteAsync(string path);
}
