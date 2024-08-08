using Ecommerce.Domain.Shared;

namespace Ecommerce.Application.Identity.Users.Contracts;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}