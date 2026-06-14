using LD.Application.Common.Interfaces.Repository;
using LD.Domain.Entities;

namespace LD.Api.Workers;

public sealed class DummyTaskGeneratorWorker : BackgroundService
{
    private readonly TaskGeneratorState _state;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DummyTaskGeneratorWorker> _logger;

    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(20);

    private static readonly string[] Priorities  = ["Alta", "Media", "Baja"];
    private static readonly string[] Activities  =
    [
        "Cambio de ubicación",
        "Surtido de mercancía",
        "Conteo de inventario",
        "Inspección de daños",
        "Reorganización de zona",
        "Verificación de ASN"
    ];
    private static readonly string[] Zones       = ["A", "B", "C", "D", "E"];
    private static readonly string[] Descriptions =
    [
        "Verificar que el pallet esté correctamente identificado antes de moverlo.",
        "Reportar cualquier daño encontrado al supervisor de turno.",
        "Confirmar cantidades con el sistema antes de finalizar.",
        "Tomar evidencia fotográfica al completar la tarea.",
        "Coordinar con el equipo de montacargas si se requiere equipo especial.",
        "Actualizar el sistema una vez concluida la operación."
    ];

    public DummyTaskGeneratorWorker(
        TaskGeneratorState state,
        IServiceScopeFactory scopeFactory,
        ILogger<DummyTaskGeneratorWorker> logger)
    {
        _state       = state;
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DummyTaskGeneratorWorker iniciado — arranca APAGADO");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_state.IsRunning)
            {
                try
                {
                    await CreateDummyTaskAsync(stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error al generar tarea dummy");
                }
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task CreateDummyTaskAsync(CancellationToken ct)
    {
        using var scope    = _scopeFactory.CreateScope();
        var taskRepo       = scope.ServiceProvider.GetRequiredService<IRepository<OperationalTask>>();
        var warehouseRepo  = scope.ServiceProvider.GetRequiredService<IRepository<Warehouse>>();

        var warehouses = await warehouseRepo.GetManyAsync();
        if (warehouses.Count == 0)
        {
            _logger.LogWarning("DummyTaskGeneratorWorker: no hay almacenes en BD, saltando ciclo");
            return;
        }

        var rng         = Random.Shared;
        var warehouse   = warehouses[rng.Next(warehouses.Count)];
        var zone        = Zones[rng.Next(Zones.Length)];
        var rack        = rng.Next(1, 30);
        var pallet      = rng.Next(1000, 9999);

        var task = new OperationalTask
        {
            WarehouseId = warehouse.WarehouseId,
            Priority    = Priorities[rng.Next(Priorities.Length)],
            Activity    = Activities[rng.Next(Activities.Length)],
            Name        = $"[DEMO] {zone}{rack:D2} — pallet #{pallet}",
            Description = Descriptions[rng.Next(Descriptions.Length)]
        };

        var created = await taskRepo.CreateAsync(task);

        if (created)
            _logger.LogInformation(
                "Tarea dummy creada: '{Name}' | Prioridad={Priority} | Almacén={WarehouseId}",
                task.Name, task.Priority, task.WarehouseId);
        else
            _logger.LogWarning("DummyTaskGeneratorWorker: CreateAsync devolvió false");
    }
}
