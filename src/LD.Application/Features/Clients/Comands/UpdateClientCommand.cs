using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
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
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(
        IClientRepository clientRepository,
        IMapper mapper)
    {
        _mapper = mapper;
        _clientRepository = clientRepository;
    }

    public async Task<Result<string>> Handle(UpdateClientCommand request,CancellationToken cancellationToken)
    {
        try
        {
            // 1️⃣ Buscar cliente
            var client = await _clientRepository.GetByIdAsync(request.ClientId);
            if (client is null)
                return Result<string>.Failure( "No existe el cliente", new List<string> { "No existe el cliente" }, 404);

            // 2️⃣ Mapear datos básicos
            _mapper.Map(request, client);

            // 3️⃣ FiscalData (IMPORTANTE)
            if (request.FiscalData != null)
            {
                if (client.ClientFiscalData == null)
                {
                    // crear
                    client.ClientFiscalData = _mapper.Map<ClientFiscalData>(request.FiscalData);
                }
                else
                {
                    // actualizar
                    _mapper.Map(request.FiscalData,client.ClientFiscalData);
                }
            }

            // 4️⃣ Guardar
            var updated = await _clientRepository.UpdateAsync(client);

            if (!updated)
                return Result<string>.Failure("Error al actualizar", new List<string> { "Hubo un erros al actualizar el cliente" });


            return Result<string>.Success("Cliente actualizado",client.ClientId.ToString());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Error inesperado",new());
        }
    }
}