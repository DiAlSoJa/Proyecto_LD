using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class CreateSecurityRegistrationCommand : SecurityRegistrationRequest, IRequest<Result<string>>
{
}

public class CreateSecurityRegistrationCommandHandler : IRequestHandler<CreateSecurityRegistrationCommand, Result<string>>
{
    private readonly IRepository<SecurityRegistration> _repository;
    private readonly IFileStorageService _fileStorage;
    private readonly IMapper _mapper;

    public CreateSecurityRegistrationCommandHandler(
        IRepository<SecurityRegistration> repository,
        IFileStorageService fileStorage,
        IMapper mapper)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateSecurityRegistrationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Guardar fotos en disco — las rutas van al entity
            var licFoto1 = await _fileStorage.SaveAsync(request.LicenciaFoto1, "licencias", "lic1");
            var licFoto2 = await _fileStorage.SaveAsync(request.LicenciaFoto2, "licencias", "lic2");
            var vehFoto1 = await _fileStorage.SaveAsync(request.VehiculoFoto1, "vehiculos", "veh1");
            var vehFoto2 = await _fileStorage.SaveAsync(request.VehiculoFoto2, "vehiculos", "veh2");
            var firma    = await _fileStorage.SaveAsync(request.Firma,          "firmas",    "firma");

            var entity = _mapper.Map<SecurityRegistration>(request);
            entity.LicenciaFoto1 = licFoto1;
            entity.LicenciaFoto2 = licFoto2;
            entity.VehiculoFoto1 = vehFoto1;
            entity.VehiculoFoto2 = vehFoto2;
            entity.Firma         = firma;

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
