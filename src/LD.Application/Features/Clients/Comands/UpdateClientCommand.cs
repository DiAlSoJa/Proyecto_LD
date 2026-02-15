using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LD.Application.Features.Clients.Queries;

public class UpdateClientCommand : ClientRequest,  IRequest<Result<string>>
{


}
public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result<string>>
{
    private readonly IRepository<Client> _clientRepository;
    public UpdateClientCommandHandler(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<string>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientRepository.CreateAsync(new Client
            {
               
                ComercialName = request.CommercialName,
                ClientNumber=""
            });
            return result?Result<string>.Success("Cliente creado con exito",""): Result<string>.Failure("Hubo un error al crear el cliente",null);

        }catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el cliente", new ErrorResponse());
        }
    }
}
