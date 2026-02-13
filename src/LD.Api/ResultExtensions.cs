using LD.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace LD.Api.Common.Results
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result);

            var statusCode = result.Error?.Code ?? StatusCodes.Status500InternalServerError;

            return statusCode switch
            {
                StatusCodes.Status400BadRequest =>
                    new BadRequestObjectResult(result),

                StatusCodes.Status401Unauthorized =>
                    new UnauthorizedObjectResult(result),

                StatusCodes.Status403Forbidden =>
                    new ObjectResult(result)
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    },

                StatusCodes.Status404NotFound =>
                    new NotFoundObjectResult(result),

                StatusCodes.Status409Conflict =>
                    new ConflictObjectResult(result),

                StatusCodes.Status422UnprocessableEntity =>
                    new UnprocessableEntityObjectResult(result),

                _ =>
                    new ObjectResult(result)
                    {
                        StatusCode = statusCode
                    }
            };
        }
    }
}
