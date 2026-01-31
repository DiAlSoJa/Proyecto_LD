using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public class CreateClientCommand : IRequest<bool>
{

}
public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, bool>
{
    private readonly IRepository<Client> _clientRepository;
    public CreateClientCommandHandler(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<bool> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {

        return true;
    }
}
