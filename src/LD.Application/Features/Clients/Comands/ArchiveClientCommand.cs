using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LD.Application.Features.Clients.Queries;

public class ArchiveClientCommand : IRequest<string>
{
    public string? ClientNumber { get; set; }
    public string? ComercialName { get; set; }

}
public class ArchiveClientCommandHandler : IRequestHandler<ArchiveClientCommand, string>
{
    private readonly IRepository<Client> _clientRepository;
    public ArchiveClientCommandHandler(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<string> Handle(ArchiveClientCommand request, CancellationToken cancellationToken)
    {

        var result = true ;
        //var result = await _clientRepository.ArchiveAsync() ;
        return result?"Cliente eliminado con exit":"Hubo un error al eliminar el cliente";
    }
}
