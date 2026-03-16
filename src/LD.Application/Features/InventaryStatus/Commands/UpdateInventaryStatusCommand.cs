
using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.InventaryStatus.Comands;

public class UpdateInventaryStatusCommand : InventaryStatusRequest, IRequest<Result<string>>
{

}


public class UpdateStatusCommandHandler : IRequestHandler<UpdateInventaryStatusCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.InventaryStatus> _statusRepository;
    private readonly IMapper _mapper;
    public UpdateStatusCommandHandler(IRepository<LD.Domain.Entities.InventaryStatus> statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateInventaryStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var statusX = await _statusRepository.GetByIdAsync(request.InventoryStatusId);
            if (statusX is null)
                return Result<string>.Failure("No existe el estatus", new List<string> { "Hubo un error al obtener el estatus" }, 404);
            _mapper.Map(request, statusX);

            var result = await _statusRepository.UpdateAsync(statusX);
            return result ? Result<string>.Success("Estatus actualizado con exito", "") : Result<string>.Failure("Hubo un error al actualizar el estatus", new List<string> { "No se encontro el estatus" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el estatus", new List<string> { ex.Message });
        }
    }
}

