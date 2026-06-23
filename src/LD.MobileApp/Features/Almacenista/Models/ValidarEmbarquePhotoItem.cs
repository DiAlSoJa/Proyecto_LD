namespace MauiAppLogin.Features.Almacenista.Models;

public class ValidarEmbarquePhotoItem
{
    public string PhotoKey { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsLegacy { get; set; }
    public string Title => IsLegacy ? $"Foto {SortOrder}" : $"Foto {SortOrder}";
}
