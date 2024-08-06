using System.Net;
using Ecommerce.Domain.Exceptions;

namespace Ecommerce.Domain.Exceptions;

public class UnauthorizedException : DomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public List<string>? Errors { get; }

    public UnauthorizedException(string message)
    : base(message)
    {
    }
}