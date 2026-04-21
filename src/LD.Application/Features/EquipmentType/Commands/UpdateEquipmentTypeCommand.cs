using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.EquipmentType.Comands;

public class UpdateEquipmentTypeCommand : EquipmentTypeRequest, IRequest<Result<string>>
{
}

public class UpdateEquipmentTypeCommandHandler : IRequestHandler<UpdateEquipmentTypeCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IMapper _mapper;

    public UpdateEquipmentTypeCommandHandler(IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository, IMapper mapper)
    {
        _equipmentTypeRepository = equipmentTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateEquipmentTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var equipmentType = await _equipmentTypeRepository.GetByIdAsync(request.EquipmentTypeId);
            if (equipmentType is null)
                return Result<string>.Failure("No existe el tipo de equipo", new List<string> { "Hubo un error al obtener el tipo de equipo" }, 404);

            _mapper.Map(request, equipmentType);

            var result = await _equipmentTypeRepository.UpdateAsync(equipmentType);
            return result
                ? Result<string>.Success("Tipo de equipo actualizado con exito", "")
                : Result<string>.Failure("Hubo un error al actualizar el tipo de equipo", new List<string> { "No se encontro el tipo de equipo" });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el tipo de equipo", new List<string> { ex.Message });
        }
    }
}
