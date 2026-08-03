using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class CreateSecurityRegistrationCommand : SecurityRegistrationRequest, IRequest<Result<string>>
{
}

public class CreateSecurityRegistrationCommandHandler : IRequestHandler<CreateSecurityRegistrationCommand, Result<string>>
{
    private readonly IRepository<SecurityRegistration> _repository;
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IFileStorageService _fileStorage;
    private readonly IUserContextService _userContext;
    private readonly IMapper _mapper;

    public CreateSecurityRegistrationCommandHandler(
        IRepository<SecurityRegistration> repository,
        IWarehouseRepository warehouseRepository,
        IFileStorageService fileStorage,
        IUserContextService userContext,
        IMapper mapper)
    {
        _repository = repository;
        _warehouseRepository = warehouseRepository;
        _fileStorage = fileStorage;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateSecurityRegistrationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.WarehouseId.HasValue || request.WarehouseId.Value <= 0)
                return Result<string>.Failure("Selecciona un almacén para continuar.", new());

            var userId = _userContext.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                return Result<string>.Failure("No se pudo identificar al usuario actual.", new(), 401);

            var assignedWarehouses = await _warehouseRepository.GetLookupByUserId(userId);
            var hasAssignedWarehouse = assignedWarehouses.Any(item =>
                int.TryParse(item.Key, out var warehouseId) &&
                warehouseId == request.WarehouseId.Value);

            if (!hasAssignedWarehouse)
            {
                return Result<string>.Failure(
                    "El almacén seleccionado no está asignado al usuario actual.",
                    new());
            }

            var entity = _mapper.Map<SecurityRegistration>(request);

            foreach (var foto in request.Fotos)
            {
                if (foto.Contenido is null || foto.Contenido.Length == 0) continue;

                var subfolder = foto.Categoria.ToString().ToLower();
                var prefix    = subfolder;
                var path      = await _fileStorage.SaveJpegAsync(foto.Contenido, subfolder, prefix);

                if (path is null) continue;

                entity.Photos.Add(new SecurityRegistrationPhoto
                {
                    Categoria = (PhotoCategoria)(int)foto.Categoria,
                    Orden     = foto.Orden,
                    FilePath  = path,
                });
            }

            var result = await _repository.CreateAsync(entity);
            return result
                ? Result<string>.Success(entity.SecurityRegistrationId.ToString(), "Registro de seguridad creado con éxito")
                : Result<string>.Failure("Error al guardar el registro", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Error al guardar el registro", new List<string> { ex.Message });
        }
    }
}
