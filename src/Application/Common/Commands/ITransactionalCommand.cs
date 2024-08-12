namespace Ecommerce.Application.Common.Commands;

public interface ITransactionalCommand : ICommand
{
}

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalCommand
{
}