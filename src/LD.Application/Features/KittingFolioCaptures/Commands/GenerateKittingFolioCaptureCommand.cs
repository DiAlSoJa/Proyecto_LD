using LD.Application.Common.Interfaces.KittingFolioImport;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using MediatR;

namespace LD.Application.Features.KittingFolioCaptures.Commands;

public class GenerateKittingFolioCaptureCommand : GenerateKittingFolioCaptureRequest, IRequest<Result<string>>
{
}

public class GenerateKittingFolioCaptureCommandHandler : IRequestHandler<GenerateKittingFolioCaptureCommand, Result<string>>
{
    private readonly IKittingFolioImportService _importService;

    public GenerateKittingFolioCaptureCommandHandler(IKittingFolioImportService importService)
    {
        _importService = importService;
    }

    public async Task<Result<string>> Handle(GenerateKittingFolioCaptureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _importService.GenerateAsync(request, cancellationToken);
            return Result<string>.Success(
                result.KittingId.ToString(),
                $"Kitting {result.KittingCode} generado con {result.CaptureCount} detalle(s).");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                "No se pudo generar el Kitting desde el archivo de folios.",
                new List<string> { ex.Message });
        }
    }
}
