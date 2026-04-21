using FluentValidation;
using LD.Application.Features.EquipmentType.Comands;

namespace LD.Application.Features.EquipmentType.Validators;

public class UpdateEquipmentTypeValidator : AbstractValidator<UpdateEquipmentTypeCommand>
{
    public UpdateEquipmentTypeValidator()
    {
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("El nombre del equipo no debe estar vacio");
    }
}
