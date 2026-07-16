using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetSecurityTasksQuery : IRequest<Result<List<SecurityTaskDto>>>
{
    public bool SoloPendientes { get; set; } = false;
    public int? SecurityRegistrationId { get; set; }
}

public class GetSecurityTasksQueryHandler
    : IRequestHandler<GetSecurityTasksQuery, Result<List<SecurityTaskDto>>>
{
    private readonly ISecurityTaskRepository _taskRepo;
    private readonly IMapper _mapper;

    public GetSecurityTasksQueryHandler(ISecurityTaskRepository taskRepo, IMapper mapper)
    {
        _taskRepo = taskRepo;
        _mapper   = mapper;
    }

    public async Task<Result<List<SecurityTaskDto>>> Handle(
        GetSecurityTasksQuery request, CancellationToken cancellationToken)
    {
        var todas = await _taskRepo.GetManyWithRegistracionAsync(request.SecurityRegistrationId);

        IEnumerable<SecurityTask> lista = todas;
        if (!request.SecurityRegistrationId.HasValue)
        {
            lista = lista.Where(t => t.TipoAccion == "AbrirCortina" || t.TipoAccion == "CerrarRegistro");
        }

        if (request.SoloPendientes)
        {
            lista = lista.Where(t => !t.Completada);
        }

        var ordenadas = lista
            .OrderByDescending(t => t.FechaIniciada)
            .ThenByDescending(t => t.SecurityTaskId)
            .ToList();

        var dtos = _mapper.Map<List<SecurityTaskDto>>(ordenadas);
        return Result<List<SecurityTaskDto>>.Success(dtos, "Tareas obtenidas correctamente");
    }
}
