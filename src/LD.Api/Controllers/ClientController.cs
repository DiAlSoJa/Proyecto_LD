using LD.Api.Common.Results;
using LD.Application.Features.Clients.Queries;
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient()
        {
            return ResultExtensions.ToActionResult("update client");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            return ResultExtensions.ToActionResult("Delete client");
        }


    }
}
