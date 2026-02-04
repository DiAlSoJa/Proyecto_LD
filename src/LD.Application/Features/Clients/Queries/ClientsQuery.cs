using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.DTOs.Client;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public class ClientsQuery : IRequest<List<ClientDto>?>
{

}

public class ClientsQueryHandler : IRequestHandler<ClientsQuery, List<ClientDto>?>
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IMapper _mapper;

    public ClientsQueryHandler(IRepository<Client> clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<List<ClientDto>?> Handle(ClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetManyAsync();
        return _mapper.Map<List<ClientDto>>(clients);
    }
}
