using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Users;

public record GetUserQuery(UserId Id) : IQuery<UserResponse>
{
}