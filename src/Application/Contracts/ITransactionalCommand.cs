namespace Ecommerce.Application.Contracts;

public interface ITransactionalCommand : ICommand
{
}

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalCommand
{
}