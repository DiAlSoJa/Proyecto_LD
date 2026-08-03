using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class AsignarCortinaCommand : IRequest<Result<string>>
{
    public int SecurityRegistrationId { get; set; }
    public int CortinaId { get; set; }
}

public class AsignarCortinaCommandHandler : IRequestHandler<AsignarCortinaCommand, Result<string>>
{
    private readonly IRepository<SecurityRegistration> _registroRepo;
    private readonly IRepository<Cortina> _cortinaRepo;
    private readonly IRepository<SecurityTask> _taskRepo;

    public AsignarCortinaCommandHandler(
        IRepository<SecurityRegistration> registroRepo,
        IRepository<Cortina> cortinaRepo,
        IRepository<SecurityTask> taskRepo)
    {
        _registroRepo = registroRepo;
        _cortinaRepo  = cortinaRepo;
        _taskRepo     = taskRepo;
    }

    public async Task<Result<string>> Handle(AsignarCortinaCommand request, CancellationToken cancellationToken)
    {
        var registro = await _registroRepo.GetByIdAsync(request.SecurityRegistrationId);
        if (registro is null)
            return Result<string>.Failure("Registro no encontrado", []);

        if (registro.Estado != RegistroEstado.Registrado)
            return Result<string>.Failure("El registro ya tiene una cortina asignada o está cerrado", []);

        var cortina = await _cortinaRepo.GetByIdAsync(request.CortinaId);
        if (cortina is null)
            return Result<string>.Failure("Cortina no encontrada", []);

        if (registro.WarehouseId.HasValue && cortina.WarehouseId != registro.WarehouseId.Value)
            return Result<string>.Failure("La cortina seleccionada no pertenece al almacén del registro", []);

        if (!cortina.EstaDisponible)
            return Result<string>.Failure("La cortina seleccionada no está disponible", []);

        cortina.EstaDisponible = false;
        await _cortinaRepo.UpdateAsync(cortina);

        registro.CortinaId = request.CortinaId;
        registro.Estado    = RegistroEstado.CortinaAsignada;
        await _registroRepo.UpdateAsync(registro);

        var tarea = new SecurityTask
        {
            SecurityRegistrationId = registro.SecurityRegistrationId,
            FechaIniciada = DateTime.UtcNow,
            TipoAccion = "AbrirCortina",
            Completada = false
        };
        await _taskRepo.CreateAsync(tarea);

        return Result<string>.Success(registro.SecurityRegistrationId.ToString(), "Cortina asignada correctamente");
    }
}
