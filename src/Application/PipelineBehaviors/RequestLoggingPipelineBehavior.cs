using Ecommerce.Domain.Shared;
using Ecommerce.Domain.Shared.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Ecommerce.Application.Behaviors;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : Result
{
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {RequestName}", requestName);

        TResponse response = await next();

        if (response.IsFailure)
        {
            using (LogContext.PushProperty("Error", requestName))
            {
                _logger.LogInformation("Handled {RequestName} ({@Response})", requestName, response);
            }
        }
        else
        {
            _logger.LogInformation("Handled {RequestName}", requestName);
        }

        return response;
    }
}