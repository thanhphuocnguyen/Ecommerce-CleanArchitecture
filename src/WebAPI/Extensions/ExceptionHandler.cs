using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebAPI.Exceptions;

internal sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            NotFoundException userNotFoundException => new ProblemDetails
            {
                Title = "NotFound",
                Detail = userNotFoundException.Message,
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404",
                Instance = httpContext.Request.Path
            },
            BadRequestException badRequestException => new ProblemDetails
            {
                Title = "BadRequest",
                Detail = badRequestException.Message,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
                Instance = httpContext.Request.Path
            },
            _ => new ProblemDetails
            {
                Title = "InternalServerError",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500",
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }
}