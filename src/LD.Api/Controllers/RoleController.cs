using LD.Api.Common.Results;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Roles.Commands;
using LD.Application.Features.Roles.Queries;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
            => ResultExtensions.ToActionResult(await _mediator.Send(new GetRolesQuery()));


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
            => ResultExtensions.ToActionResult(await _mediator.Send(new GetRoleByIdQuery(roleId)));

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
                => ResultExtensions.ToActionResult(await _mediator.Send(command));

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId,UpdateRoleCommand command)
        {
            command.RoleId = roleId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            return Ok("Delete Contact");
        }


    }
}
