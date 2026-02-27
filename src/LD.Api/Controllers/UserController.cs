using LD.Api.Common.Results;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.User.Commands;
using LD.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
            => ResultExtensions.ToActionResult(await _mediator.Send(query));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId, [FromQuery] GetUserByIdQuery query)
            => ResultExtensions.ToActionResult(await _mediator.Send(query));
        
        [HttpPost]
        public async Task<IActionResult> CreatUser([FromBody] CreateUserCommand command)
            => ResultExtensions.ToActionResult(await _mediator.Send(command));

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateContact(string userId, [FromBody] UpdateUserCommand command)
        {
            command.UserId = userId;
            return ResultExtensions.ToActionResult(await _mediator.Send(command));

        }
        
    }
}
