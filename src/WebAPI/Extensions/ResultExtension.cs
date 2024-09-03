using Ecommerce.Domain.Shared.Result;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebAPI.Exceptions;

public static class ResultExtension
{
    public static IResult Match(
        this Result result,
        Func<IResult> onSuccess)
    {
        if (result.IsSuccess)
        {
            return onSuccess();
        }

        return result switch
        {
            IValidationResult validationResult =>
                Results.BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = result.Error.Description,
                    Extensions = new Dictionary<string, object?>
                    {
                        ["errors"] = validationResult.Errors
                    }
                }),
            _ => result.Error.ToProblemDetails()
        };
    }

    public static IResult ToProblemDetails(this Error error)
    {
        return Results.Problem(
            statusCode: GetStatusCode(error.Type),
            title: GetTitle(error.Type),
            type: GetType(error.Type),
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = new[] { error }
            });

        static int GetStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidValue => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        static string GetTitle(ErrorType errorType) => errorType switch
        {
            ErrorType.Conflict => "Conflict",
            ErrorType.NotFound => "Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.InvalidValue => "Bad Request",
            _ => "Internal Server Error"
        };

        static string GetType(ErrorType errorType) => errorType switch
        {
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            ErrorType.InvalidValue => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
    }
}