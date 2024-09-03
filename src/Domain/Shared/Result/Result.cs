namespace Ecommerce.Domain.Shared.Result;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error ?? Error.None;
    }

    public bool IsSuccess { get; private set; }

    public Error Error { get; }

    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(true, Error.None, value);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(false, error, default!);

    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static implicit operator Result(bool isSuccess) => isSuccess ? Success() : Failure(Error.None);
}

public class Result<TValue> : Result
{
    protected internal Result(bool isSuccess, Error error, TValue value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public TValue Value { get; private set; }

    public static Result<TValue> Success(TValue value) => new(true, Error.None, value);

    public static new Result<TValue> Failure(Error error) => new(false, error, default!);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
}