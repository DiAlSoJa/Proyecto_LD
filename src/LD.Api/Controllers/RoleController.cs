using LD.Application.Features.Auth.Commands;
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
        public async Task<IActionResult> GetContact()
        {
            return Ok("get Contact");

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {

            return Ok("GetContactId");
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact()
        {
            return Ok("create Contact");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact()
        {
            return Ok("update Contact");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            return Ok("Delete Contact");
        }


    }
}
