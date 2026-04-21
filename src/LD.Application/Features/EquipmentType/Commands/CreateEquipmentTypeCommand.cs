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

public class CreateEquipmentTypeCommand : EquipmentTypeRequest, IRequest<Result<string>>
{
}

public class CreateEquipmentTypeCommandHandler : IRequestHandler<CreateEquipmentTypeCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.EquipmentType> _equipmentTypeRepository;
    private readonly IMapper _mapper;

    public CreateEquipmentTypeCommandHandler(IRepository<LD.Domain.Entities.EquipmentType> equipmentTypeRepository, IMapper mapper)
    {
        _equipmentTypeRepository = equipmentTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateEquipmentTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _equipmentTypeRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.EquipmentType>(request));
            return result
                ? Result<string>.Success("Tipo de equipo creado con exito", "")
                : Result<string>.Failure("Hubo un error al crear el tipo de equipo", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el tipo de equipo", new List<string> { ex.Message });
        }
    }
}
