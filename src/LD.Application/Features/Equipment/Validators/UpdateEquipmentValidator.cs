using FluentValidation;
using LD.Application.Features.Equipment.Commands;

namespace LD.Application.Features.Equipment.Validators;

public class UpdateEquipmentValidator : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentValidator()
    {
        RuleFor(x => x.EquipmentId)
            .GreaterThan(0).WithMessage("El equipo seleccionado no es válido.");
        RuleFor(x => x.EquipmentName)
            .NotEmpty().WithMessage("El nombre del equipo es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del equipo no puede superar 100 caracteres.");
        RuleFor(x => x.SerialNumber)
            .NotEmpty().WithMessage("El número de serie es obligatorio.")
            .MaximumLength(100).WithMessage("El número de serie no puede superar 100 caracteres.");
        RuleFor(x => x.EquipmentTypeId)
            .GreaterThan(0).WithMessage("Debes seleccionar un tipo de equipo válido.");
    }
}
