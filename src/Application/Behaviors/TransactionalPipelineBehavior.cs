using System.Data;
using Ecommerce.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Behaviors;

public class TransactionalPipelineBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting transaction for {RequestName}", typeof(TRequest).Name);

        using IDbTransaction transaction = await unitOfWork.BeginTransactionAsync();

        TResponse response = await next();

        transaction.Commit();

        logger.LogInformation("Transaction committed for {RequestName}", typeof(TRequest).Name);

        return response;
    }
}