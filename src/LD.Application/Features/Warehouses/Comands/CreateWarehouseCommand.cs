using LD.Application.Common.Interfaces;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Models;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Warehouses.Comands;

public class CreateWarehouseCommand : IRequest<string>
{

}


public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, string>
{
    private readonly IRepository<Warehouse> _warehouseRepository;
    public CreateWarehouseCommandHandler(IRepository<Warehouse> warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<string> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var result = await _warehouseRepository.CreateAsync(new Warehouse
        {
   
        });
        return result ? "Cliente creado con exito" : "Hubo un error al crear el cliente";
    }
}
