using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.DamageReports.Commands;

public class CreateDamageReportCommand : DamageReportRequest, IRequest<Result<string>>
{
}

public class CreateDamageReportCommandHandler : IRequestHandler<CreateDamageReportCommand, Result<string>>
{
    private readonly IRepository<DamageReport> _repository;
    private readonly IMapper _mapper;

    public CreateDamageReportCommandHandler(IRepository<DamageReport> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateDamageReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.PartNumber))
                return Result<string>.Failure("Captura el numero de parte", new());

            if (string.IsNullOrWhiteSpace(request.DamageType))
                return Result<string>.Failure("Captura el tipo de daño", new());

            if (string.IsNullOrWhiteSpace(request.Category))
                return Result<string>.Failure("Captura la categoria", new());

            if (string.IsNullOrWhiteSpace(request.NewStatus))
                return Result<string>.Failure("Captura el nuevo estatus", new());

            var damageReport = _mapper.Map<DamageReport>(request);
            damageReport.ReportDate = request.ReportDate == default ? DateTime.UtcNow : request.ReportDate;

            var result = await _repository.CreateAsync(damageReport);
            return result
                ? Result<string>.Success("Reporte de daños creado con exito", string.Empty)
                : Result<string>.Failure("Hubo un error al crear el reporte de daños", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al crear el reporte de daños", new List<string> { ex.Message });
        }
    }
}
