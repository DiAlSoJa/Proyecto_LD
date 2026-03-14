using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.Auth.Commands;
using LD.Application.Features.Module.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ModuleController : CommonController
    {
   
        [HttpGet]
        public async Task<IActionResult> GetModules()
            => ResultExtensions.ToActionResult(await Mediator.Send(new GetModulesQuery()));

        

     

    }
}
