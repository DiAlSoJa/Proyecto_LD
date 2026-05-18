using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.StandarLabel;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Asn.Commands;

public class LocateAsnCommand : IRequest<Result<string>>
{
    public int AsnId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class LocateAsnCommandHandler : IRequestHandler<LocateAsnCommand, Result<string>>
{
    private readonly IAsnRepository _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<AsnReceiptDetail> _asnReceiptDetailRepository;
    private readonly IStandarIdService _standarIdService;

    public LocateAsnCommandHandler(
        IAsnRepository asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IStandarIdService standarIdService)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnReceiptDetailRepository = asnReceiptDetailRepository;
        _standarIdService = standarIdService;
    }

    public async Task<Result<string>> Handle(LocateAsnCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new(), 401);

            var asn = await _asnRepository.GetByIdAsync(request.AsnId);

            if (asn == null)
                return Result<string>.Failure("ASN no encontrado.", new(), 404);

            if (string.Equals(asn.Status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta confirmado y no se puede ubicar.", new());

            if (string.Equals(asn.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN esta cancelado y no se puede ubicar.", new());

            if (string.Equals(asn.Status?.Trim(), "Ubicando", StringComparison.OrdinalIgnoreCase))
                return Result<string>.Failure("El ASN ya esta en estatus Ubicando.", new());

            var asnDetails = await _asnDetailRepository.GetManyAsync() ?? new List<LD.Domain.Entities.AsnDetail>();
            var asnDetailIds = asnDetails
                .Where(x => x.AsnId == request.AsnId)
                .Select(x => x.AsnDetailId)
                .ToHashSet();

            var details = (await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>())
                .Where(x => asnDetailIds.Contains(x.AsnDetailId))
                .ToList();

            if (!details.Any())
                return Result<string>.Failure("El ASN debe tener al menos un ASN Receipt Detail.", new());

            var invalidDetails = details
                .Where(x =>
                    (x.LocationId is null && string.IsNullOrWhiteSpace(x.LocationCode)) ||
                    string.IsNullOrWhiteSpace(x.Status) ||
                    string.IsNullOrWhiteSpace(x.SD))
                .Select(x => string.IsNullOrWhiteSpace(x.PartNumber)
                    ? x.AsnReceiptDetailId.ToString()
                    : x.PartNumber)
                .Distinct()
                .ToList();

            if (invalidDetails.Any())
            {
                return Result<string>.Failure(
                    $"Antes de confirmar, todos los asnreceiptdetails deben tener ubicacion, estatus y SD. Lineas con problema: {string.Join(", ", invalidDetails)}.",
                    new());
            }

            var detailIdsWithoutStandardId = details
                .Where(x => !x.StandardId.HasValue)
                .Select(x => x.AsnReceiptDetailId)
                .ToList();

            if (detailIdsWithoutStandardId.Any())
                await _standarIdService.AssignStandarIdsToReceiptDetailsAsync(detailIdsWithoutStandardId, request.UserId);

            asn.Status = "Ubicando";
            asn.LastModifiedAt = DateTime.Now;
            asn.LastModifiedByUserId = request.UserId;

            var updated = await _asnRepository.UpdateAsync(asn);
            if (!updated)
                return Result<string>.Failure("No se pudo actualizar el ASN a Ubicando.", new());

            return Result<string>.Success(asn.AsnId.ToString(), "ASN marcado como Ubicando correctamente.");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al marcar el ASN como Ubicando.", new() { ex.Message });
        }
    }
}
