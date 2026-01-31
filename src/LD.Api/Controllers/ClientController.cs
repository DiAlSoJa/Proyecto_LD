using LD.Application.Features.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] ClientsQuery query)
        {
            return Ok(await _mediator.Send(query));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
            => Ok(await _mediator.Send(new ClientByIdQuery(id)));


        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient()
        {
            return Ok("update client");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            return Ok("Delete client");
        }


    }
}
