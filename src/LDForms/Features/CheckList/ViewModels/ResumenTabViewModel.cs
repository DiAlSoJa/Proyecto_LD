using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Checklist;

namespace LD.FormsX.Features.CheckList.ViewModels;

public partial class ResumenTabViewModel : ObservableObject
{
    private readonly ChecklistService _checklistService;

    [ObservableProperty]
    private string statusText = "Sin registros para mostrar";

    [ObservableProperty]
    private DateTime? fechaDesde;

    [ObservableProperty]
    private DateTime? fechaHasta;

    public event Action<List<ChecklistSummaryDto>>? OnChecklistsLoaded;
    public event Action<ChecklistDetailDto?>? OnChecklistDetailLoaded;

    public ResumenTabViewModel(ChecklistService checklistService)
    {
        _checklistService = checklistService;
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
            OnChecklistDetailLoaded?.Invoke(response.IsSuccess ? response.Data : null);
        }
        catch
        {
            OnChecklistDetailLoaded?.Invoke(null);
        }
    }
}
