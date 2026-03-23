using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Features.Clients.Queries;
using FluentValidation;
using LD.Application.Features.Category.Comands;

namespace LD.Application.Features.Category.Validators;

public class UpdateCategoryValidator
    : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.CategoryName)
             .NotEmpty().WithMessage("El nombre de la categoría no debe estar vacia")
             .MaximumLength(50).WithMessage("El nombre de la categoría no debe exceder de 50 caracteres");

    }
}
