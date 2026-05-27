using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Checklist;
using System.IO;

namespace LD.FormsX.Features.CheckList.ViewModels;

public partial class ResumenTabViewModel : ObservableObject
{
    private readonly ChecklistService _checklistService;
    private readonly EquipmentService _equipmentService;

    [ObservableProperty]
    private string statusText = "Sin registros para mostrar";

    [ObservableProperty]
    private DateTime? fechaDesde;

    [ObservableProperty]
    private DateTime? fechaHasta;

    // Detalle cargado más recientemente (usado para la galería de fotos)
    public ChecklistDetailDto? DetalleActual { get; private set; }

    public event Action<List<ChecklistSummaryDto>>? OnChecklistsLoaded;
    public event Action<ChecklistDetailDto?>? OnChecklistDetailLoaded;

    public ResumenTabViewModel(ChecklistService checklistService, EquipmentService equipmentService)
    {
        _checklistService = checklistService;
        _equipmentService = equipmentService;
    }

    public async Task BuscarAsync(DateTime? from, DateTime? to)
    {
        try
        {
            StatusText = "Buscando...";
            var filters = new GetChecklistsQueryRequest { From = from, To = to };
            var response = await _checklistService.GetChecklistsAsync(filters);

            if (!response.IsSuccess || response.Data is null)
            {
                StatusText = response.ErrorMessage ?? "No se pudieron cargar los checklists.";
                OnChecklistsLoaded?.Invoke(new List<ChecklistSummaryDto>());
                return;
            }

            StatusText = response.Data.Count == 0
                ? "Sin registros para el rango seleccionado."
                : $"{response.Data.Count} registro(s) encontrado(s).";

            OnChecklistsLoaded?.Invoke(response.Data);
        }
        catch (Exception ex)
        {
            StatusText = ex.Message;
            OnChecklistsLoaded?.Invoke(new List<ChecklistSummaryDto>());
        }
    }

    public async Task CargarDetalleAsync(int checklistId)
    {
        try
        {
            var response = await _checklistService.GetByIdAsync(checklistId);
            DetalleActual = response.IsSuccess ? response.Data : null;
            OnChecklistDetailLoaded?.Invoke(DetalleActual);
        }
        catch
        {
            DetalleActual = null;
            OnChecklistDetailLoaded?.Invoke(null);
        }
    }

    public async Task<byte[]?> DescargarImagenEquipoAsync(int equipmentId, string side)
    {
        if (equipmentId <= 0 || string.IsNullOrWhiteSpace(side))
            return null;

        try
        {
            var bytes = await _equipmentService.DownloadImage(equipmentId, side);
            return bytes.Length == 0 ? null : bytes;
        }
        catch
        {
            return null;
        }
    }

    // Descarga las fotos del detalle actual y las guarda como archivos temporales.
    // Devuelve las rutas de los archivos descargados para que el caller pueda abrirlos.
    public async Task<List<string>> DescargarFotosAsync()
    {
        var rutas = new List<string>();
        if (DetalleActual?.Photos is null || DetalleActual.Photos.Count == 0)
            return rutas;

        var tempDir = Path.GetTempPath();
        foreach (var photo in DetalleActual.Photos.OrderBy(p => p.Order))
        {
            try
            {
                var bytes = await _checklistService.GetPhotoBytesAsync(photo.RelativePath);
                if (bytes.Length == 0) continue;

                var ext  = Path.GetExtension(photo.RelativePath);
                var file = Path.Combine(tempDir, $"checklist_foto_{photo.Order}{ext}");
                await File.WriteAllBytesAsync(file, bytes);
                rutas.Add(file);
            }
            catch { /* foto no disponible, se omite */ }
        }
        return rutas;
    }
}
