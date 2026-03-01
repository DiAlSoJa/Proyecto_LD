using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Client;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Clients.Queries;

public record ClientByIdQuery(int ClientId)
    : IRequest<Result<ClientDto?>>;
public class ClientByIdQueryHandler : IRequestHandler<ClientByIdQuery, Result<ClientDto?>>
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IMapper _mapper;
    public ClientByIdQueryHandler(IRepository<Client> clientRepository,IMapper mapper)
    {
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<Result<ClientDto?>> Handle(ClientByIdQuery request, CancellationToken cancellationToken)
    {
        var clientDb = await _clientRepository.GetByIdAsync(request.ClientId);
        if (clientDb == null) return Result<ClientDto?>.Failure("Cliente no encontrado",new() , 404);
        return Result<ClientDto?>.Success (_mapper.Map<ClientDto>(clientDb),"Cliente obtenido con exito") ;
    }
}
