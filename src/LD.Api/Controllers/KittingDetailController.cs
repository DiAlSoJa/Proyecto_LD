using AutoMapper;
using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class KittingDetailController : CommonController
{
    private readonly LdProyectDbContext _context;
    private readonly IMapper _mapper;

    public KittingDetailController(LdProyectDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingDetail()
    {
        var entities = await _context.KittingDetails
            .AsNoTracking()
            .OrderByDescending(x => x.KittingDetailId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingDetailDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingDetailDto>?>.Success(dto, "Kitting Details obtenidos correctamente"));
    }

    [HttpGet("{kittingDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingDetailById(int kittingDetailId)
    {
        var entity = await _context.KittingDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.KittingDetailId == kittingDetailId);

        if (entity is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingDetailRequest?>.Failure("Kitting Detail no encontrado.", new List<string> { "No existe el Kitting Detail." }, 404));
        }

        var dto = _mapper.Map<KittingDetailRequest>(entity);
        return ResultExtensions.ToActionResult(Result<KittingDetailRequest?>.Success(dto, "Kitting Detail obtenido correctamente"));
    }

    [HttpGet("kitting/{kittingId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingDetailByKitting(int kittingId)
    {
        var entities = await _context.KittingDetails
            .AsNoTracking()
            .Where(x => x.KittingId == kittingId)
            .OrderByDescending(x => x.KittingDetailId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingDetailDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingDetailDto>>.Success(dto, "Kitting Details obtenidos correctamente"));
    }

    [HttpPost]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateKittingDetail([FromBody] KittingDetailRequest request)
    {
        try
        {
            if (request.KittingId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("KittingId es obligatorio.", new List<string> { "KittingId es obligatorio." }));

            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return ResultExtensions.ToActionResult(Result<string>.Failure("PartNumber es obligatorio.", new List<string> { "PartNumber es obligatorio." }));

            var validation = await EnsureKittingEditableAsync(request.KittingId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            var entity = _mapper.Map<KittingDetail>(request);
            entity.Status = NormalizeStatus(entity.Status);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.KittingDetails.Add(entity);
                var result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se pudo guardar el Kitting Detail.", new List<string> { "No se pudo guardar el Kitting Detail." }));
                }

                await EnsureDefaultIssueDetailAsync(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    Result<string>.Success(entity.KittingDetailId.ToString(), "Kitting Detail creado con exito"));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al crear el Kitting Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpPut("{kittingDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UpdateKittingDetail(int kittingDetailId, [FromBody] KittingDetailRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return ResultExtensions.ToActionResult(Result<string>.Failure("PartNumber es obligatorio.", new List<string> { "PartNumber es obligatorio." }));

            var entity = await _context.KittingDetails.FirstOrDefaultAsync(x => x.KittingDetailId == kittingDetailId);
            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Detail.", new List<string> { "No existe el Kitting Detail." }, 404));
            }

            var validation = await EnsureKittingEditableAsync(entity.KittingId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            var previousQuantity = entity.Quantity;
            request.KittingDetailId = entity.KittingDetailId;
            request.KittingId = entity.KittingId;
            request.Status = NormalizeStatus(request.Status) ?? NormalizeStatus(entity.Status);

            _mapper.Map(request, entity);
            await SyncIssueDetailsFromDetailAsync(entity, previousQuantity);
            var updated = await _context.SaveChangesAsync() > 0;

            if (!updated)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo actualizar el Kitting Detail.", new List<string> { "No se pudo actualizar el Kitting Detail." }));
            }

            return ResultExtensions.ToActionResult(
                Result<string>.Success(entity.KittingDetailId.ToString(), "Kitting Detail actualizado"));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al actualizar el Kitting Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpDelete("{kittingDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> DeleteKittingDetail(int kittingDetailId)
    {
        try
        {
            var entity = await _context.KittingDetails.FirstOrDefaultAsync(x => x.KittingDetailId == kittingDetailId);
            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Detail.", new List<string> { "No existe el Kitting Detail." }, 404));
            }

            var validation = await EnsureKittingEditableAsync(entity.KittingId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var issueDetails = await _context.KittingIssueDetails
                    .Where(x => x.KittingDetailId == kittingDetailId)
                    .ToListAsync();

                if (issueDetails.Count > 0)
                {
                    _context.KittingIssueDetails.RemoveRange(issueDetails);
                }

                _context.KittingDetails.Remove(entity);
                var deleted = await _context.SaveChangesAsync() > 0;

                if (!deleted)
                {
                    await transaction.RollbackAsync();
                    return ResultExtensions.ToActionResult(
                        Result<string>.Failure("No se pudo eliminar el Kitting Detail.", new List<string> { "No se pudo eliminar el Kitting Detail." }));
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return ResultExtensions.ToActionResult(
                Result<string>.Success(kittingDetailId.ToString(), "Kitting Detail eliminado"));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al eliminar el Kitting Detail.", new List<string> { ex.Message }));
        }
    }

    private async Task<Result<string>?> EnsureKittingEditableAsync(int kittingId)
    {
        var kitting = await _context.Kittings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.KittingId == kittingId);

        if (kitting is null)
        {
            return Result<string>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404);
        }

        if (IsTerminalStatus(kitting.Status))
        {
            return Result<string>.Failure(
                "El Kitting ya esta finalizado y no se puede modificar.",
                new List<string> { "El Kitting ya esta finalizado." });
        }

        return null;
    }

    private async Task EnsureDefaultIssueDetailAsync(KittingDetail detail)
    {
        var hasIssues = await _context.KittingIssueDetails
            .AsNoTracking()
            .AnyAsync(x => x.KittingDetailId == detail.KittingDetailId);

        if (hasIssues)
            return;

        _context.KittingIssueDetails.Add(BuildDefaultIssueDetail(detail));
    }

    private async Task SyncIssueDetailsFromDetailAsync(KittingDetail detail, decimal previousQuantity)
    {
        var issueDetails = await _context.KittingIssueDetails
            .Where(x => x.KittingDetailId == detail.KittingDetailId)
            .ToListAsync();

        if (issueDetails.Count == 0)
        {
            _context.KittingIssueDetails.Add(BuildDefaultIssueDetail(detail));
            return;
        }

        foreach (var issueDetail in issueDetails)
        {
            issueDetail.ProductId = NormalizeProductId(detail.ProductId);
            issueDetail.PartNumber = detail.PartNumber;
            issueDetail.Description = detail.Description;
            issueDetail.StandardQuantity = detail.StandardQuantity;
            issueDetail.MaximumQuantity = detail.MaximumQuantity;
            issueDetail.LotNumber = detail.LotNumber;
            issueDetail.ExpirationDate = detail.ExpirationDate;
            issueDetail.Reference = detail.CustomerReference;
            issueDetail.PurchaseOrder = detail.PurchaseOrder;
            issueDetail.CustomsDeclarationNumber = detail.CustomsDeclarationNumber;

            if (issueDetails.Count == 1 &&
                (!issueDetail.ReceivedQuantity.HasValue || issueDetail.ReceivedQuantity.Value == previousQuantity))
            {
                issueDetail.ReceivedQuantity = detail.Quantity;
            }

            if (string.IsNullOrWhiteSpace(issueDetail.Status) && !string.IsNullOrWhiteSpace(detail.Status))
            {
                issueDetail.Status = detail.Status;
            }

            if (string.IsNullOrWhiteSpace(issueDetail.SD) && !string.IsNullOrWhiteSpace(detail.SD))
            {
                issueDetail.SD = detail.SD;
            }
        }
    }

    private static KittingIssueDetail BuildDefaultIssueDetail(KittingDetail detail)
    {
        return new KittingIssueDetail
        {
            KittingDetailId = detail.KittingDetailId,
            ProductId = NormalizeProductId(detail.ProductId),
            PartNumber = detail.PartNumber,
            Description = detail.Description,
            StandardQuantity = detail.StandardQuantity,
            MaximumQuantity = detail.MaximumQuantity,
            SD = detail.SD,
            ReceivedQuantity = detail.Quantity,
            Status = detail.Status,
            LotNumber = detail.LotNumber,
            ExpirationDate = detail.ExpirationDate,
            Reference = detail.CustomerReference,
            PurchaseOrder = detail.PurchaseOrder,
            CustomsDeclarationNumber = detail.CustomsDeclarationNumber
        };
    }

    private static int? NormalizeProductId(int? productId)
    {
        return productId.HasValue && productId.Value > 0
            ? productId.Value
            : null;
    }

    private static bool IsTerminalStatus(string? status) =>
        IsConfirmedStatus(status) || IsCancelledStatus(status);

    private static bool IsConfirmedStatus(string? status) =>
        string.Equals(status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase);

    private static bool IsCancelledStatus(string? status) =>
        string.Equals(status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase);

    private static string? NormalizeStatus(string? status) =>
        string.IsNullOrWhiteSpace(status)
            ? null
            : status.Trim();
}
