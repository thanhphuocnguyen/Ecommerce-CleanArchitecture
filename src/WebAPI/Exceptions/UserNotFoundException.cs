using Ecommerce.Domain.Shared.Result;

namespace Ecommerce.WebAPI.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string userId)
    : base("User not found", [new Error("user_not_found", $"User with id {userId} not found", ErrorType.Conflict)])
    {
    }
}