using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs.OperationalTasks;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.OperationalTasks.Queries;

public class OperationalTaskQuery : IRequest<Result<List<OperationalTaskDto>>>
{
    public bool SoloPendientes { get; set; }
}

public class OperationalTaskQueryHandler : IRequestHandler<OperationalTaskQuery, Result<List<OperationalTaskDto>>>
{
    private readonly IRepository<OperationalTask> _repository;
    private readonly IMapper _mapper;

    public OperationalTaskQueryHandler(IRepository<OperationalTask> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<List<OperationalTaskDto>>> Handle(OperationalTaskQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetManyAsync() ?? new List<OperationalTask>();
        var filtered = tasks
            .Where(x => !request.SoloPendientes || !x.Completed)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return Result<List<OperationalTaskDto>>.Success(
            _mapper.Map<List<OperationalTaskDto>>(filtered),
            "Tareas obtenidas correctamente");
    }
}
