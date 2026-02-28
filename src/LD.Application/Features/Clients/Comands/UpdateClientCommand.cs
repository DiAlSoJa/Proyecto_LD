using AutoMapper;
using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Results;
using LD.Contracts.Requests.Client;
using LD.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LD.Application.Features.Clients.Queries;



public class UpdateClientCommand : ClientRequest, IRequest<Result<string>>
{
    public int ClientId { get; set; }
}

public class UpdateClientCommandHandler
    : IRequestHandler<UpdateClientCommand, Result<string>>
{
    private readonly IRepository<Client> _clientRepository;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(IRepository<Client> clientRepository, IMapper mapper)
    {
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<Result<string>> Handle( UpdateClientCommand request, CancellationToken cancellationToken)
    {
        // 1️⃣ Buscar cliente
        var client = await _clientRepository.GetByIdAsync(request.ClientId);

        if (client is null)
            return Result<string>.Failure("No existe el cliente a actualizar",new(),404);

        // 2️⃣ Mapear cambios
        _mapper.Map(request, client);


        // 3️⃣ Guardar
        var updated = await _clientRepository.UpdateAsync(client);

        if (!updated)
        {
            return Result<string>.Failure("Hubo un error al intentar actualizar el cliente",new());
        }

        return Result<string>.Success("Cliente actualizado correctamente","se actualizo el cliente correctamente");
    }
}
