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
       
    }
}
