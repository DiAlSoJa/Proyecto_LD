using FluentValidation;
using LD.Application.Features.Equipment.Commands;

namespace LD.Application.Features.Equipment.Validators;

public class UpdateEquipmentValidator : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentValidator()
    {
        RuleFor(x => x.EquipmentId).GreaterThan(0);
        RuleFor(x => x.EquipmentName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SerialNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EquipmentTypeId).GreaterThan(0);
    }
}
