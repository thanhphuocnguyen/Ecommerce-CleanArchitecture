namespace Ecommerce.Domain.Shared.Result;

public interface IValidationResult
{
    public static readonly Error ValidationError = Error.InvalidValue("ValidationError", "A validation problem occurred.");

    Error[] Errors { get; }
}