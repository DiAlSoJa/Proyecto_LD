using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;

namespace LD.Application.Common.Guards;

public static class AsnModificationGuard
{
    private const string ConfirmedStatus = "Confirmado";
    private const string CancelledStatus = "Cancelado";
    private const string TerminalStatusMessage = "El ASN esta confirmado o cancelado y no permite agregar, editar ni eliminar registros.";

    public static async Task<Result<string>?> EnsureAsnIsEditableAsync(
        int asnId,
        IRepository<Asn> asnRepository)
    {
        var asn = await asnRepository.GetByIdAsync(asnId);
        if (asn is null)
            return Result<string>.Failure("No existe el ASN", new List<string> { "No existe el ASN" }, 404);

        return IsTerminalStatus(asn.Status)
            ? Result<string>.Failure(TerminalStatusMessage, new List<string> { TerminalStatusMessage })
            : null;
    }

    public static async Task<Result<string>?> EnsureAsnDetailParentIsEditableAsync(
        int asnDetailId,
        IRepository<AsnDetail> asnDetailRepository,
        IRepository<Asn> asnRepository)
    {
        var asnDetail = await asnDetailRepository.GetByIdAsync(asnDetailId);
        if (asnDetail is null)
            return Result<string>.Failure("No existe el detalle de ASN", new List<string> { "No existe el detalle de ASN" }, 404);

        return await EnsureAsnIsEditableAsync(asnDetail.AsnId, asnRepository);
    }

    public static async Task<Result<string>?> EnsureAsnReceiptParentIsEditableAsync(
        int asnReceiptDetailId,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IRepository<AsnDetail> asnDetailRepository,
        IRepository<Asn> asnRepository)
    {
        var asnReceiptDetail = await asnReceiptDetailRepository.GetByIdAsync(asnReceiptDetailId);
        if (asnReceiptDetail is null)
            return Result<string>.Failure("No existe el detalle de recepción del ASN", new List<string> { "No existe el detalle de recepción del ASN" }, 404);

        return await EnsureAsnDetailParentIsEditableAsync(asnReceiptDetail.AsnDetailId, asnDetailRepository, asnRepository);
    }

    public static async Task<Result<string>?> EnsureAsnReceiptIsNotLastForDetailAsync(
        int asnReceiptDetailId,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository)
    {
        var asnReceiptDetail = await asnReceiptDetailRepository.GetByIdAsync(asnReceiptDetailId);
        if (asnReceiptDetail is null)
            return Result<string>.Failure("No existe el detalle de recepción del ASN", new List<string> { "No existe el detalle de recepción del ASN" }, 404);

        var receipts = await asnReceiptDetailRepository.GetManyAsync();
        var linkedReceiptsCount = receipts?
            .Count(x => x.AsnDetailId == asnReceiptDetail.AsnDetailId) ?? 0;

        if (linkedReceiptsCount <= 1)
        {
            const string message = "No se puede eliminar la recepción porque es el único registro ligado al detalle de recepción del ASN.";
            return Result<string>.Failure(message, new List<string> { message });
        }

        return null;
    }

    private static bool IsTerminalStatus(string? status) =>
        string.Equals(status?.Trim(), ConfirmedStatus, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(status?.Trim(), CancelledStatus, StringComparison.OrdinalIgnoreCase);
}
