using LD.Application.Common.Interfaces;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public record ClientByIdQuery(int ClientId)
    : IRequest<Client?>;
public class ClientByIdQueryHandler : IRequestHandler<ClientByIdQuery, Client?>
{
    private readonly IRepository<Client> _clientRepository;
    public ClientByIdQueryHandler(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Client?> Handle(ClientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _clientRepository.GetByIdAsync(request.ClientId);
    }
}
