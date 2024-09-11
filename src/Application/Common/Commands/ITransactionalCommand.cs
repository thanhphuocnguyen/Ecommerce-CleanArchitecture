namespace Ecommerce.Domain.Common.Commands;

public interface ITransactionalCommand : ICommand
{
}

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalCommand
{
}