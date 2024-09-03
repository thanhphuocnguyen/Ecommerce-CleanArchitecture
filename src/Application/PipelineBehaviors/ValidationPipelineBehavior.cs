using Ecommerce.Domain.Shared.Result;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
: IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    private readonly ILogger<ValidationPipelineBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        Error[] errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null)
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage,
                ErrorType.InvalidValue))
            .Distinct()
            .ToArray();

        _logger.LogInformation("Validation errors occurred: {Errors}", errors);

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors, _logger);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors, ILogger logger)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            logger.LogInformation("Validation errors occurred: {Errors}", errors);
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;
        logger.LogInformation("Type of ValidationResult: {ValidationResult}", validationResult.GetType());
        return (TResult)validationResult;
    }
}