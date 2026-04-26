using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.Security;
using MediatR;

namespace LD.Application.Features.Security.Queries;

public class GetSecurityTasksQuery : IRequest<Result<List<SecurityTaskDto>>>
{
    public bool SoloPendientes { get; set; } = false;
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
        var todas = await _taskRepo.GetManyWithRegistracionAsync();
        var lista = todas
            .Where(t => !request.SoloPendientes || !t.Completada)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

        var dtos = _mapper.Map<List<SecurityTaskDto>>(lista);
        return Result<List<SecurityTaskDto>>.Success(dtos, "Tareas obtenidas correctamente");
    }
}
