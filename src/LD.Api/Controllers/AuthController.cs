using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
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
    public class AuthController : CommonController
    {


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetMeQuery()));
        

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);
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
