using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Comands;
using LD.Application.Features.Projects.Comands;
using LD.Application.Features.Projects.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectController : CommonController
    {
      

        [HttpGet]
        [Permission(PermissionKeys.Project_View)]
        public async Task<IActionResult> GetProjects()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new ProjectQuery()));

        }
        [HttpGet("{projectId}")]
        [Permission(PermissionKeys.Project_View)]
        public async Task<IActionResult> GetProjectById(int projectId)
             => ResultExtensions.ToActionResult(await Mediator.Send(new ProjectByIdQuery(projectId)));

        [HttpPost]
        [Permission(PermissionKeys.Project_Create)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }


        [HttpPut("{projectId}")]
        [Permission(PermissionKeys.Project_Update)]
        public async Task<IActionResult> UpdateProject(int projectId, UpdateProjectCommand command)
        {
            command.ProjectId = projectId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContact(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
