using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Domain.Shared.Primitives;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Infrastructure.Identity.Entities;

public class AppUser : IdentityUser<Guid>, IAggregateRoot
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}