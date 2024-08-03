namespace Ecommerce.Domain.Shared;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    public ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError, default!) => Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}

public sealed class ValidationResult : Result, IValidationResult
{
    public ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError) => Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}