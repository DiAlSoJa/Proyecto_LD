using LD.Contracts.Enums;

namespace LD.Contracts.DTOs.Security;

public class PatioMonitorDto
{
    public int SecurityRegistrationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string TipoVehiculo { get; set; } = string.Empty;
    public PatioMonitorSequence_e SequenceType { get; set; }
    public string SequenceText { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Licencia { get; set; } = string.Empty;
    public DateTime Vencimiento { get; set; }
    public string Celular { get; set; } = string.Empty;
    public string Linea { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RegistroEstado_e Estado { get; set; }
    public int? CortinaId { get; set; }
    public string? CortinaNumero { get; set; }
    public string CurrentStep { get; set; } = string.Empty;
    public string NextStep { get; set; } = string.Empty;
    public string CurrentArea { get; set; } = string.Empty;
    public string StatusBadgeText { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public string AlertText { get; set; } = string.Empty;
    public string ProgressText { get; set; } = string.Empty;
    public DateTime? LastEventAt { get; set; }
    public int MinutesInPatio { get; set; }
    public int MinutesSinceLastEvent { get; set; }
    public int CompletedSteps { get; set; }
    public int PendingSteps { get; set; }
    public int TotalSteps { get; set; }
    public bool IsCritical { get; set; }
    public List<PatioMonitorStepDto> Steps { get; set; } = new();
}

public class PatioMonitorStepDto
{
    public string Code { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public int Order { get; set; }
    public PatioMonitorStepState_e State { get; set; }
    public string StateText { get; set; } = string.Empty;
    public bool IsTrackedBySystem { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Note { get; set; }
}
