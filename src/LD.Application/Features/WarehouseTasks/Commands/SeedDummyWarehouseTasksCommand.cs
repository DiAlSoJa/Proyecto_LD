using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.WarehouseTasks.Commands;

/// <summary>
/// SOLO PARA PRUEBAS. Genera N tareas de almacén aleatorias en estado NoAsignada
/// (con WarehouseId) para que el TaskDispatcherWorker las auto-asigne a los usuarios
/// que estén conectados y disponibles. Quitar/deshabilitar antes de producción.
/// </summary>
public class SeedDummyWarehouseTasksCommand : IRequest<Result<string>>
{
    public int WarehouseId { get; set; }
    public int Count { get; set; } = 1;
}

public class SeedDummyWarehouseTasksCommandHandler
    : IRequestHandler<SeedDummyWarehouseTasksCommand, Result<string>>
{
    private static readonly string[] Priorities  = { "Alta", "Media", "Baja" };
    private static readonly string[] Activities   = { "Surtido", "Acomodo", "Conteo", "Reubicación", "Empaque" };
    private static readonly string[] NamePrefixes = { "Pasillo", "Rack", "Zona", "Bahía", "Andén" };

    private readonly IRepository<WarehouseTask> _repository;

    public SeedDummyWarehouseTasksCommandHandler(IRepository<WarehouseTask> repository)
    {
        _repository = repository;
    }

    public async Task<Result<string>> Handle(SeedDummyWarehouseTasksCommand request, CancellationToken cancellationToken)
    {
        if (request.WarehouseId <= 0)
            return Result<string>.Failure("Indica un WarehouseId válido", new());

        var count = Math.Clamp(request.Count, 1, 50);
        var created = 0;

        for (var i = 0; i < count; i++)
        {
            var rnd = Random.Shared;
            var task = new WarehouseTask
            {
                WarehouseId = request.WarehouseId,
                Priority    = Priorities[rnd.Next(Priorities.Length)],
                Activity    = Activities[rnd.Next(Activities.Length)],
                Name        = $"{NamePrefixes[rnd.Next(NamePrefixes.Length)]} {rnd.Next(1, 100)} (DUMMY)",
                Description = "Tarea generada automáticamente para pruebas de auto-asignación.",
                // Status = NoAsignada por defecto → elegible para el worker
            };

            if (await _repository.CreateAsync(task))
                created++;
        }

        return created > 0
            ? Result<string>.Success($"{created} tarea(s) dummy creada(s) en el almacén {request.WarehouseId}", string.Empty)
            : Result<string>.Failure("No se pudo crear ninguna tarea dummy", new());
    }
}
