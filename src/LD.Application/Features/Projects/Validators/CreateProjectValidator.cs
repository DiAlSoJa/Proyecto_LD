using FluentValidation;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Projects.Comands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Clients.Validators;

public class CreateProjectValidator
    : AbstractValidator<CreateProjectCommand>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IClientRepository _clientRepository;

    public CreateProjectValidator(IWarehouseRepository warehouseRepository, IClientRepository clientRepository)
    {
        _warehouseRepository = warehouseRepository;
        _clientRepository = clientRepository;

        RuleFor(x => x.WarehouseId)
           .NotNull().WithMessage("El almacén es obligatorio.")
           .MustAsync(async (id, cancelation) =>
           {

               return await _warehouseRepository.GetByIdAsync(id ?? 0) != null;
           }).WithMessage("No se encontro el almacen");

        RuleFor(x => x.ClientId)
         .NotNull().WithMessage("El cliente es obligatorio.")
         .MustAsync(async (id, cancelation) =>
         {

             return await _clientRepository.GetByIdAsync(id ?? 0) != null;
         }).WithMessage("No se encontro el cliente");

        //RuleFor(x => x.StorageTypeId)
        //.NotNull().WithMessage("El tipo de almacenamiento es obligatorio.")
        //.MustAsync(async (id, cancelation) =>
        //{

        //    return await _clientRepository.GetByIdAsync(id ?? 0) != null;
        //}).WithMessage("No se encontro el tipo de almacenamiento ");
    }
}
