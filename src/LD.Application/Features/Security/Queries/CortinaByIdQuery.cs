using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public record CortinaByIdQuery(int CortinaId) : IRequest<Result<CortinaRequest?>>;

public class CortinaByIdQueryHandler : IRequestHandler<CortinaByIdQuery, Result<CortinaRequest?>>
{
    private readonly IRepository<Cortina> _cortinaRepository;
    private readonly IMapper _mapper;

    public CortinaByIdQueryHandler(IRepository<Cortina> cortinaRepository, IMapper mapper)
    {
        _cortinaRepository = cortinaRepository;
        _mapper = mapper;
    }

    public async Task<Result<CortinaRequest?>> Handle(CortinaByIdQuery request, CancellationToken cancellationToken)
    {
        var cortinaDb = await _cortinaRepository.GetByIdAsync(request.CortinaId);
        if (cortinaDb is null)
            return Result<CortinaRequest?>.Failure("Cortina no encontrada", new(), 404);

        return Result<CortinaRequest?>.Success(_mapper.Map<CortinaRequest>(cortinaDb), "Cortina obtenida con exito");
    }
}
