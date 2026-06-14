// Mantener sincronizado con el enum gemelo en LD.Domain/Enums/OperationalTaskStatus_e.cs
// Si agregas un valor aquí, agrégalo también allá con el MISMO entero subyacente
// o el cast explícito en OperationalTaskProfile dejará de funcionar correctamente.
namespace LD.Contracts.Enums;

public enum OperationalTaskStatus
{
    NoAsignada = 0,
    Asignada   = 1,
    Completada = 2
}
