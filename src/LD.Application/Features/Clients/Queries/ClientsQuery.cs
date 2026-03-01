using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public class ClientsQuery : IRequest<Result<List< ClientDto>?>>
{

}

public class ClientsQueryHandler : IRequestHandler<ClientsQuery, Result<List<ClientDto>?>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientsQueryHandler(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ClientDto>?>> Handle(ClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetManyAsync();
        var clientDtos = _mapper.Map<List<ClientDto>>(clients);
        return Result<List<ClientDto>?>.Success(clientDtos, "Clientes obtenidos correctamente");
    }
}
