using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Warehouses.Comands;

namespace LD.Application.Features.Clients.Validators;

public class CreateWarehouseValidator
    : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseValidator()
    {
       RuleFor(x=>x.WarehouseName)
            .NotEmpty().WithMessage("El nombre del almacen no debe estar vacio")
            .MaximumLength(100).WithMessage("El nombre del almacen no debe exceder de 100 caracteres");

        RuleFor(x => x.Address)
          .NotEmpty().WithMessage("La direccion del almacen no debe estar vacia")
          .MaximumLength(200).WithMessage("La direccion no debe de exceder los 200 caracteres");

        RuleFor(x => x.Neighborhood)
         .NotEmpty().WithMessage("La colonia no puede estar vacia")
         .MaximumLength(50).WithMessage("La colonia no debe de exceder los 50 caracteres");

        RuleFor(x => x.City)
        .NotEmpty().WithMessage("La ciudad no puede estar vacia")
        .MaximumLength(100).WithMessage("La ciudad no debe de exceder los 100 caracteres");

        RuleFor(x => x.ZipCode)
         .NotEmpty().WithMessage("El código postal es obligatorio.")
         .Matches(@"^\d{5}$").WithMessage("El código postal debe tener 5 dígitos.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("La capacidad debe ser mayor que cero.")
            .PrecisionScale(10, 4, true).WithMessage("La capacidad no debe exceder 10 dígitos en total, con hasta 4 decimales.");


    }
}
