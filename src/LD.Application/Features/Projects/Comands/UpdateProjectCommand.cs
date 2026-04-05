using AutoMapper;
using LD.Application.Common.Interfaces.Auth;
using LD.Application.Common.Interfaces.Repository;
using LD.Application.Common.Models;
using LD.Application.Common.Results;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Application.Features.Projects.Comands;

public class UpdateProjectCommand :ProjectRequest, IRequest<Result<string>>
{

}


public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<string>>
{

    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    public UpdateProjectCommandHandler(IProjectRepository projectRepository,IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public async Task<Result<string>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {

        try
        {
            // 1️ Buscar proyecto con configuraciones de escaneo
            var project = await _projectRepository.GetByIdWithConfigsAsync(request.ProjectId.GetValueOrDefault(-1));
            if (project is null)
                return Result<string>.Failure("No existe el prjecto", new List<string> { "No existe el prjecto" }, 404);

            // 2️⃣ Mapear datos básicos (sin ScanConfigurations)
            _mapper.Map(request, project);

            // 3️⃣ Sincronizar configuraciones de escaneo
            project.ScanConfigurations.Clear();
            foreach (var scanReq in request.ScanConfigurations)
                project.ScanConfigurations.Add(_mapper.Map<ScanConfiguration>(scanReq));

            // 4️⃣ Guardar
            var updated = await _projectRepository.UpdateAsync(project);

            return updated ? 
                Result<string>.Success("Projecto actualizado con exito", "") : 
                Result<string>.Failure("Hubo un error al actualizar el Projecto", new());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("Hubo un error al actualizar el Projecto", new List<string> { ex.Message });
        }
    }
}
