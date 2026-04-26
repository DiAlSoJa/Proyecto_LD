using FluentValidation;
using LD.Application.Features.Checklist.Commands;

namespace LD.Application.Features.Checklist.Validators;

public class SubmitChecklistValidator : AbstractValidator<SubmitChecklistCommand>
{
    public SubmitChecklistValidator()
    {
        RuleFor(x => x.EquipmentId).GreaterThan(0).WithMessage("El EquipmentId debe ser mayor a 0.");
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Answers).NotEmpty().WithMessage("El checklist debe tener al menos una respuesta.");
        RuleFor(x => x.Observaciones).MaximumLength(2000).When(x => x.Observaciones is not null);

        RuleForEach(x => x.DefectMarks).ChildRules(mark =>
        {
            mark.RuleFor(m => m.XPercent).InclusiveBetween(0, 1)
                .WithMessage("XPercent debe estar entre 0 y 1.");
            mark.RuleFor(m => m.YPercent).InclusiveBetween(0, 1)
                .WithMessage("YPercent debe estar entre 0 y 1.");
        });

        RuleForEach(x => x.Answers).ChildRules(a =>
        {
            a.RuleFor(x => x.AnswerText).NotEmpty().MaximumLength(500);
            a.RuleFor(x => x.QuestionText).NotEmpty().MaximumLength(500);
        });
    }
}
