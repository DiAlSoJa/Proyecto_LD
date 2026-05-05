using LD.Api.Common.Results;
using LD.Api.Controllers.Common;
using LD.Application.Features.StandardLabels.Commands;
using LD.Application.Features.StandardLabels.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class StandardLabelController : CommonController
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateStandardLabelsCommand command)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(command));
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        return ResultExtensions.ToActionResult(await Mediator.Send(new StandardLabelByCodeQuery { Code = code }));
    }
}
