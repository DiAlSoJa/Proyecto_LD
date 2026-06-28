using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Constants;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.User;
using MediatR;

namespace LD.Application.Features.Lookup.Queries;

public record GetCycleCountAuditorLookupQuery(string UserId)
    : IRequest<Result<List<DropDownDto>>>;

public class GetCycleCountAuditorLookupQueryHandler
    : IRequestHandler<GetCycleCountAuditorLookupQuery, Result<List<DropDownDto>>>
{
    private readonly IApplicationUserManager _userManager;
    private readonly IWarehouseRepository _warehouseRepository;

    public GetCycleCountAuditorLookupQueryHandler(
        IApplicationUserManager userManager,
        IWarehouseRepository warehouseRepository)
    {
        _userManager = userManager;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Result<List<DropDownDto>>> Handle(
        GetCycleCountAuditorLookupQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
            return Result<List<DropDownDto>>.Failure("Usuario invalido", new());

        var warehouses = await _warehouseRepository.GetLookupByUserId(request.UserId);
        var warehouseIds = warehouses
            .Select(x => int.TryParse(x.Key, out var warehouseId) ? warehouseId : 0)
            .Where(x => x > 0)
            .ToHashSet();

        if (warehouseIds.Count == 0)
            return Result<List<DropDownDto>>.Success([], "Auditores obtenidos con exito");

        var users = await _userManager.GetUsersAsync();
        var auditors = users
            .Where(x => x.User?.Activo == true)
            .Where(HasCycleCountPermission)
            .Where(x => x.Warehouse?.Any(w => warehouseIds.Contains(w.Id)) == true)
            .OrderBy(x => x.User?.Nombre ?? x.User?.UserName)
            .Select(x => new DropDownDto
            {
                Key = x.User?.Id,
                Value = string.IsNullOrWhiteSpace(x.User?.Nombre)
                    ? x.User?.UserName
                    : x.User?.Nombre,
                Description = string.IsNullOrWhiteSpace(x.User?.UserName)
                    ? x.User?.Nombre
                    : x.User?.UserName
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value))
            .ToList();

        return Result<List<DropDownDto>>.Success(auditors, "Auditores obtenidos con exito");
    }

    private static bool HasCycleCountPermission(GetUserDto user)
    {
        return user.Permissions?.Any(permission =>
            string.Equals(permission.PermissionKey, PermissionKeys.CycleCount_View, StringComparison.OrdinalIgnoreCase)) == true;
    }
}
