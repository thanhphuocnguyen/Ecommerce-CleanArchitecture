using Ecommerce.Application.Contracts;

namespace Ecommerce.Application;

public interface ITransactionalCommand : ICommand
{
}

public interface ITransactionalCommand<TResponse> : ICommand<TResponse>, ITransactionalCommand
{
}