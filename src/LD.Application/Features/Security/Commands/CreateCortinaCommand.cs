using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class CreateCortinaCommand : CortinaRequest, IRequest<Result<string>>
{
}

public class CreateCortinaCommandHandler : IRequestHandler<CreateCortinaCommand, Result<string>>
{
    private readonly IRepository<Cortina> _cortinaRepository;
    private readonly IMapper _mapper;

    public CreateCortinaCommandHandler(IRepository<Cortina> cortinaRepository, IMapper mapper)
    {
        _cortinaRepository = cortinaRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateCortinaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _cortinaRepository.CreateAsync(_mapper.Map<Cortina>(request));
            return result
                ? Result<string>.Success("Cortina creada con exito", "")
                : Result<string>.Failure("Hubo un error al crear la cortina", new List<string>());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear la cortina", new List<string> { ex.Message });
        }
    }
}
