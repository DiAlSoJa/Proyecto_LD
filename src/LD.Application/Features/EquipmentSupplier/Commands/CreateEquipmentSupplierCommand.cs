using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LD.Application.Features.EquipmentSupplier.Commands;

public class CreateEquipmentSupplierCommand : EquipmentSupplierRequest, IRequest<Result<string>>
{
}

public class CreateEquipmentSupplierCommandHandler : IRequestHandler<CreateEquipmentSupplierCommand, Result<string>>
{
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _equipmentSupplierRepository;
    private readonly IMapper _mapper;

    public CreateEquipmentSupplierCommandHandler(
        IRepository<LD.Domain.Entities.EquipmentSupplier> equipmentSupplierRepository,
        IMapper mapper)
    {
        _equipmentSupplierRepository = equipmentSupplierRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateEquipmentSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var supplierName = request.EquipmentSupplierName.Trim();
            if (string.IsNullOrWhiteSpace(supplierName))
                return Result<string>.Failure("Captura el nombre del proveedor", new());

            var existingSuppliers = await _equipmentSupplierRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentSupplier>();
            if (existingSuppliers.Any(x => string.Equals(x.EquipmentSupplierName, supplierName, StringComparison.OrdinalIgnoreCase)))
                return Result<string>.Failure("Ya existe un proveedor con ese nombre", new());

            request.EquipmentSupplierName = supplierName;

            var result = await _equipmentSupplierRepository.CreateAsync(_mapper.Map<LD.Domain.Entities.EquipmentSupplier>(request));

            return result
                ? Result<string>.Success("Proveedor creado con exito", string.Empty)
                : Result<string>.Failure("Hubo un error al crear el proveedor", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el proveedor", new List<string> { ex.Message });
        }
    }
}
