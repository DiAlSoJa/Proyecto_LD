using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Category.Comands;
using LD.Application.Features.Category.Queries;
using LD.Contracts.Constants;
using LD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Category_View)]
        public async Task<IActionResult> getCategory()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new CategoryQuery()));

        }

        [HttpGet("{categoryId}")]
        [Permission(PermissionKeys.Category_View)]
        public async Task<IActionResult> GeCategoryById(string categoryId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new CategoryByIdQuery(categoryId)));


        [HttpPost]
        [Permission(PermissionKeys.Category_Create)]
        public async Task<IActionResult> Createcategory([FromBody] CreateCategoryCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{categoryId}")]
        [Permission(PermissionKeys.Category_Update)]
        public async Task<IActionResult> Updatecategory(string categoryId, UpdateCategoryCommand command)
        {
            command.CategoryIdS = categoryId;
            var result = await Mediator.Send(command);
            return ResultExtensions.ToActionResult(result);
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteWarehouse(int id)
        //{
        //    return Ok("Delete Contact");
        //}


    }
}
