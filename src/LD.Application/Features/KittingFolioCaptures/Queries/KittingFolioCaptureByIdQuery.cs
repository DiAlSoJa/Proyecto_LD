using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.KittingFolioCapture;
using MediatR;

namespace LD.Application.Features.KittingFolioCaptures.Queries;

public class KittingFolioCaptureByIdQuery : IRequest<Result<KittingFolioCaptureDto?>>
{
    public int KittingFolioCaptureId { get; set; }
}

public class KittingFolioCaptureByIdQueryHandler : IRequestHandler<KittingFolioCaptureByIdQuery, Result<KittingFolioCaptureDto?>>
{
    private readonly IKittingFolioCaptureRepository _repository;
    private readonly IMapper _mapper;

    public KittingFolioCaptureByIdQueryHandler(IKittingFolioCaptureRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<KittingFolioCaptureDto?>> Handle(KittingFolioCaptureByIdQuery request, CancellationToken cancellationToken)
    {
        var capture = await _repository.GetByIdWithRelationsAsync(request.KittingFolioCaptureId);
        if (capture is null)
        {
            return Result<KittingFolioCaptureDto?>.Failure(
                "No existe el folio capturado",
                new List<string> { "No existe el folio capturado" },
                404);
        }

        return Result<KittingFolioCaptureDto?>.Success(_mapper.Map<KittingFolioCaptureDto>(capture), "Folio capturado obtenido correctamente");
    }
}
