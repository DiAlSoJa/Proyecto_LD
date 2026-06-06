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
public class KittingIssueController : CommonController
{
    private readonly LdProyectDbContext _context;
    private readonly IMapper _mapper;

    public KittingIssueController(LdProyectDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingIssue()
    {
        var entities = await _context.KittingIssueDetails
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .OrderByDescending(x => x.KittingReceiptDetailId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingIssueDetailDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingIssueDetailDto>?>.Success(dto, "Kitting Issue Details obtenidos correctamente"));
    }

    [HttpGet("{kittingIssueDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingIssueById(int kittingIssueDetailId)
    {
        var entity = await _context.KittingIssueDetails
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);

        if (entity is null)
        {
            return ResultExtensions.ToActionResult(
                Result<KittingIssueRequest?>.Failure("Kitting Issue Detail no encontrado.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
        }

        var dto = _mapper.Map<KittingIssueRequest>(entity);
        return ResultExtensions.ToActionResult(Result<KittingIssueRequest?>.Success(dto, "Kitting Issue Detail obtenido correctamente"));
    }

    [HttpGet("kittingDetail/{kittingDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> GetKittingIssueByKittingDetailId(int kittingDetailId)
    {
        var entities = await _context.KittingIssueDetails
            .AsNoTracking()
            .Include(x => x.Product)
            .Include(x => x.Location)
            .Include(x => x.StandardLabel)
            .Where(x => x.KittingDetailId == kittingDetailId)
            .OrderByDescending(x => x.KittingReceiptDetailId)
            .ToListAsync();

        var dto = _mapper.Map<List<KittingIssueDetailDto>>(entities);
        return ResultExtensions.ToActionResult(Result<List<KittingIssueDetailDto>>.Success(dto, "Kitting Issue Details obtenidos correctamente"));
    }

    [HttpPost]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateKittingIssue([FromBody] KittingIssueRequest request)
    {
        try
        {
            if (request.KittingDetailId <= 0)
                return ResultExtensions.ToActionResult(Result<string>.Failure("KittingDetailId es obligatorio.", new List<string> { "KittingDetailId es obligatorio." }));

            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return ResultExtensions.ToActionResult(Result<string>.Failure("PartNumber es obligatorio.", new List<string> { "PartNumber es obligatorio." }));

            var validation = await EnsureKittingDetailEditableAsync(request.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            var entity = _mapper.Map<KittingIssueDetail>(request);
            entity.Status = NormalizeStatus(entity.Status);

            _context.KittingIssueDetails.Add(entity);
            var result = await _context.SaveChangesAsync();

            return ResultExtensions.ToActionResult(
                result > 0
                    ? Result<string>.Success(entity.KittingReceiptDetailId.ToString(), "Kitting Issue Detail creado con exito")
                    : Result<string>.Failure("No se pudo guardar el Kitting Issue Detail.", new List<string> { "No se pudo guardar el Kitting Issue Detail." }));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al crear el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpPut("{kittingIssueDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> UpdateKittingIssue(int kittingIssueDetailId, [FromBody] KittingIssueRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return ResultExtensions.ToActionResult(Result<string>.Failure("PartNumber es obligatorio.", new List<string> { "PartNumber es obligatorio." }));

            var entity = await _context.KittingIssueDetails.FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);
            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Issue Detail.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
            }

            var validation = await EnsureKittingDetailEditableAsync(entity.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            request.KittingReceiptDetailId = entity.KittingReceiptDetailId;
            request.KittingDetailId = entity.KittingDetailId;
            request.Status = NormalizeStatus(request.Status) ?? NormalizeStatus(entity.Status);

            _mapper.Map(request, entity);
            var updated = await _context.SaveChangesAsync() > 0;

            if (!updated)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo actualizar el Kitting Issue Detail.", new List<string> { "No se pudo actualizar el Kitting Issue Detail." }));
            }

            return ResultExtensions.ToActionResult(
                Result<string>.Success(entity.KittingReceiptDetailId.ToString(), "Kitting Issue Detail actualizado"));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al actualizar el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    [HttpDelete("{kittingIssueDetailId}")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> DeleteKittingIssue(int kittingIssueDetailId)
    {
        try
        {
            var entity = await _context.KittingIssueDetails.FirstOrDefaultAsync(x => x.KittingReceiptDetailId == kittingIssueDetailId);
            if (entity is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No existe el Kitting Issue Detail.", new List<string> { "No existe el Kitting Issue Detail." }, 404));
            }

            var validation = await EnsureKittingDetailEditableAsync(entity.KittingDetailId);
            if (validation is not null)
                return ResultExtensions.ToActionResult(validation);

            _context.KittingIssueDetails.Remove(entity);
            var deleted = await _context.SaveChangesAsync() > 0;

            if (!deleted)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo eliminar el Kitting Issue Detail.", new List<string> { "No se pudo eliminar el Kitting Issue Detail." }));
            }

            return ResultExtensions.ToActionResult(
                Result<string>.Success(kittingIssueDetailId.ToString(), "Kitting Issue Detail eliminado"));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al eliminar el Kitting Issue Detail.", new List<string> { ex.Message }));
        }
    }

    private async Task<Result<string>?> EnsureKittingDetailEditableAsync(int kittingDetailId)
    {
        var detail = await _context.KittingDetails
            .AsNoTracking()
            .Include(x => x.Kitting)
            .FirstOrDefaultAsync(x => x.KittingDetailId == kittingDetailId);

        if (detail is null)
        {
            return Result<string>.Failure("Kitting Detail no encontrado.", new List<string> { "No existe el Kitting Detail." }, 404);
        }

        if (detail.Kitting is null)
        {
            return Result<string>.Failure("No se encontro el Kitting relacionado.", new List<string> { "No se encontro el Kitting relacionado." }, 404);
        }

        if (IsTerminalStatus(detail.Kitting.Status))
        {
            return Result<string>.Failure(
                "El Kitting ya esta finalizado y no se puede modificar.",
                new List<string> { "El Kitting ya esta finalizado." });
        }

        return null;
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
