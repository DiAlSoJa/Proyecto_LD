using LD.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            return Ok("GetClientds");

        }

        [HttpGet]
        public async Task<IActionResult> GetClients2()
        {
            return Ok("GetClientdasdfasfs");

        }
        [HttpPost]
        public async Task<IActionResult> CreateClient()
        {
            return Ok("create client");
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
