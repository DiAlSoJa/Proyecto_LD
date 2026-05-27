using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.ASN;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Asn.Queries;

public class LocatingAsnPalletsQuery : IRequest<Result<List<LocatingAsnPalletDto>>>
{
}

public class LocatingAsnPalletsQueryHandler : IRequestHandler<LocatingAsnPalletsQuery, Result<List<LocatingAsnPalletDto>>>
{
    private readonly IAsnRepository _asnRepository;
    private readonly IRepository<LD.Domain.Entities.AsnDetail> _asnDetailRepository;
    private readonly IRepository<AsnReceiptDetail> _asnReceiptDetailRepository;
    private readonly IRepository<AvailableInventory> _availableInventoryRepository;

    public LocatingAsnPalletsQueryHandler(
        IAsnRepository asnRepository,
        IRepository<LD.Domain.Entities.AsnDetail> asnDetailRepository,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IRepository<AvailableInventory> availableInventoryRepository)
    {
        _asnRepository = asnRepository;
        _asnDetailRepository = asnDetailRepository;
        _asnReceiptDetailRepository = asnReceiptDetailRepository;
        _availableInventoryRepository = availableInventoryRepository;
    }

    public async Task<Result<List<LocatingAsnPalletDto>>> Handle(LocatingAsnPalletsQuery request, CancellationToken cancellationToken)
    {
        var asns = (await _asnRepository.GetManyAsync() ?? new List<LD.Domain.Entities.Asn>())
            .Where(x => string.Equals(x.Status?.Trim(), "Ubicando", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!asns.Any())
            return Result<List<LocatingAsnPalletDto>>.Success(new List<LocatingAsnPalletDto>(), "ASN por ubicar obtenidos correctamente.");

        var asnDetails = await _asnDetailRepository.GetManyAsync() ?? new List<LD.Domain.Entities.AsnDetail>();
        var receiptDetails = await _asnReceiptDetailRepository.GetManyAsync() ?? new List<AsnReceiptDetail>();
        var availableStandardIds = (await _availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>())
            .Where(x => x.StandardId.HasValue)
            .Select(x => x.StandardId!.Value)
            .ToHashSet();

        var result = new List<LocatingAsnPalletDto>();

        foreach (var asn in asns.OrderBy(x => x.AsnCode))
        {
            var asnDetailIds = asnDetails
                .Where(x => x.AsnId == asn.AsnId)
                .Select(x => x.AsnDetailId)
                .ToHashSet();

            var pendingReceipts = receiptDetails
                .Where(x => asnDetailIds.Contains(x.AsnDetailId))
                .Where(x => !x.StandardId.HasValue || !availableStandardIds.Contains(x.StandardId.Value))
                .OrderBy(x => x.AsnReceiptDetailId)
                .ToList();

            if (!pendingReceipts.Any())
                continue;

            var pendingPallets = pendingReceipts
                .Where(x => x.StandardId.HasValue)
                .Select(x => x.StandardId!.Value)
                .Distinct()
                .Count();

            if (pendingPallets == 0)
                pendingPallets = pendingReceipts.Count;

            result.Add(new LocatingAsnPalletDto
            {
                AsnId = asn.AsnId,
                AsnCode = string.IsNullOrWhiteSpace(asn.AsnCode) ? asn.AsnId.ToString() : asn.AsnCode,
                PalletsPorMover = pendingPallets,
                LocationCode = pendingReceipts
                    .Select(x => x.LocationCode?.Trim())
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? string.Empty
            });
        }

        return Result<List<LocatingAsnPalletDto>>.Success(result, "ASN por ubicar obtenidos correctamente.");
    }
}
