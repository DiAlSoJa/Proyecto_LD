using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Requests.Client;
using LD.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LD.Application.Features.Clients.Queries;

public class CreateClientCommand :ClientRequest,  IRequest<Result<string>>
{


}
public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<string>>
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IMapper _mapper;

    public CreateClientCommandHandler(IRepository<Client> clientRepository,IMapper mapper)
    {
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<Result<string>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientRepository.CreateAsync(_mapper.Map<Client>(request));
            return result?Result<string>.Success("Cliente creado con exito",""): Result<string>.Failure("Hubo un error al crear el cliente",new());

        }catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el cliente", new ErrorResponse());
        }
    }
}
