using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;

namespace LD.Application.Common.Guards;

public static class AsnReceiptValidationGuard
{
    public static async Task<Result<string>?> EnsureReceiptCanBeSavedAsync(
        AsnReceiptRequest request,
        IRepository<AsnReceiptDetail> receiptRepository,
        IRepository<AsnDetail> detailRepository,
        IRepository<Asn> asnRepository,
        IRepository<AvailableInventory> availableInventoryRepository,
        IProjectRepository projectRepository,
        int? excludedReceiptId = null)
    {
        var detail = await detailRepository.GetByIdAsync(request.AsnDetailId);
        if (detail is null)
            return Failure("No existe el detalle de ASN.", 404);

        var detailValidation = ValidateReceiptMatchesDetail(request, detail);
        if (detailValidation is not null)
            return detailValidation;

        var receipts = await receiptRepository.GetManyAsync() ?? new List<AsnReceiptDetail>();

        var standardIdValidation = ValidateStandardId(request, receipts, excludedReceiptId);
        if (standardIdValidation is not null)
            return standardIdValidation;

        var quantityValidation = ValidateQuantity(request, detail, receipts, excludedReceiptId);
        if (quantityValidation is not null)
            return quantityValidation;

        return await EnsureUniqueLotIsNotDuplicatedAsync(
            detail,
            request.LotNumber,
            receipts,
            detailRepository,
            asnRepository,
            availableInventoryRepository,
            projectRepository,
            excludedReceiptId);
    }

    private static Result<string>? ValidateReceiptMatchesDetail(AsnReceiptRequest request, AsnDetail detail)
    {
        var requestedPartNumber = Normalize(request.PartNumber);
        if (string.IsNullOrWhiteSpace(requestedPartNumber))
            return Failure("PartNumber es obligatorio.", 400);

        var detailPartNumber = Normalize(detail.PartNumber);
        if (!string.Equals(requestedPartNumber, detailPartNumber, StringComparison.OrdinalIgnoreCase))
        {
            return Failure(
                $"El numero de parte {requestedPartNumber} no coincide con el detalle de ASN {detailPartNumber}.",
                400);
        }

        var requestedLot = Normalize(request.LotNumber);
        if (!string.IsNullOrWhiteSpace(requestedLot))
        {
            var detailLot = Normalize(detail.LotNumber);
            if (string.IsNullOrWhiteSpace(detailLot) || !string.Equals(requestedLot, detailLot, StringComparison.OrdinalIgnoreCase))
            {
                return Failure(
                    $"El lote {requestedLot} no coincide con el detalle de ASN.",
                    400);
            }
        }

        return null;
    }

    private static Result<string>? ValidateStandardId(
        AsnReceiptRequest request,
        IReadOnlyCollection<AsnReceiptDetail> receipts,
        int? excludedReceiptId)
    {
        var standardIdText = Normalize(request.StandardId);
        if (string.IsNullOrWhiteSpace(standardIdText))
            return null;

        if (!int.TryParse(standardIdText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var standardId) || standardId <= 0)
            return Failure("StandardId es invalido.", 400);

        var alreadyAssigned = receipts.Any(x =>
            x.StandardId == standardId
            && (!excludedReceiptId.HasValue || x.AsnReceiptDetailId != excludedReceiptId.Value));

        if (!alreadyAssigned)
            return null;

        return Failure($"El StandardId {standardIdText} ya fue agregado previamente.", 400);
    }

    private static Result<string>? ValidateQuantity(
        AsnReceiptRequest request,
        AsnDetail detail,
        IReadOnlyCollection<AsnReceiptDetail> receipts,
        int? excludedReceiptId)
    {
        if (!request.ReceivedQuantity.HasValue)
            return null;

        if (request.ReceivedQuantity.Value <= 0)
            return Failure("ReceivedQuantity debe ser mayor a cero.", 400);

        var currentQuantity = receipts
            .Where(x => x.AsnDetailId == detail.AsnDetailId
                && (!excludedReceiptId.HasValue || x.AsnReceiptDetailId != excludedReceiptId.Value))
            .Sum(x => x.ReceivedQuantity ?? 0m);

        var totalQuantity = currentQuantity + request.ReceivedQuantity.Value;
        if (totalQuantity <= detail.Quantity)
            return null;

        return Failure(
            $"La suma de las cantidades de recepciones ({totalQuantity:0.##}) no puede ser mayor a la cantidad del detalle ({detail.Quantity:0.##}).",
            400);
    }

    private static async Task<Result<string>?> EnsureUniqueLotIsNotDuplicatedAsync(
        AsnDetail detail,
        string? lotNumber,
        IReadOnlyCollection<AsnReceiptDetail> receipts,
        IRepository<AsnDetail> detailRepository,
        IRepository<Asn> asnRepository,
        IRepository<AvailableInventory> availableInventoryRepository,
        IProjectRepository projectRepository,
        int? excludedReceiptId)
    {
        var normalizedLot = Normalize(lotNumber);
        if (string.IsNullOrWhiteSpace(normalizedLot))
            return null;

        var asn = await asnRepository.GetByIdAsync(detail.AsnId);
        if (asn is null)
            return null;

        var project = await projectRepository.GetByIdAsync(asn.ProjectId);
        if (project is null || !project.UniqueLot)
            return null;

        var asnIdsInProject = (await asnRepository.GetManyAsync() ?? new List<Asn>())
            .Where(x => x.ProjectId == project.ProjectId)
            .Select(x => x.AsnId)
            .ToHashSet();

        var asnDetailIds = (await detailRepository.GetManyAsync() ?? new List<AsnDetail>())
            .Where(x => asnIdsInProject.Contains(x.AsnId))
            .Select(x => x.AsnDetailId)
            .ToHashSet();

        var duplicateReceiptExists = receipts.Any(x =>
            (!excludedReceiptId.HasValue || x.AsnReceiptDetailId != excludedReceiptId.Value)
            && asnDetailIds.Contains(x.AsnDetailId)
            && !string.IsNullOrWhiteSpace(x.LotNumber)
            && string.Equals(x.LotNumber.Trim(), normalizedLot, StringComparison.OrdinalIgnoreCase));

        var inventories = await availableInventoryRepository.GetManyAsync() ?? new List<AvailableInventory>();
        var duplicateInventoryExists = inventories.Any(x =>
            x.ClientId == asn.ClientId
            && x.ProjectId == project.ProjectId
            && !string.IsNullOrWhiteSpace(x.LotNumber)
            && string.Equals(x.LotNumber.Trim(), normalizedLot, StringComparison.OrdinalIgnoreCase));

        if (!duplicateReceiptExists && !duplicateInventoryExists)
            return null;

        var message = $"El lote {normalizedLot} ya existe en recepciones o inventario para este cliente y proyecto y no se permite repetirlo.";
        return Failure(message, 400);
    }

    private static Result<string> Failure(string message, int code) =>
        Result<string>.Failure(message, new List<string> { message }, code);

    private static string Normalize(string? value) => value?.Trim() ?? string.Empty;
}
