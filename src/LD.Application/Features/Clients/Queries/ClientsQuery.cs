using LD.Domain.Entities;
using LD.Application.Common.Interfaces;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public class ClientsQuery : IRequest<List<Client>?>
{

}

public class ClientsQueryHandler : IRequestHandler<ClientsQuery, List<Client>?>
{
    private readonly IRepository<Client> _clientRepository;
    public ClientsQueryHandler(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<List<Client>?> Handle(ClientsQuery request, CancellationToken cancellationToken)
    {
        return await _clientRepository.GetManyAsync();
    }
}
