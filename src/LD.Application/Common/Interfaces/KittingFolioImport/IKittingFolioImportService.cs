using LD.Contracts.DTOs.KittingFolioCapture;
using LD.Contracts.Requests;

namespace LD.Application.Common.Interfaces.KittingFolioImport;

public interface IKittingFolioImportService
{
    Task<KittingFolioCapturePreviewDto> PreviewAsync(
        GenerateKittingFolioCaptureRequest request,
        CancellationToken cancellationToken = default);

    Task<(int KittingId, string KittingCode, int CaptureCount)> GenerateAsync(
        GenerateKittingFolioCaptureRequest request,
        CancellationToken cancellationToken = default);
}
