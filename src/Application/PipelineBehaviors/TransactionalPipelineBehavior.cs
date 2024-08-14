using System.Data;
using Ecommerce.Application.Common.Commands;
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
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting transaction for {RequestName}", typeof(TRequest).Name);

        using IDbTransaction transaction = await _unitOfWork.BeginTransactionAsync();

        TResponse response = await next();

        transaction.Commit();

        _logger.LogInformation("Transaction committed for {RequestName}", typeof(TRequest).Name);

        return response;
    }
}