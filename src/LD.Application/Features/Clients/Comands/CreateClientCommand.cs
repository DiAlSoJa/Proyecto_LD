using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LD.Application.Features.Clients.Queries;

public class CreateClientCommand : IRequest<string>
{
    public string? ClientNumber { get; set; }
    public string? ComercialName { get; set; }

}
public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, string>
{
    private readonly IRepository<Client> _clientRepository;
    public CreateClientCommandHandler(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<string> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {

        var result = await _clientRepository.CreateAsync(new Client
        {
            ClientNumber = request.ClientNumber,
            ComercialName = request.ComercialName
        });
        return result?"Cliente creado con exito":"Hubo un error al crear el cliente";
    }
}
