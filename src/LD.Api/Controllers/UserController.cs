using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.User.Commands;
using LD.Application.Features.User.Queries;
using LD.Application.Features.Warehouses.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : CommonController
    {


        [HttpGet]
        [Authorize(Policy = PermissionKeys.User_View)]
        public async Task<IActionResult> GetUsers()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetUsersQuery()));

        [HttpGet("{userId}")]
        [Authorize(Policy = PermissionKeys.User_View)]
        public async Task<IActionResult> GetUserById(string userId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetUserByIdQuery(userId)));

        [HttpPost]
        [Authorize(Policy = PermissionKeys.User_Create)]
        public async Task<IActionResult> CreatUser([FromBody] CreateUserCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));

        [HttpPut("{userId}")]
        [Authorize(Policy = PermissionKeys.User_Update)]
        public async Task<IActionResult> UpdateContact(string userId, [FromBody] UpdateUserCommand command)
        {
            command.UserId = userId;
            return ResultExtensions.ToActionResult(await Mediator.Send(command));

        }
        
    }
}
