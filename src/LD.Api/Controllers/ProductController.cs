using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Items.Comands;
using LD.Application.Features.Items.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LD.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : CommonController
    {
        [HttpGet]
        [Authorize(Policy = PermissionKeys.Product_View)]
        public async Task<IActionResult> GetProducts()
            => ResultExtensions.ToActionResult(await Mediator.Send(new ProductQuery()));

        
        [HttpGet("{productId}")]
        [Authorize(Policy = PermissionKeys.Product_View)]
        public async Task<IActionResult> GetProductById(int productId)
             => ResultExtensions.ToActionResult(await Mediator.Send(new ProductByIdQuery(productId)));

        [HttpPost]
        [Authorize(Policy = PermissionKeys.Product_Create)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));
        

        [HttpPut("{productId}")]
        [Authorize(Policy = PermissionKeys.Product_Update)]
        public async Task<IActionResult> UpdateProduct(int productId, UpdateProductCommand command)
        {
            command.ProductId = productId;
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
