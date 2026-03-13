using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Clients.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    
    [Route("api/[controller]")]
    public class ClientController : CommonController
    {


        [HttpGet("{id}")]
        [Authorize(Policy =PermissionKeys.Client_View)]
        public async Task<IActionResult> GetClient(int id)
            => ResultExtensions.ToActionResult(await Mediator.Send(new ClientByIdQuery(id)));

        [HttpGet]
        [Authorize(Policy = PermissionKeys.Client_View)]
        public async Task<IActionResult> GetClients()
           => ResultExtensions.ToActionResult(await Mediator.Send(new ClientsQuery()));

        [HttpPost]
        [Authorize(Policy = PermissionKeys.Client_Create)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));

        [HttpPut("{clientId}")]
        [Authorize(Policy = PermissionKeys.Client_Update)]
        public async Task<IActionResult> UpdateClient(int clientId , UpdateClientCommand command)
        {
            command.ClientId = clientId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpDelete("{clientId}")]
        [Authorize(Policy = PermissionKeys.Client_Delete)]
        public async Task<IActionResult> DeleteClient(int clientId, ArchiveClientCommand command)
        {
            command.ClientId = clientId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }


    }
}
