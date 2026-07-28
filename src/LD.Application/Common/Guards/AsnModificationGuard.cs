using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;

namespace LD.Application.Common.Guards;

public static class AsnModificationGuard
{
    private const string CreatedStatus = "Creado";
    private const string ConfirmedStatus = "Confirmado";
    private const string CancelledStatus = "Cancelado";
    private const string TerminalStatusMessage = "El ASN esta confirmado o cancelado y no permite agregar, editar ni eliminar registros.";
    private const string DetailDeletionStatusMessage = "Solo se pueden eliminar detalles mientras el ASN tenga estatus Creado.";

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

    public static async Task<Result<string>?> EnsureAsnDetailParentAllowsDeletionAsync(
        int asnDetailId,
        IRepository<AsnDetail> asnDetailRepository,
        IRepository<Asn> asnRepository)
    {
        var asnDetail = await asnDetailRepository.GetByIdAsync(asnDetailId);
        if (asnDetail is null)
            return Result<string>.Failure("No existe el detalle de ASN", new List<string> { "No existe el detalle de ASN" }, 404);

        return await EnsureAsnAllowsDetailDeletionAsync(asnDetail.AsnId, asnRepository);
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

    public static async Task<Result<string>?> EnsureAsnReceiptParentAllowsDeletionAsync(
        int asnReceiptDetailId,
        IRepository<AsnReceiptDetail> asnReceiptDetailRepository,
        IRepository<AsnDetail> asnDetailRepository,
        IRepository<Asn> asnRepository)
    {
        var asnReceiptDetail = await asnReceiptDetailRepository.GetByIdAsync(asnReceiptDetailId);
        if (asnReceiptDetail is null)
            return Result<string>.Failure("No existe el detalle de recepción del ASN", new List<string> { "No existe el detalle de recepción del ASN" }, 404);

        return await EnsureAsnDetailParentAllowsDeletionAsync(
            asnReceiptDetail.AsnDetailId,
            asnDetailRepository,
            asnRepository);
    }

    private static async Task<Result<string>?> EnsureAsnAllowsDetailDeletionAsync(
        int asnId,
        IRepository<Asn> asnRepository)
    {
        var asn = await asnRepository.GetByIdAsync(asnId);
        if (asn is null)
            return Result<string>.Failure("No existe el ASN", new List<string> { "No existe el ASN" }, 404);

        return string.Equals(asn.Status?.Trim(), CreatedStatus, StringComparison.OrdinalIgnoreCase)
            ? null
            : Result<string>.Failure(DetailDeletionStatusMessage, new List<string> { DetailDeletionStatusMessage });
    }

    private static bool IsTerminalStatus(string? status) =>
        string.Equals(status?.Trim(), ConfirmedStatus, StringComparison.OrdinalIgnoreCase) ||
        string.Equals(status?.Trim(), CancelledStatus, StringComparison.OrdinalIgnoreCase);
}
