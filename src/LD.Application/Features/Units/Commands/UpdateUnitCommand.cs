
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

namespace LD.Application.Features.Units.Comands;

public class UpdateUnitCommand : UnitRequest, IRequest<Result<string>>
{

}


public class UpdateUnitCommandHandler : IRequestHandler<UpdateUnitCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.Units> _unitRepository;
    private readonly IMapper _mapper;
    public UpdateUnitCommandHandler(IRepository<LD.Domain.Entities.Units> unitRepository, IMapper mapper)
    {
        _unitRepository = unitRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var unit = await _unitRepository.GetByIdAsync(request.UnitId.Value);
            if (unit is null)
                return Result<string>.Failure("No existe la unidad", new List<string> { "Hubo un error al obtener la unidad" }, 404);
            _mapper.Map(request, unit);

            var result = await _unitRepository.UpdateAsync(unit);
            return result ? Result<string>.Success("Unidad actualizada con exito", "") : Result<string>.Failure("Hubo un error al actualizar la unidad", new List<string> { "No se encontro la unidad" });

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar la unidad", new List<string> { ex.Message });
        }
    }
}

