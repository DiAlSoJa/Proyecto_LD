using FluentValidation;
using LD.Application.Common.Results;
using MediatR;


namespace LD.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Any())
        {
            var detailedErrors = failures
                .Select(f => string.IsNullOrWhiteSpace(f.PropertyName)
                    ? f.ErrorMessage
                    : $"{f.PropertyName}: {f.ErrorMessage}")
                .ToList();

            var message = "Errores de validacion: " + string.Join(" | ", detailedErrors);

            return (TResponse)(object)Result<string>.Failure(message, detailedErrors, 400);

        }

        return await next();
    }
}
