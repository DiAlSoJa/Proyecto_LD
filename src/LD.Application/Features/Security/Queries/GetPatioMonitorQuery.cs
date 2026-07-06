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
        var openTask = GetCompletedTask(tasks, "AbrirCortina");
        var closeTask = GetCompletedTask(tasks, "CerrarRegistro");

        var steps = BuildSteps(registration, sequenceType, openTask, closeTask);
        var lastEventAt = GetLastEventAt(registration, openTask, closeTask);
        var minutesInPatio = (int)Math.Max(0, (now - registration.CreatedAt).TotalMinutes);
        var minutesSinceLastEvent = (int)Math.Max(0, (now - lastEventAt).TotalMinutes);

        var currentIndex = steps.FindIndex(step => step.State != PatioMonitorStepState_e.Completed);
        var currentStep = currentIndex >= 0 ? steps[currentIndex] : null;
        var nextStep = currentIndex >= 0 && currentIndex + 1 < steps.Count
            ? steps[currentIndex + 1]
            : null;

        var isCritical = currentStep is not null && ShouldFlagAsCritical(currentStep, minutesSinceLastEvent);
        if (currentStep is not null && currentStep.State != PatioMonitorStepState_e.Completed)
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
        SecurityTask? closeTask)
    {
        var curtainAssigned = registration.CortinaId.HasValue;
        var openCompleted = openTask?.Completada == true;
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
            State = openCompleted ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
            StateText = openCompleted ? "Completado" : "Pendiente",
            IsTrackedBySystem = true,
            CompletedAt = openTask?.FechaCompletada,
            Note = openCompleted
                ? "Tarea de seguridad completada"
                : "Pendiente de apertura"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "ComenzarOperacion",
            Label = "Comenzar operación",
            Area = "Operaciones",
            Order = order++,
            State = openCompleted ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
            StateText = openCompleted ? "Completado" : "Pendiente",
            IsTrackedBySystem = false,
            CompletedAt = openTask?.FechaCompletada,
            Note = openCompleted
                ? "Derivado de la apertura de cortina"
                : "No existe evento directo"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "TerminarOperacion",
            Label = "Terminar operación",
            Area = "Operaciones",
            Order = order++,
            State = closeCompleted ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
            StateText = closeCompleted ? "Completado" : "Pendiente",
            IsTrackedBySystem = false,
            CompletedAt = closeTask?.FechaCompletada,
            Note = closeCompleted
                ? "Derivado del cierre del registro"
                : "Pendiente de terminar la operación"
        });

        steps.Add(new PatioMonitorStepDto
        {
            Code = "CerrarCortina",
            Label = "Cerrar cortina",
            Area = "Seguridad",
            Order = order++,
            State = closeCompleted ? PatioMonitorStepState_e.Completed : PatioMonitorStepState_e.Pending,
            StateText = closeCompleted ? "Completado" : "Pendiente",
            IsTrackedBySystem = true,
            CompletedAt = closeTask?.FechaCompletada,
            Note = closeCompleted
                ? "Cortina liberada"
                : "Esperando cierre"
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

    private static SecurityTask? GetCompletedTask(List<SecurityTask> tasks, string tipoAccion)
        => tasks
            .Where(t => t.TipoAccion == tipoAccion && t.Completada)
            .OrderByDescending(t => t.FechaCompletada ?? t.CreatedAt)
            .FirstOrDefault();

    private static DateTime GetLastEventAt(
        SecurityRegistration registration,
        SecurityTask? openTask,
        SecurityTask? closeTask)
    {
        if (closeTask?.FechaCompletada.HasValue == true)
            return closeTask.FechaCompletada.Value;

        if (openTask?.FechaCompletada.HasValue == true)
            return openTask.FechaCompletada.Value;

        if (registration.Estado == RegistroEstado.CortinaAsignada && registration.LastModifiedAt.HasValue)
            return registration.LastModifiedAt.Value;

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
            "ComenzarOperacion" => 30,
            "TerminarOperacion" => 45,
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
