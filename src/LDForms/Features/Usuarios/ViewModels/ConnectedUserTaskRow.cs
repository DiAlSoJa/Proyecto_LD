using System;
using LD.Contracts.Enums;

namespace LD.FormsX.Features.Usuarios.ViewModels;

/// <summary>
/// Fila aplanada para el grid "tareas asignadas de cada usuario conectado":
/// combina el usuario asignado con los datos de su tarea.
/// </summary>
public class ConnectedUserTaskRow
{
    public string? UserName { get; set; }
    public int Folio { get; set; }
    public string? TaskName { get; set; }
    public string? WarehouseName { get; set; }
    public WarehouseTaskStatus Status { get; set; }
    public DateTime? AssignedAt { get; set; }
}
