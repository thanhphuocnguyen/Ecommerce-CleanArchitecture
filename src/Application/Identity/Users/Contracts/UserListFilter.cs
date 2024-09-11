using Ecommerce.Domain.Shared;

namespace Ecommerce.Domain.Identity.Users.Contracts;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}