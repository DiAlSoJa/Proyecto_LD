using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Results;
using LD.Contracts.DTOs;
using MediatR;

namespace LD.Application.Features.Lookup.Queries
{
    public record GetProjectClientsByUserWarehousesQuery(string UserId)
        : IRequest<Result<List<UserProjectClientDto>>>;

    public class GetProjectClientsByUserWarehousesQueryHandler
        : IRequestHandler<GetProjectClientsByUserWarehousesQuery, Result<List<UserProjectClientDto>>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectClientsByUserWarehousesQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<List<UserProjectClientDto>>> Handle(
            GetProjectClientsByUserWarehousesQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Result<List<UserProjectClientDto>>.Failure("UserId es obligatorio.", new());

            var projects = await _projectRepository.GetProjectClientsByUserWarehousesAsync(request.UserId);
            return Result<List<UserProjectClientDto>>.Success(projects, "Proyectos obtenidos con exito");
        }
    }
}
