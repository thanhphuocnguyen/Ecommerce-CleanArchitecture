using Ecommerce.Domain.Shared;

namespace Ecommerce.WebAPI.Exceptions;

public class BadRequestException : Exception
{
    public Error[] Errors { get; }

    public BadRequestException(string? message, Error[]? errors = null)
        : base(message)
    {
        Errors = errors ?? [];
    }
}