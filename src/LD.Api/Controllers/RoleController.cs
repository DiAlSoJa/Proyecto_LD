using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Roles.Commands;
using LD.Application.Features.Roles.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RoleController : CommonController
    {

        [HttpGet]
        public async Task<IActionResult> GetRoles()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetRolesQuery()));


        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(string roleId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetRoleByIdQuery(roleId)));

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
                => ResultExtensions.ToActionResult(await Mediator.Send(command));

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId,UpdateRoleCommand command)
        {
            command.RoleId = roleId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            return Ok("Delete Contact");
        }


    }
}
