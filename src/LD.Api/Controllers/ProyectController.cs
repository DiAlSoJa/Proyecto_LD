using LD.Application.Features.Comands;
using LD.Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProyectController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetProyects([FromQuery] ProjectQuery query)
        {
            return Ok(await _mediator.Send(query));

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
             => Ok(await _mediator.Send(new ProjectByIdQuery(id)));

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateLocationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateContact()
        //{
        //    return Ok("update Contact");
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContact(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
