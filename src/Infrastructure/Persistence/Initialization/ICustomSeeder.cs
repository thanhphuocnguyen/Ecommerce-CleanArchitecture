using System;

namespace Ecommerce.Infrastructure.Persistence.Initialization;

public interface ICustomSeeder
{
    Task InitializeAsync(CancellationToken cancellationToken);
}