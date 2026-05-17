using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class AbrirCortinaCommand : IRequest<Result<string>>
{
    public int SecurityTaskId { get; set; }
    public string? RealizadaPor { get; set; }
    public string FotoBase64 { get; set; } = string.Empty;
}

public class AbrirCortinaCommandHandler : IRequestHandler<AbrirCortinaCommand, Result<string>>
{
    private readonly IRepository<SecurityTask> _taskRepo;
    private readonly IRepository<SecurityRegistration> _registroRepo;

    public AbrirCortinaCommandHandler(
        IRepository<SecurityTask> taskRepo,
        IRepository<SecurityRegistration> registroRepo)
    {
        _taskRepo     = taskRepo;
        _registroRepo = registroRepo;
    }

    public async Task<Result<string>> Handle(AbrirCortinaCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FotoBase64))
            return Result<string>.Failure("La foto es obligatoria para abrir cortina", []);

        var tarea = await _taskRepo.GetByIdAsync(request.SecurityTaskId);
        if (tarea is null || tarea.TipoAccion != "AbrirCortina")
            return Result<string>.Failure("Tarea no encontrada", []);

        if (tarea.Completada)
            return Result<string>.Failure("La tarea ya fue completada", []);

        tarea.Completada      = true;
        tarea.FechaCompletada = DateTime.UtcNow;
        tarea.RealizadaPor    = request.RealizadaPor;
        await _taskRepo.UpdateAsync(tarea);

        var registro = await _registroRepo.GetByIdAsync(tarea.SecurityRegistrationId);
        if (registro is not null)
        {
            registro.Estado = RegistroEstado.PendienteDeCerrar;
            await _registroRepo.UpdateAsync(registro);

            var tareaClose = new SecurityTask
            {
                SecurityRegistrationId = registro.SecurityRegistrationId,
                TipoAccion             = "CerrarRegistro",
                Completada             = false
            };
            await _taskRepo.CreateAsync(tareaClose);
        }

        return Result<string>.Success(tarea.SecurityTaskId.ToString(), "Cortina abierta — vehículo pendiente de cierre");
    }
}
