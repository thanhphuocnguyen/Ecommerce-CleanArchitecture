namespace Ecommerce.Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = Error.InvalidValue("ValidationError", "A validation problem occurred.");

    Error[] Errors { get; }
}