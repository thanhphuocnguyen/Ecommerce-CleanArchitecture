namespace Ecommerce.Domain.Identity.Users.Contracts;

public class UserRolesRequest
{
    public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
}