using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Results;

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