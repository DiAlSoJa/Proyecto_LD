using LD.Contracts.DTOs.WarehouseTasks;

namespace LD.Contracts.DTOs.Realtime;

/// <summary>
/// Usuario actualmente conectado al hub SignalR, enriquecido con su nombre,
/// almacenes asignados y las tareas que tiene asignadas en este momento.
/// </summary>
public class ConnectedUserDto
{
    public string UserId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public DateTime? ConnectedAt { get; set; }
    public List<ConnectedUserWarehouseDto> Warehouses { get; set; } = new();
    public List<WarehouseTaskDto> AssignedTasks { get; set; } = new();
}

public class ConnectedUserWarehouseDto
{
    public int WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
}
