using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class DeliveryOrderController : CommonController
{
    private readonly LdProyectDbContext _context;

    public DeliveryOrderController(LdProyectDbContext context)
    {
        _context = context;
    }

    [HttpPost("from-kittings")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> CreateFromKittings([FromBody] CreateDeliveryOrderRequest request)
    {
        try
        {
            var kittingIds = request.KittingIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (kittingIds.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar al menos un kitting.", new List<string> { "Debes seleccionar al menos un kitting." }));
            }

            var kittings = await _context.Kittings
                .Include(x => x.Project)
                .Include(x => x.Client)
                .Where(x => kittingIds.Contains(x.KittingId))
                .ToListAsync();

            if (kittings.Count != kittingIds.Count)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Uno o más kittings no existen.", new List<string> { "Uno o más kittings no existen." }));
            }

            var orderByRequest = kittings.ToDictionary(x => x.KittingId);
            var orderedKittings = new List<Kitting>();
            foreach (var kittingId in kittingIds)
            {
                if (orderByRequest.TryGetValue(kittingId, out var kitting))
                    orderedKittings.Add(kitting);
            }

            var first = orderedKittings.First();
            if (orderedKittings.Any(x => x.ProjectId != first.ProjectId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo proyecto.", new List<string> { "Todos los kittings deben pertenecer al mismo proyecto." }));
            }

            if (orderedKittings.Any(x => x.ClientId != first.ClientId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo cliente.", new List<string> { "Todos los kittings deben pertenecer al mismo cliente." }));
            }

            var project = first.Project;
            if (project is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontró el proyecto del kitting.", new List<string> { "No se encontró el proyecto del kitting." }));
            }

            var prefix = GetDeliveryOrderPrefix(project);
            if (string.IsNullOrWhiteSpace(prefix))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("El proyecto no tiene configurado prefijo de orden de entrega.", new List<string> { "El proyecto no tiene configurado prefijo de orden de entrega." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var currentNumber = GetDeliveryOrderNumber(project);
                var deliveryOrderCode = $"{prefix}{currentNumber:D5}";

                var deliveryOrder = new DeliveryOrder
                {
                    ClientId = first.ClientId,
                    ProjectId = first.ProjectId,
                    DeliveryOrderCode = deliveryOrderCode,
                    PreDeliveryOrderCode = deliveryOrderCode,
                    Status = "Cargado"
                };

                _context.DeliveryOrders.Add(deliveryOrder);
                await _context.SaveChangesAsync();

                var existingRelations = await _context.DeliveryOrderKittings
                    .Where(x => kittingIds.Contains(x.KittingId))
                    .Select(x => x.KittingId)
                    .ToListAsync();

                if (existingRelations.Count > 0)
                {
                    throw new Exception("Uno o más kittings ya fueron cargados en una orden de entrega.");
                }

                var orderKittings = orderedKittings
                    .Select((kitting, index) => new DeliveryOrderKitting
                    {
                        DeliveryOrderId = deliveryOrder.DeliveryOrderId,
                        KittingId = kitting.KittingId,
                        SortOrder = index + 1
                    })
                    .ToList();

                _context.DeliveryOrderKittings.AddRange(orderKittings);
                await AssignDeliveryOrderToIssueDetailsAsync(kittingIds, deliveryOrder.DeliveryOrderId);

                SetDeliveryOrderNumber(project, currentNumber + 1);
                _context.Projects.Update(project);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    result > 0
                        ? Result<string>.Success(deliveryOrder.DeliveryOrderCode ?? deliveryOrder.DeliveryOrderId.ToString(), "Orden de entrega creada correctamente.")
                        : Result<string>.Failure("No se pudo crear la orden de entrega.", new List<string> { "No se pudo crear la orden de entrega." }));
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
                Result<string>.Failure("Hubo un error al crear la orden de entrega.", new List<string> { ex.Message }));
        }
    }

    [HttpPost("add-kittings")]
    [Permission(PermissionKeys.Shipment_View)]
    public async Task<IActionResult> AddKittingsToExistingOrder([FromBody] AddKittingsToDeliveryOrderRequest request)
    {
        try
        {
            var deliveryOrderCode = request.DeliveryOrderCode?.Trim();
            if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar una orden de entrega.", new List<string> { "Debes seleccionar una orden de entrega." }));
            }

            var kittingIds = request.KittingIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            if (kittingIds.Count == 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes seleccionar al menos un kitting.", new List<string> { "Debes seleccionar al menos un kitting." }));
            }

            var deliveryOrder = await _context.DeliveryOrders
                .Include(x => x.Client)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.DeliveryOrderCode == deliveryOrderCode ||
                    x.PreDeliveryOrderCode == deliveryOrderCode);

            if (deliveryOrder is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontro la orden de entrega seleccionada.", new List<string> { "No se encontro la orden de entrega seleccionada." }));
            }

            var kittings = await _context.Kittings
                .Include(x => x.Project)
                .Include(x => x.Client)
                .Where(x => kittingIds.Contains(x.KittingId))
                .ToListAsync();

            if (kittings.Count != kittingIds.Count)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Uno o mas kittings no existen.", new List<string> { "Uno o mas kittings no existen." }));
            }

            var orderByRequest = kittings.ToDictionary(x => x.KittingId);
            var orderedKittings = new List<Kitting>();
            foreach (var kittingId in kittingIds)
            {
                if (orderByRequest.TryGetValue(kittingId, out var kitting))
                    orderedKittings.Add(kitting);
            }

            var first = orderedKittings.First();
            if (orderedKittings.Any(x => x.ProjectId != first.ProjectId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo proyecto.", new List<string> { "Todos los kittings deben pertenecer al mismo proyecto." }));
            }

            if (orderedKittings.Any(x => x.ClientId != first.ClientId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Todos los kittings deben pertenecer al mismo cliente.", new List<string> { "Todos los kittings deben pertenecer al mismo cliente." }));
            }

            if (deliveryOrder.ProjectId != first.ProjectId || deliveryOrder.ClientId != first.ClientId)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("La orden de entrega seleccionada no coincide con el cliente y proyecto de los kittings.", new List<string> { "La orden de entrega seleccionada no coincide con el cliente y proyecto de los kittings." }));
            }

            var existingRelations = await _context.DeliveryOrderKittings
                .AsNoTracking()
                .Where(x => kittingIds.Contains(x.KittingId))
                .Select(x => new { x.KittingId, x.DeliveryOrderId })
                .ToListAsync();

            if (existingRelations.Count > 0)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Uno o mas kittings ya estan relacionados con una orden de entrega.", new List<string> { "Uno o mas kittings ya estan relacionados con una orden de entrega." }));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var currentMaxSortOrder = await _context.DeliveryOrderKittings
                    .Where(x => x.DeliveryOrderId == deliveryOrder.DeliveryOrderId)
                    .Select(x => (int?)x.SortOrder)
                    .MaxAsync() ?? 0;

                var orderKittings = orderedKittings
                    .Select((kitting, index) => new DeliveryOrderKitting
                    {
                        DeliveryOrderId = deliveryOrder.DeliveryOrderId,
                        KittingId = kitting.KittingId,
                        SortOrder = currentMaxSortOrder + index + 1
                    })
                    .ToList();

                _context.DeliveryOrderKittings.AddRange(orderKittings);
                await AssignDeliveryOrderToIssueDetailsAsync(kittingIds, deliveryOrder.DeliveryOrderId);

                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultExtensions.ToActionResult(
                    result > 0
                        ? Result<string>.Success(deliveryOrder.DeliveryOrderCode ?? deliveryOrder.PreDeliveryOrderCode ?? deliveryOrder.DeliveryOrderId.ToString(), "Kittings agregados correctamente a la orden de entrega.")
                        : Result<string>.Failure("No se pudieron agregar los kittings a la orden de entrega.", new List<string> { "No se pudieron agregar los kittings a la orden de entrega." }));
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
                Result<string>.Failure("Hubo un error al agregar los kittings a la orden de entrega.", new List<string> { ex.Message }));
        }
    }

    private async Task AssignDeliveryOrderToIssueDetailsAsync(List<int> kittingIds, int deliveryOrderId)
    {
        if (kittingIds.Count == 0 || deliveryOrderId <= 0)
            return;

        var kittingDetailIds = await _context.KittingDetails
            .AsNoTracking()
            .Where(x => kittingIds.Contains(x.KittingId))
            .Select(x => x.KittingDetailId)
            .ToListAsync();

        if (kittingDetailIds.Count == 0)
            return;

        var issueDetails = await _context.KittingIssueDetails
            .Where(x => kittingDetailIds.Contains(x.KittingDetailId))
            .ToListAsync();

        foreach (var issueDetail in issueDetails)
        {
            issueDetail.DeliveryOrderId = deliveryOrderId;
        }
    }

    private static string GetDeliveryOrderPrefix(Project project)
    {
        if (!string.IsNullOrWhiteSpace(project.DoPrefix))
            return project.DoPrefix.Trim();

        if (!string.IsNullOrWhiteSpace(project.DeliveryOrderPrefix))
            return project.DeliveryOrderPrefix.Trim();

        return string.Empty;
    }

    private static int GetDeliveryOrderNumber(Project project)
    {
        if (!string.IsNullOrWhiteSpace(project.DoNumber) &&
            int.TryParse(project.DoNumber, out var doNumber))
        {
            return doNumber;
        }

        if (!string.IsNullOrWhiteSpace(project.DeliveryOrderNumber) &&
            int.TryParse(project.DeliveryOrderNumber, out var deliveryOrderNumber))
        {
            return deliveryOrderNumber;
        }

        return 1;
    }

    private static void SetDeliveryOrderNumber(Project project, int nextNumber)
    {
        var value = nextNumber.ToString();
        project.DoNumber = value;
        project.DeliveryOrderNumber = value;
    }
}
