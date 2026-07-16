using LD.Api.Common.Results;
using LD.Api.Authorization;
using LD.Api.Controllers.Common;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.LoadMapping;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class LoadMappingController : CommonController
{
    private readonly LdProyectDbContext _context;

    public LoadMappingController(LdProyectDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<List<LoadMappingDto>>.Failure("No se pudo identificar al usuario actual.", new List<string> { "No se pudo identificar al usuario actual." }));
            }

            var mappings = await _context.LoadMappings
                .AsNoTracking()
                .Include(mapping => mapping.Client)
                .Include(mapping => mapping.Project)
                .Where(mapping => _context.Projects.Any(project =>
                    project.ProjectId == mapping.ProjectId &&
                    _context.UserWarehouses.Any(userWarehouse =>
                        userWarehouse.UserId == CurrentUserId &&
                        userWarehouse.WarehouseId == project.WarehouseId)))
                .Where(mapping => _context.DeliveryOrderKittings
                    .AsNoTracking()
                    .Any(orderKitting =>
                        orderKitting.Kitting != null &&
                        (orderKitting.Kitting.Status == KittingStatusNames.Cargando ||
                         orderKitting.Kitting.Status == KittingStatusNames.LegacyCargando) &&
                        orderKitting.DeliveryOrder != null &&
                        orderKitting.DeliveryOrder.ClientId == mapping.ClientId &&
                        orderKitting.DeliveryOrder.ProjectId == mapping.ProjectId &&
                        ((orderKitting.DeliveryOrder.DeliveryOrderCode ?? orderKitting.DeliveryOrder.PreDeliveryOrderCode) ?? string.Empty) ==
                        (mapping.DeliveryOrderCode ?? string.Empty)))
                .Select(mapping => new LoadMappingDto
                {
                    MapeoCargaId = mapping.LoadMappingId,
                    Cliente = mapping.Client != null ? mapping.Client.CommercialName ?? string.Empty : string.Empty,
                    Proyecto = mapping.Project != null ? mapping.Project.ProjectName ?? string.Empty : string.Empty,
                    OrdenEntrega = mapping.DeliveryOrderCode ?? string.Empty
                })
                .OrderBy(mapping => mapping.Cliente)
                .ThenBy(mapping => mapping.Proyecto)
                .ThenBy(mapping => mapping.OrdenEntrega)
                .ToListAsync();

            return ResultExtensions.ToActionResult(
                Result<List<LoadMappingDto>>.Success(mappings, "Mapeos obtenidos correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<List<LoadMappingDto>>.Failure("Hubo un error al consultar los mapeos.", new List<string> { ex.Message }));
        }
    }

    [HttpGet("loading-orders")]
    public async Task<IActionResult> GetLoadingOrders()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<List<LoadMappingAvailableOrderDto>>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." }));
            }

            var loadingOrders = await _context.DeliveryOrderKittings
                .AsNoTracking()
                .Include(x => x.DeliveryOrder)
                    .ThenInclude(x => x.Client)
                .Include(x => x.DeliveryOrder)
                    .ThenInclude(x => x.Project)
                    .ThenInclude(x => x.Warehouse)
                .Where(x =>
                    x.Kitting != null &&
                    (x.Kitting.Status == KittingStatusNames.Cargando || x.Kitting.Status == KittingStatusNames.LegacyCargando) &&
                    x.DeliveryOrder != null &&
                    _context.Projects.Any(project =>
                        project.ProjectId == x.DeliveryOrder.ProjectId &&
                        _context.UserWarehouses.Any(userWarehouse =>
                            userWarehouse.UserId == CurrentUserId &&
                            userWarehouse.WarehouseId == project.WarehouseId)))
                .Select(x => new
                {
                    x.DeliveryOrderId,
                    DeliveryOrderCode = x.DeliveryOrder != null
                        ? x.DeliveryOrder.DeliveryOrderCode ?? x.DeliveryOrder.PreDeliveryOrderCode ?? string.Empty
                        : string.Empty,
                    ClientId = x.DeliveryOrder != null ? x.DeliveryOrder.ClientId : 0,
                    Client = x.DeliveryOrder != null && x.DeliveryOrder.Client != null
                        ? x.DeliveryOrder.Client.CommercialName ?? string.Empty
                        : string.Empty,
                    ProjectId = x.DeliveryOrder != null ? x.DeliveryOrder.ProjectId : 0,
                    Project = x.DeliveryOrder != null && x.DeliveryOrder.Project != null
                        ? x.DeliveryOrder.Project.ProjectName ?? string.Empty
                        : string.Empty,
                    WarehouseId = x.DeliveryOrder != null && x.DeliveryOrder.Project != null
                        ? x.DeliveryOrder.Project.WarehouseId
                        : 0,
                    Warehouse = x.DeliveryOrder != null &&
                                x.DeliveryOrder.Project != null &&
                                x.DeliveryOrder.Project.Warehouse != null
                        ? x.DeliveryOrder.Project.Warehouse.WarehouseName ?? string.Empty
                        : string.Empty,
                    Status = x.Kitting != null ? x.Kitting.Status ?? string.Empty : string.Empty
                })
                .ToListAsync();

            var orders = loadingOrders
                .Where(x => !string.IsNullOrWhiteSpace(x.DeliveryOrderCode))
                .GroupBy(x => x.DeliveryOrderId)
                .Select(group =>
                {
                    var first = group.First();

                    return new LoadMappingAvailableOrderDto
                    {
                        ClientId = first.ClientId,
                        ProjectId = first.ProjectId,
                        WarehouseId = first.WarehouseId,
                        Cliente = first.Client,
                        Proyecto = first.Project,
                        Almacen = first.Warehouse,
                        OrdenEntrega = first.DeliveryOrderCode,
                        Status = KittingStatusNames.Display(first.Status),
                        KittingsCount = group.Count()
                    };
                })
                .OrderBy(x => x.OrdenEntrega)
                .ThenBy(x => x.Cliente)
                .ThenBy(x => x.Proyecto)
                .ToList();

            return ResultExtensions.ToActionResult(
                Result<List<LoadMappingAvailableOrderDto>>.Success(orders, "Ordenes en carga obtenidas correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<List<LoadMappingAvailableOrderDto>>.Failure(
                    "Hubo un error al consultar las ordenes en carga.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpGet("{loadMappingId:int}/scans")]
    [Permission(PermissionKeys.Auditing_View)]
    public async Task<IActionResult> GetScans(int loadMappingId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<List<LoadMappingScanDto>>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            if (!await CanAccessLoadMappingAsync(loadMappingId))
            {
                return ResultExtensions.ToActionResult(
                    Result<List<LoadMappingScanDto>>.Failure(
                        "El mapeo de carga no esta disponible para el usuario.",
                        new List<string> { "El mapeo de carga no esta disponible para el usuario." },
                        404));
            }

            var scans = await _context.LoadMappingScans
                .AsNoTracking()
                .Where(x => x.LoadMappingId == loadMappingId && x.IsActive)
                .OrderBy(x => x.ScannedAt)
                .ThenBy(x => x.LoadMappingScanId)
                .Select(x => new LoadMappingScanDto
                {
                    LoadMappingScanId = x.LoadMappingScanId,
                    LoadMappingId = x.LoadMappingId,
                    KittingReceiptDetailId = x.KittingReceiptDetailId,
                    Side = x.Side,
                    StandardId = x.StandardId,
                    Result = x.Result,
                    IsSuccess = x.IsSuccess,
                    Kitting = x.Kitting,
                    PartNumber = x.PartNumber,
                    Description = x.Description,
                    Quantity = x.Quantity,
                    LotNumber = x.LotNumber,
                    Message = x.Message,
                    ScannedAt = x.ScannedAt
                })
                .ToListAsync();

            return ResultExtensions.ToActionResult(
                Result<List<LoadMappingScanDto>>.Success(scans, "Escaneos obtenidos correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<List<LoadMappingScanDto>>.Failure(
                    "Hubo un error al consultar los escaneos del mapeo.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPost("{loadMappingId:int}/scans")]
    [Permission(PermissionKeys.Auditing_View)]
    public async Task<IActionResult> CreateScan(int loadMappingId, [FromBody] CreateLoadMappingScanRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<LoadMappingScanDto>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            if (!await CanAccessLoadMappingAsync(loadMappingId))
            {
                return ResultExtensions.ToActionResult(
                    Result<LoadMappingScanDto>.Failure(
                        "El mapeo de carga no esta disponible para el usuario.",
                        new List<string> { "El mapeo de carga no esta disponible para el usuario." },
                        404));
            }

            var standardId = request.StandardId?.Trim();
            if (string.IsNullOrWhiteSpace(standardId))
            {
                return ResultExtensions.ToActionResult(
                    Result<LoadMappingScanDto>.Failure(
                        "StandardId es obligatorio.",
                        new List<string> { "StandardId es obligatorio." }));
            }

            var scan = new LoadMappingScan
            {
                LoadMappingId = loadMappingId,
                KittingReceiptDetailId = request.KittingReceiptDetailId > 0 ? request.KittingReceiptDetailId : null,
                Side = Truncate(string.IsNullOrWhiteSpace(request.Side) ? "Izquierda" : request.Side, 20),
                StandardId = Truncate(standardId, 100),
                Result = Truncate(string.IsNullOrWhiteSpace(request.Result)
                    ? request.IsSuccess ? "OK" : "ERROR"
                    : request.Result, 20),
                IsSuccess = request.IsSuccess,
                Kitting = Truncate(request.Kitting, 100),
                PartNumber = Truncate(request.PartNumber, 100),
                Description = Truncate(request.Description, 250),
                Quantity = request.Quantity,
                LotNumber = Truncate(request.LotNumber, 50),
                Message = Truncate(request.Message, 500),
                ScannedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                CreatedByUserId = CurrentUserId,
                IsActive = true
            };

            _context.LoadMappingScans.Add(scan);
            var saved = await _context.SaveChangesAsync() > 0;

            if (!saved)
            {
                return ResultExtensions.ToActionResult(
                    Result<LoadMappingScanDto>.Failure(
                        "No se pudo guardar el escaneo.",
                        new List<string> { "No se pudo guardar el escaneo." }));
            }

            return ResultExtensions.ToActionResult(
                Result<LoadMappingScanDto>.Success(ToDto(scan), "Escaneo guardado correctamente."));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<LoadMappingScanDto>.Failure(
                    "Hubo un error al guardar el escaneo.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpDelete("{loadMappingId:int}/scans/{scanId:int}")]
    [Permission(PermissionKeys.LoadMappingScan_Delete)]
    public async Task<IActionResult> DeleteScan(int loadMappingId, int scanId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "No se pudo identificar al usuario actual.",
                        new List<string> { "No se pudo identificar al usuario actual." },
                        401));
            }

            var scan = await _context.LoadMappingScans
                .FirstOrDefaultAsync(x =>
                    x.LoadMappingScanId == scanId &&
                    x.LoadMappingId == loadMappingId &&
                    x.IsActive);

            if (scan is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure(
                        "No se encontro el escaneo activo para retirar.",
                        new List<string> { "No se encontro el escaneo activo para retirar." },
                        404));
            }

            scan.IsActive = false;
            scan.DeletedAt = DateTime.Now;
            scan.DeletedByUserId = CurrentUserId;
            scan.LastModifiedAt = DateTime.Now;
            scan.LastModifiedByUserId = CurrentUserId;

            var saved = await _context.SaveChangesAsync() > 0;

            return ResultExtensions.ToActionResult(
                saved
                    ? Result<string>.Success("OK", "Escaneo retirado correctamente.")
                    : Result<string>.Failure("No se pudo retirar el escaneo.", new List<string> { "No se pudo retirar el escaneo." }));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure(
                    "Hubo un error al retirar el escaneo.",
                    new List<string> { ex.Message }));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoadMappingRequest request)
    {
        try
        {
            var deliveryOrderCode = request.DeliveryOrderCode?.Trim();
            if (request.ClientId <= 0 || request.ProjectId <= 0 || string.IsNullOrWhiteSpace(deliveryOrderCode))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Debes completar cliente, proyecto y orden de entrega.", new List<string> { "Debes completar cliente, proyecto y orden de entrega." }));
            }

            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se pudo identificar al usuario actual.", new List<string> { "No se pudo identificar al usuario actual." }));
            }

            var project = await _context.Projects
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x =>
                    x.ClientId == request.ClientId &&
                    x.ProjectId == request.ProjectId &&
                    _context.UserWarehouses.Any(userWarehouse =>
                        userWarehouse.UserId == CurrentUserId &&
                        userWarehouse.WarehouseId == x.WarehouseId));

            if (project is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("El cliente y proyecto no están disponibles para los almacenes del usuario.", new List<string> { "El cliente y proyecto no están disponibles para los almacenes del usuario." }));
            }

            var client = project.Client;
            if (client is null)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("No se encontró el cliente seleccionado.", new List<string> { "No se encontró el cliente seleccionado." }));
            }

            var alreadyExists = await _context.LoadMappings.AnyAsync(x =>
                x.ClientId == request.ClientId &&
                x.ProjectId == request.ProjectId &&
                x.DeliveryOrderCode == deliveryOrderCode);

            if (alreadyExists)
            {
                return ResultExtensions.ToActionResult(
                    Result<string>.Failure("Ese mapeo de carga ya existe.", new List<string> { "Ese mapeo de carga ya existe." }));
            }

            var mapping = new LoadMapping
            {
                ClientId = request.ClientId,
                ProjectId = request.ProjectId,
                DeliveryOrderCode = deliveryOrderCode,
                CreatedAt = DateTime.Now,
                CreatedByUserId = CurrentUserId,
                IsActive = true
            };

            _context.LoadMappings.Add(mapping);
            var result = await _context.SaveChangesAsync();

            return ResultExtensions.ToActionResult(
                result > 0
                    ? Result<string>.Success(mapping.LoadMappingId.ToString(), "Mapeo de carga creado correctamente.")
                    : Result<string>.Failure("No se pudo crear el mapeo de carga.", new List<string> { "No se pudo crear el mapeo de carga." }));
        }
        catch (Exception ex)
        {
            return ResultExtensions.ToActionResult(
                Result<string>.Failure("Hubo un error al crear el mapeo de carga.", new List<string> { ex.Message }));
        }
    }

    private async Task<bool> CanAccessLoadMappingAsync(int loadMappingId)
    {
        if (loadMappingId <= 0 || string.IsNullOrWhiteSpace(CurrentUserId))
            return false;

        return await _context.LoadMappings.AnyAsync(mapping =>
            mapping.LoadMappingId == loadMappingId &&
            _context.Projects.Any(project =>
                project.ProjectId == mapping.ProjectId &&
                _context.UserWarehouses.Any(userWarehouse =>
                    userWarehouse.UserId == CurrentUserId &&
                    userWarehouse.WarehouseId == project.WarehouseId)));
    }

    private static LoadMappingScanDto ToDto(LoadMappingScan scan)
    {
        return new LoadMappingScanDto
        {
            LoadMappingScanId = scan.LoadMappingScanId,
            LoadMappingId = scan.LoadMappingId,
            KittingReceiptDetailId = scan.KittingReceiptDetailId,
            Side = scan.Side,
            StandardId = scan.StandardId,
            Result = scan.Result,
            IsSuccess = scan.IsSuccess,
            Kitting = scan.Kitting,
            PartNumber = scan.PartNumber,
            Description = scan.Description,
            Quantity = scan.Quantity,
            LotNumber = scan.LotNumber,
            Message = scan.Message,
            ScannedAt = scan.ScannedAt
        };
    }

    private static string Truncate(string? value, int maxLength)
    {
        var normalized = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        return normalized.Length <= maxLength
            ? normalized
            : normalized[..maxLength];
    }
}
