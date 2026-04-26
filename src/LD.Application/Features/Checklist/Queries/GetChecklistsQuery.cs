using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Checklist;
using MediatR;

namespace LD.Application.Features.Checklist.Queries;

public class GetChecklistsQuery : GetChecklistsQueryRequest, IRequest<Result<List<ChecklistSummaryDto>>>
{
}

public class GetChecklistsQueryHandler
    : IRequestHandler<GetChecklistsQuery, Result<List<ChecklistSummaryDto>>>
{
    private readonly IChecklistRepository _checklistRepository;

    public GetChecklistsQueryHandler(IChecklistRepository checklistRepository)
    {
        _checklistRepository = checklistRepository;
    }

    public async Task<Result<List<ChecklistSummaryDto>>> Handle(
        GetChecklistsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = await _checklistRepository.GetSummariesAsync(
                request.From, request.To, request.EquipmentTypeId, request.EquipmentId);

            return Result<List<ChecklistSummaryDto>>.Success(data, "Checklists obtenidos correctamente.");
        }
        catch (Exception ex)
        {
            return Result<List<ChecklistSummaryDto>>.Failure(
                "Error al obtener los checklists.", new List<string> { ex.Message });
        }
    }
}
