using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Checklist;
using MediatR;

namespace LD.Application.Features.Checklist.Queries;

public record GetChecklistByIdQuery(int ChecklistId, string PhotoBaseUrl) : IRequest<Result<ChecklistDetailDto?>>;

public class GetChecklistByIdQueryHandler
    : IRequestHandler<GetChecklistByIdQuery, Result<ChecklistDetailDto?>>
{
    private readonly IChecklistRepository _checklistRepository;

    public GetChecklistByIdQueryHandler(IChecklistRepository checklistRepository)
    {
        _checklistRepository = checklistRepository;
    }

    public async Task<Result<ChecklistDetailDto?>> Handle(
        GetChecklistByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var detail = await _checklistRepository.GetDetailAsync(request.ChecklistId, request.PhotoBaseUrl);
            if (detail is null)
                return Result<ChecklistDetailDto?>.Failure("Checklist no encontrado.", new(), 404);

            return Result<ChecklistDetailDto?>.Success(detail, "Checklist obtenido correctamente.");
        }
        catch (Exception ex)
        {
            return Result<ChecklistDetailDto?>.Failure(
                "Error al obtener el checklist.", new List<string> { ex.Message });
        }
    }
}
