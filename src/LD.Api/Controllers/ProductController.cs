using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Asn.Queries;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Clients.Queries;
using LD.Application.Features.Product.Comands;
using LD.Application.Features.Product.Queries;
using LD.Contracts.Constants;
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
        [Permission(PermissionKeys.Product_View)]
        public async Task<IActionResult> GetProducts([FromQuery] int? clientId = null, [FromQuery] int? projectId = null)
            => ResultExtensions.ToActionResult(await Mediator.Send(new ProductQuery
            {
                ClientId = clientId,
                ProjectId = projectId
            }));

        
        [HttpGet("{productId}")]
        [Permission(PermissionKeys.Product_View)]
        public async Task<IActionResult> GetProductById(int productId)
             => ResultExtensions.ToActionResult(await Mediator.Send(new ProductByIdQuery(productId)));

        [HttpGet("{clientId}/{projectId}")]
        [Permission(PermissionKeys.Product_View)]
        public async Task<IActionResult> GetProductByClient(int clientId, int projectId)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new ProductByClientIdQuery { ClientId = clientId, ProjectId = projectId }));
        }



        [HttpPost]
        [Permission(PermissionKeys.Product_Create)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
            => ResultExtensions.ToActionResult(await Mediator.Send(command));
        

        [HttpPut("{productId}")]
        [Permission(PermissionKeys.Product_Update)]
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
