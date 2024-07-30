namespace Ecommerce.Domain.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
            "RequestValidationError",
            "Request body is invalid.",
            ErrorType.InvalidValue);

    Error[] Errors { get; }
}