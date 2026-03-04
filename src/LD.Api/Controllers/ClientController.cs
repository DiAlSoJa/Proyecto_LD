using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Clients.Queries;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : CommonController
    {
  


        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
            => ResultExtensions.ToActionResult(await Mediator.Send(new ClientByIdQuery(id)));


        [HttpGet]
        public async Task<IActionResult> GetClients()
           => ResultExtensions.ToActionResult(await Mediator.Send(new ClientsQuery()));

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));


        [HttpPut("{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId , UpdateClientCommand command)
        {
            command.ClientId = clientId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(int clientId, ArchiveClientCommand command)
        {
            command.ClientId = clientId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }


    }
}
