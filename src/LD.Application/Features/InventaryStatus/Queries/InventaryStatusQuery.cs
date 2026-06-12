using AutoMapper;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.InventaryStatus;
using LD.Domain.Entities;
using MediatR;

namespace LD.Application.Features.Status.Queries;

public record InventaryStatusQuery(int? ClientId = null, int? ProjectId = null)
    : IRequest<Result<List<InventaryStatusDto>?>>;

public class StatusQueryHandler : IRequestHandler<InventaryStatusQuery, Result<List<InventaryStatusDto>?>>
{
    private readonly IRepository<LD.Domain.Entities.InventaryStatus> _statusRepository;
    private readonly IRepository<Client> _clientRepository;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;

    public StatusQueryHandler(
        IRepository<LD.Domain.Entities.InventaryStatus> statusRepository,
        IRepository<Client> clientRepository,
        IRepository<Project> projectRepository,
        IMapper mapper)
    {
        _statusRepository = statusRepository;
        _clientRepository = clientRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<InventaryStatusDto>?>> Handle(InventaryStatusQuery request, CancellationToken cancellationToken)
    {
        var status = await _statusRepository.GetManyAsync() ?? new List<LD.Domain.Entities.InventaryStatus>();
        var filteredStatus = status
            .Where(x => MatchesContext(x, request.ClientId, request.ProjectId))
            .ToList();

        var clients = await _clientRepository.GetManyAsync() ?? new List<Client>();
        var projects = await _projectRepository.GetManyAsync() ?? new List<Project>();

        var clientNamesById = clients
            .GroupBy(x => x.ClientId)
            .ToDictionary(
                x => x.Key,
                x => x.First().CommercialName?.Trim() ?? string.Empty);

        var projectNamesById = projects
            .GroupBy(x => x.ProjectId)
            .ToDictionary(
                x => x.Key,
                x => x.First().ProjectName?.Trim() ?? string.Empty);

        var statusDtos = _mapper.Map<List<InventaryStatusDto>>(filteredStatus);

        for (var i = 0; i < filteredStatus.Count && i < statusDtos.Count; i++)
        {
            var entity = filteredStatus[i];
            var dto = statusDtos[i];

            if (entity.ClientId.HasValue)
            {
                dto.ClientId = entity.ClientId;
                if (clientNamesById.TryGetValue(entity.ClientId.Value, out var clientName))
                {
                    dto.Cliente = clientName;
                }
            }

            if (entity.ProjectId.HasValue)
            {
                dto.ProjectId = entity.ProjectId;
                if (projectNamesById.TryGetValue(entity.ProjectId.Value, out var projectName))
                {
                    dto.Proyecto = projectName;
                }
            }
        }

        return Result<List<InventaryStatusDto>?>.Success(statusDtos, "Estatus obtenidos correctamente");
    }

    private static bool MatchesContext(LD.Domain.Entities.InventaryStatus status, int? clientId, int? projectId)
    {
        if (!clientId.HasValue && !projectId.HasValue)
        {
            return true;
        }

        var clientMatches = !clientId.HasValue
            || !status.ClientId.HasValue
            || status.ClientId.Value == clientId.Value;

        var projectMatches = !projectId.HasValue
            || !status.ProjectId.HasValue
            || status.ProjectId.Value == projectId.Value;

        return clientMatches && projectMatches;
    }
}
