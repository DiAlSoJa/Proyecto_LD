using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Items.Comands;
using LD.Application.Features.Items.Queries;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : CommonController
    {
        [HttpGet]
        public async Task<IActionResult> GetItems()
            => ResultExtensions.ToActionResult(await Mediator.Send(new ProductQuery()));

        
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
             => ResultExtensions.ToActionResult(await Mediator.Send(new ProductByIdQuery(itemId)));

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateProductCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));
        

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateLocation(int itemId, UpdateProductCommand command)
        {
            command.ItemId = itemId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteClient(int id)
        //{
        //    return Ok("Delete client");
        //}


    }
}
