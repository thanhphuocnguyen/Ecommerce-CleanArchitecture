using System.Security.Claims;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Common.Interfaces;

public interface IUserContext
{
    string? Name { get; }

    Guid? GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}