using LD.Api.Common.Results;
using LD.Application.Common.Results;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe([FromQuery] GetMeQuery command)
        {
            return ResultExtensions.ToActionResult(await _mediator.Send(command));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }
      
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword()
        {
            return ResultExtensions.ToActionResult(Result < string>.Success( "cambiado de contrase",""));
        }
    }
}
