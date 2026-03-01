using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
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

    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public CreateClientCommandHandler(
        IClientRepository clientRepository,
        IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateClientCommand request,CancellationToken cancellationToken)
    {
        try
        {
            var client = _mapper.Map<Client>(request);     
            if (request.FiscalData is not null)
            {
                client.ClientFiscalData = _mapper.Map<ClientFiscalData>(request.FiscalData);
            }

            var created = await _clientRepository.CreateAsync(client);

            if (!created)
                return Result<string>.Failure("No se pudo crear el cliente",new ErrorResponse() );

            return Result<string>.Success( "Cliente creado con éxito",client.ClientId.ToString());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el cliente", new ErrorResponse());
        }
    }
}
