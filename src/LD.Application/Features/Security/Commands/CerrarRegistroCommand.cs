using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class CerrarRegistroCommand : IRequest<Result<string>>
{
    public int SecurityTaskId { get; set; }
    public string? RealizadaPor { get; set; }
    public string FotoBase64 { get; set; } = string.Empty;
}

public class CerrarRegistroCommandHandler : IRequestHandler<CerrarRegistroCommand, Result<string>>
{
    private readonly IRepository<SecurityTask> _taskRepo;
    private readonly IRepository<SecurityRegistration> _registroRepo;
    private readonly IRepository<Cortina> _cortinaRepo;

    public CerrarRegistroCommandHandler(
        IRepository<SecurityTask> taskRepo,
        IRepository<SecurityRegistration> registroRepo,
        IRepository<Cortina> cortinaRepo)
    {
        _taskRepo     = taskRepo;
        _registroRepo = registroRepo;
        _cortinaRepo  = cortinaRepo;
    }

    public async Task<Result<string>> Handle(CerrarRegistroCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FotoBase64))
            return Result<string>.Failure("La foto es obligatoria para cerrar cortina", []);

        var tarea = await _taskRepo.GetByIdAsync(request.SecurityTaskId);
        if (tarea is null || tarea.TipoAccion != "CerrarRegistro")
            return Result<string>.Failure("Tarea de cierre no encontrada", []);

        if (tarea.Completada)
            return Result<string>.Failure("La tarea ya fue completada", []);

        tarea.Completada      = true;
        tarea.FechaCompletada = DateTime.UtcNow;
        tarea.RealizadaPor    = request.RealizadaPor;
        await _taskRepo.UpdateAsync(tarea);

        var registro = await _registroRepo.GetByIdAsync(tarea.SecurityRegistrationId);
        if (registro is not null)
        {
            // Liberar la cortina
            if (registro.CortinaId.HasValue)
            {
                var cortina = await _cortinaRepo.GetByIdAsync(registro.CortinaId.Value);
                if (cortina is not null)
                {
                    cortina.EstaDisponible = true;
                    await _cortinaRepo.UpdateAsync(cortina);
                }
            }

            registro.Estado   = RegistroEstado.Cerrado;
            registro.IsActive = false;
            await _registroRepo.UpdateAsync(registro);
        }

        return Result<string>.Success(tarea.SecurityTaskId.ToString(), "Registro cerrado — vehículo salió del patio");
    }
}
