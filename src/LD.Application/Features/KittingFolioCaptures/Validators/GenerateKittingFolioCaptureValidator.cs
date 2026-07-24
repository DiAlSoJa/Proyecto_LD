using FluentValidation;
using LD.Application.Features.KittingFolioCaptures.Commands;

namespace LD.Application.Features.KittingFolioCaptures.Validators;

public class GenerateKittingFolioCaptureValidator : AbstractValidator<GenerateKittingFolioCaptureCommand>
{
    public GenerateKittingFolioCaptureValidator()
    {
        KittingFolioCaptureRequestRules.Apply(this);
    }
}
