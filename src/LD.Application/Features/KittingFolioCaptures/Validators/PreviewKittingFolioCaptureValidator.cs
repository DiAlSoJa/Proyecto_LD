using FluentValidation;
using LD.Application.Features.KittingFolioCaptures.Queries;

namespace LD.Application.Features.KittingFolioCaptures.Validators;

public class PreviewKittingFolioCaptureValidator : AbstractValidator<PreviewKittingFolioCaptureQuery>
{
    public PreviewKittingFolioCaptureValidator()
    {
        KittingFolioCaptureRequestRules.Apply(this);
    }
}
