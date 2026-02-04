using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Items.Comands;
using LD.Application.Features.Items.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] ItemQuery query)
        {
            return Ok(await _mediator.Send(query));

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
             => Ok(await _mediator.Send(new ItemByIdQuery(id)));

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateClient()
        //{
        //    return Ok("update client");
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteClient(int id)
        //{
        //    return Ok("Delete client");
        //}


    }
}
