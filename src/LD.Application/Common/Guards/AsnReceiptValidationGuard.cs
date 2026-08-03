using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Enums;
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

        var uniqueValidation = await EnsureUniqueConfiguredValuesAreNotDuplicatedAsync(
            detail,
            request,
            receipts,
            detailRepository,
            asnRepository,
            projectRepository,
            excludedReceiptId);
        if (uniqueValidation is not null)
            return uniqueValidation;

        return null;
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

        var detailLot = Normalize(detail.LotNumber);
        var requestedLot = Normalize(request.LotNumber);
        if (!string.IsNullOrWhiteSpace(detailLot)
            && !string.Equals(requestedLot, detailLot, StringComparison.OrdinalIgnoreCase))
        {
            return Failure(
                $"El lote {requestedLot} no coincide con el detalle de ASN.",
                400);
        }

        if (detail.ExpirationDate.HasValue
            && (!request.ExpirationDate.HasValue
                || request.ExpirationDate.Value.Date != detail.ExpirationDate.Value.Date))
        {
            return Failure(
                $"La fecha de caducidad {request.ExpirationDate:dd/MM/yyyy} no coincide con el detalle de ASN.",
                400);
        }

        var detailReference = Normalize(detail.CustomerReference);
        var requestedReference = Normalize(request.Reference);
        if (!string.IsNullOrWhiteSpace(detailReference)
            && !string.Equals(requestedReference, detailReference, StringComparison.OrdinalIgnoreCase))
        {
            return Failure(
                $"La referencia de cliente {requestedReference} no coincide con el detalle de ASN.",
                400);
        }

        var detailPurchaseOrder = Normalize(detail.PurchaseOrder);
        var requestedPurchaseOrder = Normalize(request.PurchaseOrder);
        if (!string.IsNullOrWhiteSpace(detailPurchaseOrder)
            && !string.Equals(requestedPurchaseOrder, detailPurchaseOrder, StringComparison.OrdinalIgnoreCase))
        {
            return Failure(
                $"La orden de compra {requestedPurchaseOrder} no coincide con el detalle de ASN.",
                400);
        }

        var detailCustomsDeclaration = Normalize(detail.CustomsDeclarationNumber);
        var requestedCustomsDeclaration = Normalize(request.CustomsDeclarationNumber);
        if (!string.IsNullOrWhiteSpace(detailCustomsDeclaration)
            && !string.Equals(requestedCustomsDeclaration, detailCustomsDeclaration, StringComparison.OrdinalIgnoreCase))
        {
            return Failure(
                $"El pedimento {requestedCustomsDeclaration} no coincide con el detalle de ASN.",
                400);
        }

        if (detail.ExchangeRate.HasValue
            && (!request.ExchangeRate.HasValue
                || request.ExchangeRate.Value != detail.ExchangeRate.Value))
        {
            return Failure(
                $"El tipo de cambio {request.ExchangeRate?.ToString(CultureInfo.InvariantCulture) ?? string.Empty} no coincide con el detalle de ASN.",
                400);
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

    private static async Task<Result<string>?> EnsureUniqueConfiguredValuesAreNotDuplicatedAsync(
        AsnDetail detail,
        AsnReceiptRequest request,
        IReadOnlyCollection<AsnReceiptDetail> receipts,
        IRepository<AsnDetail> detailRepository,
        IRepository<Asn> asnRepository,
        IProjectRepository projectRepository,
        int? excludedReceiptId)
    {
        var asn = await asnRepository.GetByIdAsync(detail.AsnId);
        if (asn is null)
            return null;

        var project = await projectRepository.GetByIdAsync(asn.ProjectId);
        if (project is null)
            return null;

        var uniqueConfigurations = project.ScanConfigurations
            .Where(config => config.IsUnique)
            .ToList();

        if (uniqueConfigurations.Count == 0)
            return null;

        var asnDetailIds = await GetAsnDetailIdsAsync(detailRepository, detail.AsnId);
        if (asnDetailIds.Count == 0)
            return null;

        foreach (var config in uniqueConfigurations)
        {
            if (IsStandardIdConfiguration(config))
                continue;

            var requestedValue = Normalize(GetConfiguredValue(request, config));
            if (string.IsNullOrWhiteSpace(requestedValue))
                continue;

            var duplicateExists = receipts.Any(x =>
                (!excludedReceiptId.HasValue || x.AsnReceiptDetailId != excludedReceiptId.Value)
                && asnDetailIds.Contains(x.AsnDetailId)
                && string.Equals(GetConfiguredValue(x, config), requestedValue, StringComparison.OrdinalIgnoreCase));

            if (duplicateExists)
            {
                var fieldLabel = ResolveFieldLabel(config);
                return Failure($"El dato {requestedValue} del campo {fieldLabel} ya existe en recepciones del ASN.", 400);
            }
        }

        return null;
    }

    private static async Task<HashSet<int>> GetAsnDetailIdsAsync(
        IRepository<AsnDetail> detailRepository,
        int asnId)
    {
        return (await detailRepository.GetManyAsync() ?? new List<AsnDetail>())
            .Where(x => x.AsnId == asnId)
            .Select(x => x.AsnDetailId)
            .ToHashSet();
    }

    private static bool IsStandardIdConfiguration(ScanConfiguration config)
    {
        return string.Equals(NormalizeField(config), "standard_id", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetConfiguredValue(AsnReceiptRequest request, ScanConfiguration config)
    {
        return NormalizeConfiguredValue(NormalizeField(config), request);
    }

    private static string GetConfiguredValue(AsnReceiptDetail receipt, ScanConfiguration config)
    {
        return NormalizeConfiguredValue(NormalizeField(config), receipt);
    }

    private static string NormalizeConfiguredValue(string fieldKey, AsnReceiptRequest request)
    {
        return fieldKey switch
        {
            "lot_number" => Normalize(request.LotNumber),
            "customer_reference" => Normalize(request.Reference),
            "purchase_order" => Normalize(request.PurchaseOrder),
            "customs_declaration" => Normalize(request.CustomsDeclarationNumber),
            "qty" => NormalizeQuantity(request.ReceivedQuantity),
            "expiration_date" => NormalizeDate(request.ExpirationDate),
            "exchange_rate" => NormalizeQuantity(request.ExchangeRate),
            "sd" => Normalize(request.SD),
            "status" => Normalize(request.Status),
            "standard_id" or "standardid" => Normalize(request.StandardId),
            "part_number" or "partnumber" or "numero de parte" or "nÃºmero de parte" => Normalize(request.PartNumber),
            _ => string.Empty
        };
    }

    private static string NormalizeConfiguredValue(string fieldKey, AsnReceiptDetail receipt)
    {
        return fieldKey switch
        {
            "lot_number" => Normalize(receipt.LotNumber),
            "customer_reference" => Normalize(receipt.Reference),
            "purchase_order" => Normalize(receipt.PurchaseOrder),
            "customs_declaration" => Normalize(receipt.CustomsDeclarationNumber),
            "qty" => NormalizeQuantity(receipt.ReceivedQuantity),
            "expiration_date" => NormalizeDate(receipt.ExpirationDate),
            "exchange_rate" => NormalizeQuantity(receipt.ExchangeRate),
            "sd" => Normalize(receipt.SD),
            "status" => Normalize(receipt.Status),
            "standard_id" or "standardid" => NormalizeStandardId(receipt.StandardId),
            "part_number" or "partnumber" or "numero de parte" or "nÃºmero de parte" => Normalize(receipt.PartNumber),
            _ => string.Empty
        };
    }

    private static string NormalizeField(ScanConfiguration config)
    {
        foreach (var candidate in GetConfiguredFieldNameCandidates(config))
        {
            if (TryNormalizeConfiguredFieldKey(candidate, out var normalizedFieldKey))
                return normalizedFieldKey;
        }

        var fallbackField = GetConfiguredFieldNameCandidates(config)
            .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

        if (!string.IsNullOrWhiteSpace(fallbackField))
            return NormalizeFieldName(fallbackField);

        return config.SystemFieldId switch
        {
            (int)SystemField_e.LotNumber => "lot_number",
            (int)SystemField_e.CustomerReference => "customer_reference",
            (int)SystemField_e.PurchaseOrder => "purchase_order",
            (int)SystemField_e.CustomsDeclaration => "customs_declaration",
            (int)SystemField_e.Qty => "qty",
            (int)SystemField_e.StandardId => "standard_id",
            (int)SystemField_e.PartNumber => "partnumber",
            _ => string.Empty
        };
    }

    private static IEnumerable<string?> GetConfiguredFieldNameCandidates(ScanConfiguration config)
    {
        yield return config.ClientField;
        yield return config.SystemField?.SystemFieldName;
    }

    private static bool TryNormalizeConfiguredFieldKey(string? fieldName, out string normalizedFieldKey)
    {
        normalizedFieldKey = string.Empty;

        var normalized = NormalizeFieldName(fieldName);
        if (string.IsNullOrWhiteSpace(normalized))
            return false;

        normalizedFieldKey = normalized switch
        {
            "lotnumber" or "lote" or "lot number" or "lot_number" => "lot_number",
            "customerreference" or "referencia de cliente" or "referencia cliente" or "referencia" => "customer_reference",
            "purchaseorder" or "orden de compra" or "purchase order" or "purchase_order" or "po" => "purchase_order",
            "customsdeclaration" or "pedimento" or "declaracion aduanal" or "declaracion de aduana" or "customs declaration" or "customs_declaration" => "customs_declaration",
            "qty" or "quantity" or "cantidad" => "qty",
            "expirationdate" or "fecha de caducidad" or "caducidad" or "expiration date" or "expiration_date" => "expiration_date",
            "exchangerate" or "tipo de cambio" or "tipo cambio" or "exchange rate" or "exchange_rate" or "tc" => "exchange_rate",
            "standardid" or "standard id" or "standard_id" => "standard_id",
            "partnumber" or "part number" or "part_number" or "numerodeparte" or "numero de parte" => "partnumber",
            "sd" => "sd",
            "status" or "estatus" => "status",
            _ => string.Empty
        };

        return !string.IsNullOrWhiteSpace(normalizedFieldKey);
    }

    private static string NormalizeFieldName(string? fieldName)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return string.Empty;

        var normalized = fieldName.Trim().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var ch in normalized)
        {
            if (char.GetUnicodeCategory(ch) == UnicodeCategory.NonSpacingMark)
                continue;

            builder.Append(char.ToLowerInvariant(ch));
        }

        return builder.ToString();
    }

    private static string NormalizeQuantity(decimal? value) =>
        value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;

    private static string NormalizeDate(DateTime? value) =>
        value.HasValue ? value.Value.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : string.Empty;

    private static string NormalizeQuantity(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed)
            || decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out parsed))
        {
            return parsed.ToString(CultureInfo.InvariantCulture);
        }

        return value.Trim();
    }

    private static string NormalizeStandardId(int? value) =>
        value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;

    private static string ResolveFieldLabel(ScanConfiguration config)
    {
        if (!string.IsNullOrWhiteSpace(config.ClientField))
            return config.ClientField.Trim();

        if (!string.IsNullOrWhiteSpace(config.SystemField?.SystemFieldName))
            return config.SystemField.SystemFieldName.Trim();

        return NormalizeField(config);
    }

    private static Result<string> Failure(string message, int code) =>
        Result<string>.Failure(message, new List<string> { message }, code);

    private static string Normalize(string? value) => value?.Trim() ?? string.Empty;
}
