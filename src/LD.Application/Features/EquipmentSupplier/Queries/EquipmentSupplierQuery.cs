using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.EquipmentSupplier;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LD.Application.Features.EquipmentSupplier.Queries;

public class EquipmentSupplierQuery : IRequest<Result<List<EquipmentSupplierDto>?>>
{
}

public class EquipmentSupplierQueryHandler : IRequestHandler<EquipmentSupplierQuery, Result<List<EquipmentSupplierDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.EquipmentSupplier> _equipmentSupplierRepository;
    private readonly IMapper _mapper;

    public EquipmentSupplierQueryHandler(
        IRepository<LD.Domain.Entities.EquipmentSupplier> equipmentSupplierRepository,
        IMapper mapper)
    {
        _equipmentSupplierRepository = equipmentSupplierRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<EquipmentSupplierDto>?>> Handle(
        EquipmentSupplierQuery request,
        CancellationToken cancellationToken)
    {
        var suppliers = await _equipmentSupplierRepository.GetManyAsync() ?? new List<LD.Domain.Entities.EquipmentSupplier>();
        var supplierDtos = _mapper.Map<List<EquipmentSupplierDto>>(suppliers.OrderBy(x => x.EquipmentSupplierName).ToList());
        return Result<List<EquipmentSupplierDto>?>.Success(supplierDtos, "Proveedores obtenidos correctamente");
    }
}
