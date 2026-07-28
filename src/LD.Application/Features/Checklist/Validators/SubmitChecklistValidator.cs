using FluentValidation;
using LD.Application.Features.Checklist.Commands;

namespace LD.Application.Features.Checklist.Validators;

public class SubmitChecklistValidator : AbstractValidator<SubmitChecklistCommand>
{
    public SubmitChecklistValidator()
    {
        RuleFor(x => x.EquipmentId)
            .GreaterThan(0).WithMessage("Debes seleccionar un equipo válido.");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El usuario es obligatorio.")
            .MaximumLength(256).WithMessage("El nombre del usuario no puede superar 256 caracteres.");
        RuleFor(x => x.Answers).NotEmpty().WithMessage("El checklist debe tener al menos una respuesta.");
        RuleFor(x => x.Observaciones)
            .MaximumLength(2000).WithMessage("Las observaciones no pueden superar 2000 caracteres.")
            .When(x => x.Observaciones is not null);

        RuleForEach(x => x.DefectMarks).ChildRules(mark =>
        {
            mark.RuleFor(m => m.XPercent).InclusiveBetween(0, 1)
                .WithMessage("La posición horizontal de la marca debe estar dentro de la imagen.");
            mark.RuleFor(m => m.YPercent).InclusiveBetween(0, 1)
                .WithMessage("La posición vertical de la marca debe estar dentro de la imagen.");
        });

        RuleForEach(x => x.Answers).ChildRules(a =>
        {
            a.RuleFor(x => x.AnswerText)
                .NotEmpty().WithMessage("Debes responder todas las preguntas del checklist.")
                .MaximumLength(500).WithMessage("Una respuesta del checklist supera 500 caracteres.");
            a.RuleFor(x => x.QuestionText)
                .NotEmpty().WithMessage("Una pregunta del checklist no contiene texto.")
                .MaximumLength(500).WithMessage("Una pregunta del checklist supera 500 caracteres.");
        });
    }
}
