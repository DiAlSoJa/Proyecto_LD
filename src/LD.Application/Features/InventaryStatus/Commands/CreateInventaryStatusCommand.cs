using AutoMapper;
using LD.Application.Common.Exceptions;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.InventaryStatus.Comands;

public class CreateInventaryStatusCommand : InventaryStatusRequest, IRequest<Result<string>>
{
}

public class CreateStatusCommandHandler : IRequestHandler<CreateInventaryStatusCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.InventaryStatus> _statusRepository;
    private readonly IMapper _mapper;

    public CreateStatusCommandHandler(IRepository<LD.Domain.Entities.InventaryStatus> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateInventaryStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _statusRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.InventaryStatus>(request));
            return result
                ? Result<string>.Success("Estatus creado con exito", "")
                : Result<string>.Failure("Hubo un error al crear el estatus", new());
        }
        catch (Exception ex)
        {
            var detail = DatabaseExceptionMessageHelper.GetUserMessage(ex, "estatus", "status, cliente y proyecto");
            return Result<string>.Failure("Hubo un error al crear el estatus", new List<string> { detail });
        }
    }
}
