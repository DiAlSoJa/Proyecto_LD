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

public class CreateWarehouseCommand :WarehouseRequest, IRequest<Result<string>>
{

}


public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Result<string>>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    private readonly IMapper _mapper;
    public CreateWarehouseCommandHandler(IRepository<Warehouse> warehouseRepository,IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _warehouseRepository.CreateAsync(_mapper.Map<Warehouse>(request));
            return result ? Result<string>.Success("Almacen creado con exito", "") : Result<string>.Failure("Hubo un error al crear el Almacen", new());

        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el Almacen", new ErrorResponse());
        }
    }
}
