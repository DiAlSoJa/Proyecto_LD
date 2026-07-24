using FluentValidation;
using LD.Contracts.Requests;

namespace LD.Application.Features.KittingFolioCaptures.Validators;

internal static class KittingFolioCaptureRequestRules
{
    public static void Apply<T>(AbstractValidator<T> validator)
        where T : GenerateKittingFolioCaptureRequest
    {
        validator.RuleFor(x => x.ClientId)
            .GreaterThan(0)
            .WithMessage("Cliente es obligatorio");

        validator.RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("Proyecto es obligatorio");

        validator.RuleFor(x => x.SourceFileName)
            .NotEmpty()
            .WithMessage("Selecciona un archivo para generar el Kitting");

        validator.RuleFor(x => x.FileContent)
            .NotEmpty()
            .WithMessage("El archivo seleccionado esta vacio");
    }
}
