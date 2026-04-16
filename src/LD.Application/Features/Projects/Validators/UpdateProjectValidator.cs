using FluentValidation;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Features.Projects.Comands;

namespace LD.Application.Features.Clients.Validators;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectValidator(
        IWarehouseRepository warehouseRepository,
        IClientRepository clientRepository,
        IProjectRepository projectRepository)
    {
        _warehouseRepository = warehouseRepository;
        _clientRepository = clientRepository;
        _projectRepository = projectRepository;

        // ── Identificador ──────────────────────────────────────────────────

        RuleFor(x => x.ProjectId)
            .NotNull().WithMessage("El ID del proyecto es obligatorio.")
            .MustAsync(async (id, ct) => await _projectRepository.GetByIdAsync(id ?? 0) != null)
            .WithMessage("No se encontró el proyecto.");

        // ── Datos generales ────────────────────────────────────────────────

        RuleFor(x => x.ProjectName)
            .NotEmpty().WithMessage("El nombre del proyecto es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre del proyecto no puede superar 150 caracteres.");

        RuleFor(x => x.ClientId)
            .NotNull().WithMessage("El cliente es obligatorio.")
            .MustAsync(async (id, ct) => await _clientRepository.GetByIdAsync(id ?? 0) != null)
            .WithMessage("No se encontró el cliente.");

        RuleFor(x => x.WarehouseId)
            .NotNull().WithMessage("El almacén es obligatorio.")
            .MustAsync(async (id, ct) => await _warehouseRepository.GetByIdAsync(id ?? 0) != null)
            .WithMessage("No se encontró el almacén.");

        RuleFor(x => x.StorageTypeId)
            .NotNull().WithMessage("El tipo de almacenamiento es obligatorio.")
            .InclusiveBetween(1, 4).WithMessage("El tipo de almacenamiento debe ser FIFO (1), LIFO (2), Lote (3) o Caducidad (4).");

        // ── Unidades (FK a Units.UnitIdS — max 20 chars) ──────────────────

        RuleFor(x => x.Entrada)
            .MaximumLength(20).WithMessage("La unidad de entrada no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Entrada));

        RuleFor(x => x.StorageArea)
            .MaximumLength(20).WithMessage("La unidad de almacenamiento no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.StorageArea));

        RuleFor(x => x.ReworkArea)
            .MaximumLength(20).WithMessage("La unidad de retrabajo no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.ReworkArea));

        RuleFor(x => x.Salida)
            .MaximumLength(20).WithMessage("La unidad de salida no puede superar 20 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Salida));

        // ── Notificaciones ─────────────────────────────────────────────────

        RuleFor(x => x.ReceiptNotificationMethod)
            .NotEmpty().WithMessage("El método de notificación de recibo es obligatorio cuando está habilitado.")
            .When(x => x.ReceiptNotificationEnabled);

        RuleFor(x => x.ShipmentNotificationMethod)
            .NotEmpty().WithMessage("El método de notificación de embarque es obligatorio cuando está habilitado.")
            .When(x => x.ShipmentNotificationEnabled);

        // ── Tiempos ────────────────────────────────────────────────────────

        RuleFor(x => x.NormalHrs)
            .GreaterThan(0).WithMessage("El tiempo normal debe ser mayor a 0.")
            .When(x => x.NormalHrs.HasValue);

        RuleFor(x => x.UrgentHrs)
            .GreaterThan(0).WithMessage("El tiempo urgente debe ser mayor a 0.")
            .When(x => x.UrgentHrs.HasValue);

        // ── Prefijos ───────────────────────────────────────────────────────

        RuleFor(x => x.AsnNumber)
            .GreaterThanOrEqualTo(0).WithMessage("El número de ASN no puede ser negativo.")
            .When(x => x.AsnNumber.HasValue);

        RuleFor(x => x.AsnPrefix)
            .MaximumLength(50).WithMessage("El prefijo ASN no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.AsnPrefix));

        RuleFor(x => x.KittingNumber)
            .MaximumLength(50).WithMessage("El número de kitting no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.KittingNumber));

        RuleFor(x => x.KittingPrefix)
            .MaximumLength(50).WithMessage("El prefijo de kitting no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.KittingPrefix));

        RuleFor(x => x.DoNumber)
            .MaximumLength(50).WithMessage("El número de DO no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.DoNumber));

        RuleFor(x => x.DoPrefix)
            .MaximumLength(50).WithMessage("El prefijo de DO no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.DoPrefix));

        // ── Configuraciones de escaneo ─────────────────────────────────────

        RuleForEach(x => x.ScanConfigurations)
            .ChildRules(scan =>
            {
  
                scan.RuleFor(s => s.SystemFieldName)
                    .NotEmpty().WithMessage("[Configuracion de escaneo] El nombre del campo del sistema es obligatorio.")
                    .MaximumLength(100).WithMessage("[Configuracion de escaneo] El nombre del campo del sistema no puede superar 100 caracteres.");

                scan.RuleFor(s => s.ClientField)
                    .NotEmpty().WithMessage("[Configuracion de escaneo] El campo del cliente es obligatorio.")
                    .MaximumLength(100).WithMessage("[Configuracion de escaneo] El campo del cliente no puede superar 100 caracteres.");

                scan.RuleFor(s => s.ScanTypeId)
                    .NotNull().WithMessage("[Configuracion de escaneo] El tipo de escaneo es obligatorio.")
                    .InclusiveBetween(1, 3).WithMessage("[Configuracion de escaneo] El tipo de escaneo no es válido.");

                scan.RuleFor(s => s.ScanValue)
                    .NotEmpty().WithMessage("[Configuracion de escaneo] El valor de escaneo es obligatorio cuando se especifica un tipo de escaneo.")
                    .MaximumLength(100).WithMessage("[Configuracion de escaneo] El valor de escaneo no puede superar 100 caracteres.")
                    .When(s => s.ScanTypeId.HasValue);

                scan.RuleFor(s => s.SaveTypeId)
                    .NotNull().WithMessage("[Configuracion de escaneo] El tipo de guardado es obligatorio.")
                    .InclusiveBetween(1, 2).WithMessage("[Configuracion de escaneo] El tipo de guardado no es válido.");

                scan.RuleFor(s => s.SaveValue)
                    .GreaterThanOrEqualTo(0).WithMessage("[Configuracion de escaneo] El valor de guardado no puede ser negativo.");
            });
    }
}
