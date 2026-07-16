using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using LD.Domain.Entities;
using LD.Domain.Enums;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetPatioMonitorQuery : IRequest<Result<List<PatioMonitorDto>>>
{
}

public class GetPatioMonitorQueryHandler : IRequestHandler<GetPatioMonitorQuery, Result<List<PatioMonitorDto>>>
{
    private const int DefaultCriticalMinutes = 45;
    private static readonly string[] TiposAbrirCortina = new[] { "AbrirCortina" };
    private static readonly string[] TiposIniciarOperacion = new[] { "IniciarOperacion", "ComenzarOperacion" };
    private static readonly string[] TiposFinalizarOperacion = new[] { "FinalizarOperacion", "TerminarOperacion" };
    private static readonly string[] TiposCerrarRegistro = new[] { "CerrarRegistro" };

    private readonly ISecurityRegistrationRepository _registrationRepository;
    private readonly ISecurityTaskRepository _taskRepository;

    public GetPatioMonitorQueryHandler(
        ISecurityRegistrationRepository registrationRepository,
        ISecurityTaskRepository taskRepository)
    {
        _registrationRepository = registrationRepository;
        _taskRepository = taskRepository;
    }

    public async Task<Result<List<PatioMonitorDto>>> Handle(GetPatioMonitorQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var registrations = await _registrationRepository.GetManyWithCortinaAsync();
            var tasks = await _taskRepository.GetManyWithRegistracionAsync();
            var taskLookup = tasks
                .GroupBy(t => t.SecurityRegistrationId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var now = DateTime.UtcNow;

            var rows = registrations
                .Where(r => r.IsActive)
                .Select(r => BuildRow(r, taskLookup.TryGetValue(r.SecurityRegistrationId, out var regTasks) ? regTasks : [], now))
                .OrderByDescending(x => x.IsCritical)
                .ThenByDescending(x => x.MinutesSinceLastEvent)
                .ThenByDescending(x => x.CreatedAt)
                .ToList();

            return Result<List<PatioMonitorDto>>.Success(rows, "Monitor de patio obtenido correctamente");
        }
        catch (Exception ex)
        {
            return Result<List<PatioMonitorDto>>.Failure(
                "No se pudo obtener el monitor de patio",
                new List<string> { ex.Message });
        }
    }

    private static PatioMonitorDto BuildRow(SecurityRegistration registration, List<SecurityTask> tasks, DateTime now)
    {
        var sequenceType = DetectSequenceType(registration.TipoVehiculo);

        var openTask = GetLatestTask(tasks, TiposAbrirCortina);
        var startTask = openTask?.Completada == true
            ? GetLatestTask(tasks, TiposIniciarOperacion, openTask.FechaCompletada)
            : null;
        var finishTask = startTask?.Completada == true
            ? GetLatestTask(tasks, TiposFinalizarOperacion, startTask.FechaCompletada)
            : null;
        var closeTask = finishTask?.Completada == true
            ? GetLatestTask(tasks, TiposCerrarRegistro, finishTask.FechaCompletada)
            : null;

        var steps = BuildSteps(registration, sequenceType, openTask, startTask, finishTask, closeTask);
        var lastEventAt = GetLastEventAt(registration, openTask, startTask, finishTask, closeTask);
        var minutesInPatio = (int)Math.Max(0, (now - registration.CreatedAt).TotalMinutes);
        var minutesSinceLastEvent = (int)Math.Max(0, (now - lastEventAt).TotalMinutes);

        var currentIndex = steps.FindIndex(step => step.State != PatioMonitorStepState_e.Completed);
        var currentStep = currentIndex >= 0 ? steps[currentIndex] : null;
        var nextStep = currentIndex >= 0 && currentIndex + 1 < steps.Count
            ? steps[currentIndex + 1]
            : null;

        var isCritical = currentStep is not null && ShouldFlagAsCritical(currentStep, minutesSinceLastEvent);
        if (currentStep is not null)
        {
            currentStep.State = isCritical ? PatioMonitorStepState_e.Overdue : PatioMonitorStepState_e.Current;
            currentStep.StateText = isCritical ? "Crítico" : "En curso";
        }

        if (currentIndex >= 0)
        {
            for (var i = currentIndex + 1; i < steps.Count; i++)
            {
                steps[i].State = PatioMonitorStepState_e.Pending;
                steps[i].StateText = "Pendiente";
            }
        }

        var completedSteps = steps.Count(x => x.State == PatioMonitorStepState_e.Completed);
        var pendingSteps = steps.Count - completedSteps;

        return new PatioMonitorDto
        {
            SecurityRegistrationId = registration.SecurityRegistrationId,
            CreatedAt = registration.CreatedAt,
            Tipo = registration.Tipo,
            TipoVehiculo = registration.TipoVehiculo,
            SequenceType = sequenceType,
            SequenceText = SequenceText(sequenceType),
            Nombre = registration.Nombre,
            Licencia = registration.Licencia,
            Vencimiento = registration.Vencimiento,
            Celular = registration.Celular,
            Linea = registration.Linea,
            Origen = registration.Origen,
            Numero = registration.Numero,
            Placa = registration.Placa,
            IsActive = registration.IsActive,
            Estado = (RegistroEstado_e)(int)registration.Estado,
            CortinaId = registration.CortinaId,
            CortinaNumero = registration.Cortina?.Numero,
            CurrentStep = currentStep?.Label ?? "Sin pendientes",
            NextStep = nextStep?.Label ?? "Sin pendientes",
            CurrentArea = currentStep?.Area ?? string.Empty,
            StatusBadgeText = currentStep is null
                ? "Completado"
                : (isCritical ? "Crítico" : "En seguimiento"),
            StatusText = currentStep is null
                ? "Completado"
                : (isCritical
                    ? "Crítico"
                    : $"En seguimiento · {currentStep.Area}"),
            AlertText = BuildAlertText(currentStep, pendingSteps, minutesSinceLastEvent, isCritical),
            ProgressText = $"{completedSteps}/{steps.Count} pasos",
            LastEventAt = lastEventAt,
            MinutesInPatio = minutesInPatio,
            MinutesSinceLastEvent = minutesSinceLastEvent,
            CompletedSteps = completedSteps,
            PendingSteps = pendingSteps,
            TotalSteps = steps.Count,
            IsCritical = isCritical,
            Steps = steps
        };
    }

    private static List<PatioMonitorStepDto> BuildSteps(
        SecurityRegistration registration,
        PatioMonitorSequence_e sequenceType,
        SecurityTask? openTask,
        SecurityTask? startTask,
        SecurityTask? finishTask,
        SecurityTask? closeTask)
    {
        var curtainAssigned = registration.CortinaId.HasValue;
        var openCompleted = openTask?.Completada == true;
        var startCompleted = startTask?.Completada == true;
        var finishCompleted = finishTask?.Completada == true;
        var closeCompleted = closeTask?.Completada == true;

        var steps = new List<PatioMonitorStepDto>();

        steps.Add(new PatioMonitorStepDto
        {
            Code = "RegistroUnidad",
            Label = "Registro unidad",
            Area = "Seguridad",
            Order = 1,
            State = PatioMonitorStepState_e.Completed,
            StateText = "Completado",
            IsTrackedBySystem = true,
            CompletedAt = registration.CreatedAt,
            Note = "Inicio del flujo"
        });

        var order = 2;

        if (sequenceType == PatioMonitorSequence_e.Tracto)
        {
            steps.Add(new PatioMonitorStepDto
            {
                Code = "AsignacionCaja",
                Label = "Asignación de caja",
                Area = "Operaciones",
                Order = order++,
                State = curtainAssigned ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
                StateText = curtainAssigned ? "Completado" : "Pendiente",
                IsTrackedBySystem = false,
                CompletedAt = null,
                Note = curtainAssigned
                    ? "Inferido por el avance del flujo"
                    : "Se espera antes de la cortina"
            });
        }

        steps.Add(new PatioMonitorStepDto
        {
            Code = "AsignacionCortina",
            Label = "Asignación de cortina",
            Area = "Operaciones",
            Order = order++,
            State = curtainAssigned ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
            StateText = curtainAssigned ? "Completado" : "Pendiente",
            IsTrackedBySystem = true,
            CompletedAt = registration.Estado == RegistroEstado.CortinaAsignada
                ? registration.LastModifiedAt
                : null,
            Note = curtainAssigned
                ? $"Cortina {registration.Cortina?.Numero ?? "asignada"}"
                : "Esperando cortina disponible"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "AbrirCortina",
            Label = "Abrir cortina",
            Area = "Seguridad",
            Order = order++,
            State = openCompleted
                ? PatioMonitorStepState_e.Completed
                : curtainAssigned
                    ? PatioMonitorStepState_e.Current
                    : PatioMonitorStepState_e.Pending,
            StateText = openCompleted
                ? "Completado"
                : curtainAssigned
                    ? "En curso"
                    : "Pendiente",
            IsTrackedBySystem = true,
            CompletedAt = openTask?.FechaCompletada,
            Note = openCompleted
                ? "Tarea de seguridad completada"
                : "Pendiente de apertura"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "IniciarOperacion",
            Label = "Iniciar operación",
            Area = "Operaciones",
            Order = order++,
            State = startCompleted
                ? PatioMonitorStepState_e.Completed
                : openCompleted
                    ? PatioMonitorStepState_e.Current
                    : PatioMonitorStepState_e.Pending,
            StateText = startCompleted
                ? "Completado"
                : openCompleted
                    ? "En curso"
                    : "Pendiente",
            IsTrackedBySystem = false,
            CompletedAt = startTask?.FechaCompletada,
            Note = startCompleted
                ? "La operación ya fue iniciada"
                : openCompleted
                    ? "Pendiente de iniciar operación"
                    : "Esperando apertura de cortina"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "FinalizarOperacion",
            Label = "Finalizar operación",
            Area = "Operaciones",
            Order = order++,
            State = finishCompleted
                ? PatioMonitorStepState_e.Completed
                : startCompleted
                    ? PatioMonitorStepState_e.Current
                    : PatioMonitorStepState_e.Pending,
            StateText = finishCompleted
                ? "Completado"
                : startCompleted
                    ? "En curso"
                    : "Pendiente",
            IsTrackedBySystem = false,
            CompletedAt = finishTask?.FechaCompletada,
            Note = finishCompleted
                ? "La operación ya fue finalizada"
                : startCompleted
                    ? "Pendiente de finalizar operación"
                    : "Pendiente de iniciar operación"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "CerrarCortina",
            Label = "Cerrar cortina",
            Area = "Seguridad",
            Order = order++,
            State = closeCompleted
                ? PatioMonitorStepState_e.Completed
                : finishCompleted
                    ? PatioMonitorStepState_e.Current
                    : PatioMonitorStepState_e.Pending,
            StateText = closeCompleted
                ? "Completado"
                : finishCompleted
                    ? "En curso"
                    : "Pendiente",
            IsTrackedBySystem = true,
            CompletedAt = closeTask?.FechaCompletada,
            Note = closeCompleted
                ? "Cortina liberada"
                : "Esperando finalización"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "SalidaUnidad",
            Label = "Salida a la unidad",
            Area = "Seguridad",
            Order = order,
            State = closeCompleted ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
            StateText = closeCompleted ? "Completado" : "Pendiente",
            IsTrackedBySystem = true,
            CompletedAt = closeTask?.FechaCompletada,
            Note = closeCompleted
                ? "Unidad fuera de patio"
                : "Salida pendiente"
        });

        return steps;
    }

    private static PatioMonitorSequence_e DetectSequenceType(string tipoVehiculo)
    {
        if (tipoVehiculo.Contains("tract", StringComparison.OrdinalIgnoreCase))
            return PatioMonitorSequence_e.Tracto;

        if (tipoVehiculo.Contains("caja", StringComparison.OrdinalIgnoreCase))
            return PatioMonitorSequence_e.Caja;

        return PatioMonitorSequence_e.Desconocido;
    }

    private static string SequenceText(PatioMonitorSequence_e sequenceType)
        => sequenceType switch
        {
            PatioMonitorSequence_e.Tracto => "Tracto",
            PatioMonitorSequence_e.Caja => "Caja",
            _ => "No definido"
        };

    private static SecurityTask? GetLatestTask(List<SecurityTask> tasks, string tipoAccion, DateTime? desde = null)
        => GetLatestTask(tasks, new[] { tipoAccion }, desde);

    private static SecurityTask? GetLatestTask(List<SecurityTask> tasks, string[] tiposAccion, DateTime? desde = null)
        => tasks
            .Where(t => tiposAccion.Contains(t.TipoAccion))
            .Where(t => !desde.HasValue || t.FechaIniciada >= desde.Value)
            .OrderByDescending(t => t.FechaIniciada)
            .ThenByDescending(t => t.SecurityTaskId)
            .FirstOrDefault();

    private static DateTime GetLastEventAt(
        SecurityRegistration registration,
        SecurityTask? openTask,
        SecurityTask? startTask,
        SecurityTask? finishTask,
        SecurityTask? closeTask)
    {
        if (closeTask?.FechaCompletada.HasValue == true)
            return closeTask.FechaCompletada.Value;

        if (finishTask?.FechaCompletada.HasValue == true)
            return finishTask.FechaCompletada.Value;

        if (startTask?.FechaCompletada.HasValue == true)
            return startTask.FechaCompletada.Value;

        if (openTask?.FechaCompletada.HasValue == true)
            return openTask.FechaCompletada.Value;

        if (registration.LastModifiedAt.HasValue)
            return registration.LastModifiedAt.Value;

        return registration.CreatedAt;
    }

    private static bool ShouldFlagAsCritical(PatioMonitorStepDto currentStep, int minutesSinceLastEvent)
    {
        var threshold = currentStep.Code switch
        {
            "RegistroUnidad" => 30,
            "AsignacionCaja" => 30,
            "AsignacionCortina" => 30,
            "AbrirCortina" => 20,
            "IniciarOperacion" => 20,
            "FinalizarOperacion" => 45,
            "CerrarCortina" => 20,
            "SalidaUnidad" => 15,
            _ => DefaultCriticalMinutes
        };

        return minutesSinceLastEvent >= threshold;
    }

    private static string BuildAlertText(
        PatioMonitorStepDto? currentStep,
        int pendingSteps,
        int minutesSinceLastEvent,
        bool isCritical)
    {
        if (currentStep is null)
            return "Sin pasos pendientes";

        if (isCritical)
            return $"{currentStep.Label} lleva {minutesSinceLastEvent} min sin avance";

        return pendingSteps > 0
            ? $"Faltan {pendingSteps} movimiento(s)"
            : "Sin pendientes";
    }
}
