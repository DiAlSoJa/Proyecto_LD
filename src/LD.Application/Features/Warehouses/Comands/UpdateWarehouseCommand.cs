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

namespace LD.Application.Features.Warehouses.Comands;

public class UpdateWarehouseCommand : WarehouseRequest, IRequest<Result<string>>
{

}


public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, Result<string>>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;
    public UpdateWarehouseCommandHandler(IRepository<Warehouse> warehouseRepository,IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(request.WarehouseId.Value);
            if (warehouse is null)
                return Result<string>.Failure("No existe el almacen", new ErrorResponse(), 404);
            _mapper.Map(request, warehouse);

            var result = await _warehouseRepository.UpdateAsync(warehouse);
            return result ? Result<string>.Success("Almacen actualizado con exito", "") : Result<string>.Failure("Hubo un error al actualizar el Almacen", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el Almacen", new ErrorResponse());
        }
    }
}
