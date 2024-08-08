namespace Ecommerce.Domain.Shared.Results;

public interface IValidationResult
{
    public static readonly Error ValidationError = Error.InvalidValue("ValidationError", "A validation problem occurred.");

    Error[] Errors { get; }
}