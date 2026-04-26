using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Checklist;

namespace LD.FormsX.Features.CheckList.ViewModels;

public partial class ResumenBateriasTabViewModel : ObservableObject
{
    private readonly ChecklistService _checklistService;
    private readonly EquipmentTypeService _equipmentTypeService;

    // IDs de tipos de equipo con IsBattery == true, resueltos dinámicamente al primer uso.
    private List<int> _batteryTypeIds = new();
    private bool _typeIdsResolved;

    [ObservableProperty]
    private string statusText = "Sin registros para mostrar";

    [ObservableProperty]
    private DateTime? fechaDesde;

    [ObservableProperty]
    private DateTime? fechaHasta;

    public event Action<List<ChecklistSummaryDto>>? OnChecklistsLoaded;
    public event Action<ChecklistDetailDto?>? OnChecklistDetailLoaded;

    public ResumenBateriasTabViewModel(
        ChecklistService checklistService,
        EquipmentTypeService equipmentTypeService)
    {
        _checklistService    = checklistService;
        _equipmentTypeService = equipmentTypeService;
    }

    public async Task BuscarAsync(DateTime? from, DateTime? to)
    {
        try
        {
            StatusText = "Buscando...";

            await ResolveTypesIfNeededAsync();

            if (_batteryTypeIds.Count == 0)
            {
                StatusText = "No se encontraron tipos de equipo de batería.";
                OnChecklistsLoaded?.Invoke(new List<ChecklistSummaryDto>());
                return;
            }

            // Traer checklists de todos los tipos batería y unir resultados
            var all = new List<ChecklistSummaryDto>();
            foreach (var typeId in _batteryTypeIds)
            {
                var filters = new GetChecklistsQueryRequest
                {
                    From            = from,
                    To              = to,
                    EquipmentTypeId = typeId
                };
                var response = await _checklistService.GetChecklistsAsync(filters);
                if (response.IsSuccess && response.Data is not null)
                    all.AddRange(response.Data);
            }

            all = all.OrderByDescending(x => x.CreatedAt).ToList();
            StatusText = all.Count == 0
                ? "Sin registros para el rango seleccionado."
                : $"{all.Count} registro(s) encontrado(s).";

            OnChecklistsLoaded?.Invoke(all);
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
            OnChecklistDetailLoaded?.Invoke(response.IsSuccess ? response.Data : null);
        }
        catch
        {
            OnChecklistDetailLoaded?.Invoke(null);
        }
    }

    private async Task ResolveTypesIfNeededAsync()
    {
        if (_typeIdsResolved) return;
        _typeIdsResolved = true;

        var response = await _equipmentTypeService.GetEquipmentTypes();
        if (response.IsSuccess && response.Data is not null)
            _batteryTypeIds = response.Data
                .Where(t => t.IsBattery)
                .Select(t => t.EquipmentTypeId)
                .ToList();
    }
}
