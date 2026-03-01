using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Contracts.Requests.Client;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public record ClientByIdQuery(int ClientId)
    : IRequest<Result<ClientRequest?>>;
public class ClientByIdQueryHandler : IRequestHandler<ClientByIdQuery, Result<ClientRequest?>>
{
   private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;
    public ClientByIdQueryHandler(IClientRepository clientRepository,IMapper mapper)
    {
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<Result<ClientRequest?>> Handle(ClientByIdQuery request, CancellationToken cancellationToken)
    {
        var clientDb = await _clientRepository.GetByIdAsync(request.ClientId);
        if (clientDb == null) return Result<ClientRequest?>.Failure("Cliente no encontrado",new() , 404);

        var clientResponse = _mapper.Map<ClientRequest>(clientDb);

        if(clientDb.ClientFiscalData is not null) 
            clientResponse.FiscalData = _mapper.Map<ClientFiscalDataRequest>(clientDb.ClientFiscalData);

        return Result<ClientRequest?>.Success (clientResponse, "Cliente obtenido con exito") ;
    }
}
