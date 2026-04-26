using LD.Contracts.Checklist;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IChecklistRepository
{
    Task<int> CreateChecklistAsync(Checklist checklist);
    Task<List<ChecklistSummaryDto>> GetSummariesAsync(DateTime? from, DateTime? to, int? equipmentTypeId, int? equipmentId);
    Task<ChecklistDetailDto?> GetDetailAsync(int checklistId, string photoBaseUrl);
}
