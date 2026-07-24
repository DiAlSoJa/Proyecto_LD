using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.KittingFolioCapture;
using MediatR;

namespace LD.Application.Features.KittingFolioCaptures.Queries;

public class KittingFolioCaptureQuery : IRequest<Result<List<KittingFolioCaptureDto>>>
{
    public int? ClientId { get; set; }
    public int? ProjectId { get; set; }
}

public class KittingFolioCaptureQueryHandler : IRequestHandler<KittingFolioCaptureQuery, Result<List<KittingFolioCaptureDto>>>
{
    private readonly IKittingFolioCaptureRepository _repository;
    private readonly IMapper _mapper;

    public KittingFolioCaptureQueryHandler(IKittingFolioCaptureRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<KittingFolioCaptureDto>>> Handle(KittingFolioCaptureQuery request, CancellationToken cancellationToken)
    {
        var captures = await _repository.GetManyWithRelationsAsync(request.ClientId, request.ProjectId);
        var dto = _mapper.Map<List<KittingFolioCaptureDto>>(captures)
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.KittingFolioCaptureId)
            .ToList();

        return Result<List<KittingFolioCaptureDto>>.Success(dto, "Folios de Kitting obtenidos correctamente");
    }
}
