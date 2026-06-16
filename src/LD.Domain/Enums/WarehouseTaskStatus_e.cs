// Mantener sincronizado con el enum gemelo en LD.Contracts/Enums/WarehouseTaskStatus_e.cs
// Si agregas un valor aquí, agrégalo también allá con el MISMO entero subyacente
// o el cast explícito en WarehouseTaskProfile dejará de funcionar correctamente.
namespace LD.Domain.Enums;

public enum WarehouseTaskStatus
{
    NoAsignada = 0,
    Asignada   = 1,
    Completada = 2
}
