using FluentValidation;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Features.Projects.Comands;

namespace LD.Application.Features.Clients.Validators;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ILocationRepository _locationRepository;

    public CreateProjectValidator(
        IWarehouseRepository warehouseRepository,
        IClientRepository clientRepository,
        ILocationRepository locationRepository)
    {
        _warehouseRepository = warehouseRepository;
        _clientRepository = clientRepository;
        _locationRepository = locationRepository;

        RuleFor(x => x.ProjectName)
            .NotEmpty().WithMessage("El nombre del proyecto es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre del proyecto no puede superar 150 caracteres.");

        RuleFor(x => x.ClientId)
            .NotNull().WithMessage("El cliente es obligatorio.")
            .MustAsync(async (id, ct) => await _clientRepository.GetByIdAsync(id ?? 0) != null)
            .WithMessage("No se encontro el cliente.");

        RuleFor(x => x.WarehouseId)
            .NotNull().WithMessage("El almacen es obligatorio.")
            .MustAsync(async (id, ct) => await _warehouseRepository.GetByIdAsync(id ?? 0) != null)
            .WithMessage("No se encontro el almacen.");

        RuleFor(x => x.LocationId)
            .MustAsync(async (request, locationId, ct) =>
            {
                if (!locationId.HasValue)
                    return true;

                var location = await _locationRepository.GetByIdAsync(locationId.Value);
                return location != null && location.WarehouseId == request.WarehouseId;
            })
            .WithMessage("La ubicacion seleccionada no pertenece al almacen indicado.");

        RuleFor(x => x.StorageTypeId)
            .NotNull().WithMessage("El tipo de almacenamiento es obligatorio.")
            .InclusiveBetween(1, 4).WithMessage("El tipo de almacenamiento debe ser FIFO (1), LIFO (2), Lote (3) o Caducidad (4).");

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

        RuleFor(x => x.ReceiptNotificationMethod)
            .NotEmpty().WithMessage("El metodo de notificacion de recibo es obligatorio cuando esta habilitado.")
            .When(x => x.ReceiptNotificationEnabled);

        RuleFor(x => x.ShipmentNotificationMethod)
            .NotEmpty().WithMessage("El metodo de notificacion de embarque es obligatorio cuando esta habilitado.")
            .When(x => x.ShipmentNotificationEnabled);

        RuleFor(x => x.NormalHrs)
            .GreaterThan(0).WithMessage("El tiempo normal debe ser mayor a 0.")
            .When(x => x.NormalHrs.HasValue);

        RuleFor(x => x.UrgentHrs)
            .GreaterThan(0).WithMessage("El tiempo urgente debe ser mayor a 0.")
            .When(x => x.UrgentHrs.HasValue);

        RuleFor(x => x.AsnNumber)
            .GreaterThanOrEqualTo(0).WithMessage("El numero de ASN no puede ser negativo.")
            .When(x => x.AsnNumber.HasValue);

        RuleFor(x => x.AsnPrefix)
            .MaximumLength(50).WithMessage("El prefijo ASN no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.AsnPrefix));

        RuleFor(x => x.KittingNumber)
            .MaximumLength(50).WithMessage("El numero de kitting no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.KittingNumber));

        RuleFor(x => x.KittingPrefix)
            .MaximumLength(50).WithMessage("El prefijo de kitting no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.KittingPrefix));

        RuleFor(x => x.DoNumber)
            .MaximumLength(50).WithMessage("El numero de DO no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.DoNumber));

        RuleFor(x => x.DoPrefix)
            .MaximumLength(50).WithMessage("El prefijo de DO no puede superar 50 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.DoPrefix));

        RuleForEach(x => x.ScanConfigurations)
            .ChildRules(scan =>
            {
                scan.RuleFor(s => s.SystemFieldName)
                    .NotEmpty().WithMessage("[Configuracion de escaneo] El nombre del campo del sistema es obligatorio.")
                    .MaximumLength(100).WithMessage("[Configuracion de escaneo] El nombre del campo del sistema no puede superar 100 caracteres.");

                scan.RuleFor(s => s.ClientField)
                    .NotEmpty().WithMessage("[Configuracion de escaneo] El campo del cliente es obligatorio.")
                    .MaximumLength(100).WithMessage("[Configuracion de escaneo] El campo del cliente no puede superar 100 caracteres.");

               

                scan.RuleFor(s => s.ScanValue)
                    .NotEmpty().WithMessage("[Configuracion de escaneo] El valor de escaneo es obligatorio cuando se especifica un tipo de escaneo.")
                    .MaximumLength(100).WithMessage("[Configuracion de escaneo] El valor de escaneo no puede superar 100 caracteres.")
                    .When(s => s.ScanTypeId.HasValue);

                
                scan.RuleFor(s => s.SaveValue)
                    .GreaterThanOrEqualTo(0).WithMessage("[Configuracion de escaneo] El valor de guardado no puede ser negativo.");
            });
    }
}
