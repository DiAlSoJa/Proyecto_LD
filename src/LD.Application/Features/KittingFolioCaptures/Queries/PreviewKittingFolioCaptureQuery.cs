using LD.Application.Common.Interfaces.KittingFolioImport;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.KittingFolioCapture;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.KittingFolioCaptures.Queries;

public class PreviewKittingFolioCaptureQuery : GenerateKittingFolioCaptureRequest, IRequest<Result<KittingFolioCapturePreviewDto>>
{
}

public class PreviewKittingFolioCaptureQueryHandler : IRequestHandler<PreviewKittingFolioCaptureQuery, Result<KittingFolioCapturePreviewDto>>
{
    private readonly IKittingFolioImportService _importService;

    public PreviewKittingFolioCaptureQueryHandler(IKittingFolioImportService importService)
    {
        _importService = importService;
    }

    public async Task<Result<KittingFolioCapturePreviewDto>> Handle(PreviewKittingFolioCaptureQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var preview = await _importService.PreviewAsync(request, cancellationToken);
            return Result<KittingFolioCapturePreviewDto>.Success(preview, "Vista previa generada correctamente.");
        }
        catch (Exception ex)
        {
            return Result<KittingFolioCapturePreviewDto>.Failure(
                "No se pudo generar la vista previa del archivo de folios.",
                new List<string> { ex.Message });
        }
    }
}
