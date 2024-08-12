using System.Linq.Expressions;

namespace Ecommerce.Application.Common.Interfaces;

public interface IJobService
{
    void Enqueue(Expression<Func<Task>> methodCall);
}