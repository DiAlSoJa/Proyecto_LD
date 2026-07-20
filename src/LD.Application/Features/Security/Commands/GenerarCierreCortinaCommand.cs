using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class GenerarCierreCortinaCommand : IRequest<Result<string>>
{
    public int SecurityRegistrationId { get; set; }
}

public class GenerarCierreCortinaCommandHandler : IRequestHandler<GenerarCierreCortinaCommand, Result<string>>
{
    private static readonly string[] TiposFinalizarOperacion = new[] { "FinalizarOperacion", "TerminarOperacion" };
    private static readonly string[] TiposCerrarRegistro = new[] { "CerrarRegistro" };

    private readonly IRepository<SecurityRegistration> _registroRepo;
    private readonly IRepository<SecurityTask> _taskRepo;

    public GenerarCierreCortinaCommandHandler(
        IRepository<SecurityRegistration> registroRepo,
        IRepository<SecurityTask> taskRepo)
    {
        _registroRepo = registroRepo;
        _taskRepo = taskRepo;
    }

    public async Task<Result<string>> Handle(GenerarCierreCortinaCommand request, CancellationToken cancellationToken)
    {
        var registro = await _registroRepo.GetByIdAsync(request.SecurityRegistrationId);
        if (registro is null)
            return Result<string>.Failure("Registro no encontrado", []);

        var tareas = await _taskRepo.GetManyAsync() ?? [];

        var operacionFinalizada = tareas
            .Any(t => t.SecurityRegistrationId == request.SecurityRegistrationId
                   && TiposFinalizarOperacion.Contains(t.TipoAccion)
                   && t.Completada);

        if (!operacionFinalizada)
            return Result<string>.Failure("Primero debes finalizar la operación", []);

        var tareaExistente = tareas
            .Any(t => t.SecurityRegistrationId == request.SecurityRegistrationId
                   && TiposCerrarRegistro.Contains(t.TipoAccion)
                   && !t.Completada);

        if (tareaExistente)
            return Result<string>.Success(registro.SecurityRegistrationId.ToString(), "La tarea de cierre ya fue generada");

        var tarea = new SecurityTask
        {
            SecurityRegistrationId = registro.SecurityRegistrationId,
            FechaIniciada = DateTime.UtcNow,
            TipoAccion = "CerrarRegistro",
            Completada = false
        };
        await _taskRepo.CreateAsync(tarea);

        registro.Estado = RegistroEstado.PendienteDeCerrar;
        await _registroRepo.UpdateAsync(registro);

        return Result<string>.Success(registro.SecurityRegistrationId.ToString(), "Tarea de cierre generada correctamente");
    }
}
