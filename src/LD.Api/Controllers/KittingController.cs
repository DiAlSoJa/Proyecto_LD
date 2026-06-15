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
public class KittingController : CommonController
{
    private readonly LdProyectDbContext _context;
    private readonly IMapper _mapper;

    public KittingController(LdProyectDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKitting()
    {
        var entities = await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Project)
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingDto>?>.Success(dto, "Kittings obtenidos correctamente"));
    }

    [HttpGet("{kittingId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingById(int kittingId)
    {
        var entity = await _context.Kittings
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.KittingId == kittingId);

        if (entity is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingRequest?>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404));
        }

        var dto = _mapper.Map<KittingRequest>(entity);
        return ResultExtensions.ToActionResult(Result<KittingRequest?>.Success(dto, "Kitting obtenido correctamente"));
    }

    [HttpGet("{clientId}/{projectId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingByClient(int clientId, int projectId)
    {
        var entities = await _context.Kittings
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Project)
            .Where(x => x.ClientId == clientId && x.ProjectId == projectId)
            .OrderByDescending(x => x.KittingId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingDto>>.Success(dto, "Kittings obtenidos correctamente"));
    }

    [HttpPost]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateKitting([FromBody] KittingRequest request)
    {
        try
        {
            if (request.ClientId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("Cliente es obligatorio.", new List<string> { "Cliente es obligatorio." }));

            if (request.ProjectId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("Proyecto es obligatorio.", new List<string> { "Proyecto es obligatorio." }));

            var entity = _mapper.Map<Kitting>(request);
            entity.Status = NormalizeStatus(entity.Status);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.ProjectId == entity.ProjectId);

                if (project is null)
                    throw new Exception("No se encontro el proyecto.");

                if (string.IsNullOrWhiteSpace(project.KittingPrefix))
                    throw new Exception("El proyecto no tiene configurado KittingPrefix.");

                var currentNumber = 1;
                if (!string.IsNullOrWhiteSpace(project.KittingNumber) &&
                    int.TryParse(project.KittingNumber, out var parsedNumber))
                {
                    currentNumber = parsedNumber;
                }

                entity.KittingCode = $"{project.KittingPrefix}{currentNumber:D5}";
                entity.PreKittingCode = entity.KittingCode;

                _context.Kittings.Add(entity);

                project.KittingNumber = (currentNumber + 1).ToString();
                _context.Projects.Update(project);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    result > 0
                        ? Result<string>.Success(entity.KittingId.ToString(), "Kitting creado con exito")
                        : Result<string>.Failure("Hubo un error al crear el Kitting.", new List<string> { "No se pudo guardar el Kitting." }));
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
                Result<string>.Failure("Hubo un error al crear el Kitting.", new List<string> { ex.Message }));
        }
    }

    [HttpPut("{kittingId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UpdateKitting(int kittingId, [FromBody] KittingRequest request)
    {
        try
        {
            var kitting = await _context.Kittings.FirstOrDefaultAsync(x => x.KittingId == kittingId);
            if (kitting is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting.", new List<string> { "No existe el Kitting." }, 404));
            }

            if (IsTerminalStatus(kitting.Status))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("El Kitting ya esta finalizado y no se puede editar.", new List<string> { "El Kitting ya esta finalizado." }));
            }

            request.KittingId = kitting.KittingId;
            request.KittingCode = kitting.KittingCode;
            request.PreKittingCode = kitting.PreKittingCode;
            request.Status = NormalizeStatus(request.Status) ?? NormalizeStatus(kitting.Status);

            _mapper.Map(request, kitting);
            kitting.LastModifiedAt = DateTime.Now;
            kitting.LastModifiedByUserId = CurrentUserId;

            var updated = await _context.SaveChangesAsync() > 0;
            if (!updated)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo actualizar el Kitting.", new List<string> { "No se pudo actualizar el Kitting." }));
            }

            return ResultExtensions.ToActionResult(
                Result<string>.Success(kitting.KittingId.ToString(), "Kitting actualizado"));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al actualizar el Kitting.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("{kittingId}/confirm")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> ConfirmKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Surtido", "Kitting surtido correctamente."));
    }

    [HttpPost("{kittingId}/cancel")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CancelKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Cancelado", "Kitting cancelado correctamente.", requireIssueValidation: false));
    }

    [HttpPost("{kittingId}/locate")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> LocateKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Ubicando", "Kitting marcado como Ubicando correctamente."));
    }

    [HttpPost("{kittingId}/send-to-supply")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> SendToSupplyKitting(int kittingId)
    {
        return ResultExtensions.ToActionResult(await ChangeKittingStatusAsync(kittingId, "Surtiendo", "Kitting enviado a surtir correctamente."));
    }

    private async Task<Result<string>> ChangeKittingStatusAsync(
        int kittingId,
        string targetStatus,
        string successMessage,
        bool requireIssueValidation = true)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
                return Result<string>.Failure("No se pudo identificar el usuario actual.", new List<string> { "No se pudo identificar el usuario actual." }, 401);

            var kitting = await _context.Kittings.FirstOrDefaultAsync(x => x.KittingId == kittingId);
            if (kitting is null)
                return Result<string>.Failure("Kitting no encontrado.", new List<string> { "No existe el Kitting." }, 404);

            if (IsConfirmedStatus(kitting.Status) || IsSurtidoStatus(kitting.Status))
            {
                return Result<string>.Failure("El Kitting ya esta surtido y no se puede modificar.", new List<string> { "El Kitting ya esta surtido." });
            }

            if (string.Equals(kitting.Status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                return Result<string>.Failure("El Kitting ya esta cancelado y no se puede modificar.", new List<string> { "El Kitting ya esta cancelado." });
            }

            if (string.Equals(kitting.Status?.Trim(), targetStatus, StringComparison.OrdinalIgnoreCase))
            {
                return Result<string>.Failure(
                    $"El Kitting ya esta en estatus {targetStatus}.",
                    new List<string> { $"El Kitting ya esta en estatus {targetStatus}." });
            }

            var kittingDetails = new List<KittingDetail>();
            if (requireIssueValidation || IsSurtidoStatus(targetStatus))
            {
                kittingDetails = await GetKittingDetailsForKittingAsync(kittingId);
            }

            if (requireIssueValidation)
            {
                var actionLabel = GetStatusValidationActionLabel(targetStatus);
                var issueDetails = await GetIssueDetailsForKittingDetailsAsync(
                    kittingDetails.Select(x => x.KittingDetailId).ToList());

                if ((IsConfirmedStatus(targetStatus) || IsSurtidoStatus(targetStatus)) && kittingDetails.Count == 0)
                {
                    return Result<string>.Failure(
                        "El Kitting debe tener al menos un Kitting Detail antes de surtir.",
                        new List<string> { "El Kitting debe tener al menos un Kitting Detail antes de surtir." });
                }

                if (issueDetails.Count == 0)
                {
                    return Result<string>.Failure(
                        "El Kitting debe tener al menos un Kitting Issue Detail.",
                        new List<string> { "El Kitting debe tener al menos un Kitting Issue Detail." });
                }

                if (IsConfirmedStatus(targetStatus) || IsSurtidoStatus(targetStatus))
                {
                    var detailIdsWithIssues = issueDetails
                        .Select(x => x.KittingDetailId)
                        .Distinct()
                        .ToHashSet();

                    var detailsWithoutIssues = kittingDetails
                        .Where(x => !detailIdsWithIssues.Contains(x.KittingDetailId))
                        .Select(GetKittingDetailLabel)
                        .Distinct()
                        .ToList();

                    if (detailsWithoutIssues.Any())
                    {
                        return Result<string>.Failure(
                            $"Antes de {actionLabel}, cada linea del Kitting debe tener al menos un Kitting Issue Detail. Lineas sin issue detail: {string.Join(", ", detailsWithoutIssues)}.",
                            new List<string>());
                    }
                }

                var invalidDetails = issueDetails
                    .Where(x =>
                        (x.LocationId is null && string.IsNullOrWhiteSpace(x.LocationCode)) ||
                        string.IsNullOrWhiteSpace(x.Status) ||
                        string.IsNullOrWhiteSpace(x.SD))
                    .Select(x => string.IsNullOrWhiteSpace(x.PartNumber)
                        ? x.KittingReceiptDetailId.ToString()
                        : x.PartNumber)
                    .Distinct()
                    .ToList();

                if (invalidDetails.Any())
                {
                    return Result<string>.Failure(
                        $"Antes de {actionLabel}, todos los Kitting Issue Details deben tener ubicacion, estatus y SD. Lineas con problema: {string.Join(", ", invalidDetails)}.",
                        new List<string>());
                }
            }

            if (IsSurtidoStatus(targetStatus))
            {
                await UpdateIssueDetailsSupplyStatusAsync(
                    kittingDetails.Select(x => x.KittingDetailId).ToList(),
                    targetStatus);
            }

            kitting.Status = targetStatus;
            kitting.LastModifiedAt = DateTime.Now;
            kitting.LastModifiedByUserId = CurrentUserId;

            var updated = await _context.SaveChangesAsync() > 0;
            if (!updated)
                return Result<string>.Failure("No se pudo actualizar el Kitting.", new List<string> { "No se pudo actualizar el Kitting." });

            return Result<string>.Success(kitting.KittingId.ToString(), successMessage);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el Kitting.", new List<string> { ex.Message });
        }
    }

    private async Task<List<KittingDetail>> GetKittingDetailsForKittingAsync(int kittingId)
    {
        return await _context.KittingDetails
            .AsNoTracking()
            .Where(x => x.KittingId == kittingId)
            .OrderBy(x => x.KittingDetailId)
            .ToListAsync();
    }

    private async Task<List<KittingIssueDetail>> GetIssueDetailsForKittingDetailsAsync(List<int> detailIds)
    {
        if (detailIds.Count == 0)
        {
            return new List<KittingIssueDetail>();
        }

        return await _context.KittingIssueDetails
            .AsNoTracking()
            .Where(x => detailIds.Contains(x.KittingDetailId))
            .ToListAsync();
    }

    private async Task UpdateIssueDetailsSupplyStatusAsync(List<int> detailIds, string supplyStatus)
    {
        if (detailIds.Count == 0)
        {
            return;
        }

        var issueDetails = await _context.KittingIssueDetails
            .Where(x => detailIds.Contains(x.KittingDetailId))
            .ToListAsync();

        if (issueDetails.Count == 0)
        {
            return;
        }

        var normalizedSupplyStatus = NormalizeStatus(supplyStatus);
        foreach (var issueDetail in issueDetails)
        {
            issueDetail.SupplyStatus = normalizedSupplyStatus;
            issueDetail.LastModifiedAt = DateTime.Now;
            issueDetail.LastModifiedByUserId = CurrentUserId;
        }
    }

    private static string GetKittingDetailLabel(KittingDetail detail)
    {
        return string.IsNullOrWhiteSpace(detail.PartNumber)
            ? detail.KittingDetailId.ToString()
            : detail.PartNumber;
    }

    private static string GetStatusValidationActionLabel(string targetStatus)
    {
        if (IsSurtidoStatus(targetStatus))
            return "surtir";

        return IsConfirmedStatus(targetStatus)
            ? "confirmar"
            : $"marcar como {targetStatus}";
    }

    private static bool IsTerminalStatus(string? status) =>
        IsConfirmedStatus(status) || IsSurtidoStatus(status) || IsCancelledStatus(status);

    private static bool IsConfirmedStatus(string? status) =>
        string.Equals(status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase);

    private static bool IsSurtidoStatus(string? status) =>
        string.Equals(status?.Trim(), "Surtido", StringComparison.OrdinalIgnoreCase);

    private static bool IsCancelledStatus(string? status) =>
        string.Equals(status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase);

    private static string? NormalizeStatus(string? status) =>
        string.IsNullOrWhiteSpace(status)
            ? null
            : status.Trim();
}
