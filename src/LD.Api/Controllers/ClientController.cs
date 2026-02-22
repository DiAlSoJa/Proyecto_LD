using LD.Api.Common.Results;
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
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
            => ResultExtensions.ToActionResult(await _mediator.Send(new ClientByIdQuery(id)));


        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] ClientsQuery query)
           => ResultExtensions.ToActionResult(await _mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
            => ResultExtensions.ToActionResult(await _mediator.Send(command));


        [HttpPut("{clientId}")]
        public async Task<IActionResult> UpdateClient(int clientId , UpdateClientCommand command)
        {
            command.ClientId = clientId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(int clientId, ArchiveClientCommand command)
        {
            command.ClientId = clientId;
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }


    }
}
