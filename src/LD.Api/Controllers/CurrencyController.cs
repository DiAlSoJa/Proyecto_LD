using LD.Api.Authorization;
using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Currency.Comands;
using LD.Application.Features.Currency.Queries;
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
    public class CurrencyController : CommonController
    {

        [HttpGet]
        [Permission(PermissionKeys.Currency_View)]
        public async Task<IActionResult> getCurrency()
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(new CurrencyQuery()));

        }

        [HttpGet("{currencyId}")]
        [Permission(PermissionKeys.Currency_View)]
        public async Task<IActionResult> GeCurrencyById(string currencyId)
            => ResultExtensions.ToActionResult(await Mediator.Send(new CurrencyByIdQuery(currencyId)));


        [HttpPost]
        [Permission(PermissionKeys.Currency_Create)]
        public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyCommand command)
        {
            return ResultExtensions.ToActionResult(await Mediator.Send(command));
        }

        [HttpPut("{currencyId}")]
        [Permission(PermissionKeys.Currency_Update)]
        public async Task<IActionResult> UpdateCurrency(string currencyId, UpdateCurrencyCommand command)
        {
            command.CurrencyIdS = currencyId;
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
