using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LD.Application.Features.Clients.Queries;

public class ArchiveClientCommand : IRequest<Result<string>>
{
    public int ClientId { get; set; }

}
public class ArchiveClientCommandHandler : IRequestHandler<ArchiveClientCommand, Result<string>>
{
    private readonly IArchiveRepository<Client> _clientRepository;
    public ArchiveClientCommandHandler(IArchiveRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<string>> Handle(ArchiveClientCommand request, CancellationToken cancellationToken)
    {

        var result = await _clientRepository.ArchiveAsync(request.ClientId,false);
        return result? Result<string>.Success("cliente eliminado con exito","cliente eliminado con exito") : Result<string>.Failure("cliente eliminado con exito",new());
    }
}
