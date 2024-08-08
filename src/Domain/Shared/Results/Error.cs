namespace Ecommerce.Domain.Shared.Results;

/// <summary>
/// Represents the type of error.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Represents the absence of an error.
    /// </summary>
    None,

    /// <summary>
    /// Represents a null value error.
    /// </summary>
    InvalidValue,

    /// <summary>
    /// Represents a not found error.
    /// </summary>
    NotFound,

    /// <summary>
    /// Represents an unauthorized error.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Represents a forbidden error.
    /// </summary>
    Forbidden,

    /// <summary>
    /// Represents a conflict error.
    /// </summary>
    Conflict,

    /// <summary>
    /// Represents an internal server error.
    /// </summary>
    InternalServerError
}

public record Error
{
    public string Code { get; set; }

    public string Description { get; set; }

    public ErrorType Type { get; set; }

    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static readonly Error None = new Error(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error NullValue = new Error("Errors.NullValue", "Null value was provided", ErrorType.InvalidValue);

    public static implicit operator Result(Error error) => Result.Failure(error);

    public static Error InvalidValue(string code, string description) => new Error(code, description, ErrorType.InvalidValue);

    public static Error NotFound(string code, string description) => new Error(code, description, ErrorType.NotFound);

    public static Error Unauthorized(string code, string description) => new Error(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description) => new Error(code, description, ErrorType.Forbidden);

    public static Error Conflict(string code, string description) => new Error(code, description, ErrorType.Conflict);

    public static Error InternalServerError(string code, string description) => new Error(code, description, ErrorType.InternalServerError);
}