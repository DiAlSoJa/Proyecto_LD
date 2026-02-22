using LD.Api.Common.Results;
using LD.Application.Features.Comands;
using LD.Application.Features.Projects.Comands;
using LD.Application.Features.Projects.Queries;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetProyects([FromQuery] ProjectQuery query)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(query));

        }
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
             => ResultExtensions.ToActionResult(await _mediator.Send(new ProjectByIdQuery(projectId)));

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateLocationCommand command)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(command));
        }


        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(int projectId, UpdateProjectCommand command)
        {
            command.ProjectId = projectId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContact(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
