using FluentValidation;
using LD.Application.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;


namespace LD.Application.Common.Behaviors;

public class ExceptionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{


    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;

    public ExceptionBehavior(
        ILogger<ExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception in {RequestName} - {@Request}",
                typeof(TRequest).Name,
                request);

            return (TResponse)(object) Result<string>.Failure("Unhandled exception in {RequestName} - {@Request}", null,400);
        }
    }
}
