using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Shared.Primitives;

public interface IAuditableEntity
{
    DateTimeOffset Created { get; set; }

    DateTimeOffset? LastModified { get; set; }
}