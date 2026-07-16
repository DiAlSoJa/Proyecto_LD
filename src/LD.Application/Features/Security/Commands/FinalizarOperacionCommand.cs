using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Interfaces.Storage;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Commands;

public class FinalizarOperacionCommand : IRequest<Result<string>>
{
    public int SecurityTaskId { get; set; }
    public string? RealizadaPor { get; set; }
    public string FotoBase64 { get; set; } = string.Empty;
}

public class FinalizarOperacionCommandHandler : IRequestHandler<FinalizarOperacionCommand, Result<string>>
{
    private static readonly string[] TiposFinalizarOperacion = new[] { "FinalizarOperacion", "TerminarOperacion" };

    private readonly IRepository<SecurityTask> _taskRepo;
    private readonly IRepository<SecurityRegistration> _registroRepo;
    private readonly IRepository<SecurityRegistrationPhoto> _photoRepo;
    private readonly IFileStorageService _fileStorage;

    public FinalizarOperacionCommandHandler(
        IRepository<SecurityTask> taskRepo,
        IRepository<SecurityRegistration> registroRepo,
        IRepository<SecurityRegistrationPhoto> photoRepo,
        IFileStorageService fileStorage)
    {
        _taskRepo = taskRepo;
        _registroRepo = registroRepo;
        _photoRepo = photoRepo;
        _fileStorage = fileStorage;
    }

    public async Task<Result<string>> Handle(FinalizarOperacionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FotoBase64))
            return Result<string>.Failure("La foto es obligatoria para finalizar operaciÃ³n", []);

        var tarea = await _taskRepo.GetByIdAsync(request.SecurityTaskId);
        if (tarea is null)
            return Result<string>.Failure("Tarea de operaciÃ³n no encontrada", []);

        if (tarea.TipoAccion == "IniciarOperacion")
        {
            if (!tarea.Completada)
                return Result<string>.Failure("Primero debes iniciar la operaciÃ³n", []);

            tarea = await ObtenerOCrearTareaFinalizacionAsync(tarea.SecurityRegistrationId);
        }
        else if (!TiposFinalizarOperacion.Contains(tarea.TipoAccion))
        {
            return Result<string>.Failure("Tarea de operaciÃ³n no encontrada", []);
        }

        if (tarea.Completada)
            return Result<string>.Failure("La tarea ya fue completada", []);

        tarea.Completada = true;
        tarea.FechaCompletada = DateTime.UtcNow;
        tarea.RealizadaPor = request.RealizadaPor;
        await _taskRepo.UpdateAsync(tarea);

        var registro = await _registroRepo.GetByIdAsync(tarea.SecurityRegistrationId);
        if (registro is not null)
        {
            await GuardarFotoAsync(
                registro.SecurityRegistrationId,
                tarea.SecurityTaskId,
                request.FotoBase64,
                request.RealizadaPor,
                "finalizar");

            registro.Estado = RegistroEstado.PendienteDeCerrar;
            await _registroRepo.UpdateAsync(registro);
        }

        return Result<string>.Success(tarea.SecurityTaskId.ToString(), "OperaciÃ³n finalizada correctamente");
    }

    private async Task<SecurityTask> ObtenerOCrearTareaFinalizacionAsync(int securityRegistrationId)
    {
        var existente = (await _taskRepo.GetManyAsync() ?? [])
            .Where(t => t.SecurityRegistrationId == securityRegistrationId
                     && TiposFinalizarOperacion.Contains(t.TipoAccion))
            .OrderByDescending(t => t.FechaIniciada)
            .ThenByDescending(t => t.SecurityTaskId)
            .FirstOrDefault();

        if (existente is not null)
            return existente;

        var nueva = new SecurityTask
        {
            SecurityRegistrationId = securityRegistrationId,
            FechaIniciada = DateTime.UtcNow,
            TipoAccion = "FinalizarOperacion",
            Completada = false
        };

        await _taskRepo.CreateAsync(nueva);
        return nueva;
    }

    private async Task GuardarFotoAsync(
        int securityRegistrationId,
        int securityTaskId,
        string fotoBase64,
        string? realizadaPor,
        string actionName)
    {
        var bytes = Convert.FromBase64String(fotoBase64);
        var path = await _fileStorage.SaveJpegAsync(bytes, "security/tasks", $"task_{securityTaskId}_{actionName}");

        if (string.IsNullOrWhiteSpace(path))
            return;

        await _photoRepo.CreateAsync(new SecurityRegistrationPhoto
        {
            SecurityRegistrationId = securityRegistrationId,
            SecurityTaskId = securityTaskId,
            Categoria = PhotoCategoria.Cortina,
            Orden = 1,
            RealizadaPor = realizadaPor,
            FilePath = path
        });
    }
}
