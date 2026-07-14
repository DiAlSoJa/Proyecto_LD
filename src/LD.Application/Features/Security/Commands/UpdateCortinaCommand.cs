using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class UpdateCortinaCommand : CortinaRequest, IRequest<Result<string>>
{
}

public class UpdateCortinaCommandHandler : IRequestHandler<UpdateCortinaCommand, Result<string>>
{
    private readonly IRepository<Cortina> _cortinaRepository;
    private readonly IMapper _mapper;

    public UpdateCortinaCommandHandler(IRepository<Cortina> cortinaRepository, IMapper mapper)
    {
        _cortinaRepository = cortinaRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateCortinaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.CortinaId.HasValue)
                return Result<string>.Failure("La cortina no es valida", new List<string> { "Falta el identificador" }, 400);

            var cortina = await _cortinaRepository.GetByIdAsync(request.CortinaId.Value);
            if (cortina is null)
                return Result<string>.Failure("No existe la cortina", new List<string> { "Hubo un error al obtener la cortina" }, 404);

            _mapper.Map(request, cortina);
            var result = await _cortinaRepository.UpdateAsync(cortina);

            return result
                ? Result<string>.Success("Cortina actualizada con exito", "")
                : Result<string>.Failure("Hubo un error al actualizar la cortina", new List<string> { "No se encontro la cortina" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar la cortina", new List<string> { ex.Message });
        }
    }
}
