using LD.Contracts.Checklist;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IChecklistRepository
{
    Task<int> CreateChecklistAsync(Checklist checklist);
    Task<List<ChecklistSummaryDto>> GetSummariesAsync(DateTime? from, DateTime? to, int? equipmentTypeId, int? equipmentId);
    Task<ChecklistDetailDto?> GetDetailAsync(int checklistId, string photoBaseUrl);

    // Verifica si el usuario completó un checklist del equipo indicado desde 'since'.
    // Devuelve (true, timestamp) si existe, (false, null) si no.
    Task<(bool hasCompleted, DateTime? lastChecklistAt)> GetDailyStatusAsync(
        string userId, int equipmentId, DateTime since);
}
