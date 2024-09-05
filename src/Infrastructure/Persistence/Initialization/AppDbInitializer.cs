using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Infrastructure.Persistence.Initialization;

public class AppDbInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly AppDbSeeder _dbSeeder;
    private readonly ILogger<AppDbInitializer> _logger;

    public AppDbInitializer(ApplicationDbContext dbContext, AppDbSeeder dbSeeder, ILogger<AppDbInitializer> logger)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                _logger.LogInformation("Initializing database...");
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (!await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Seeding initial data...");

                await _dbSeeder.SeedAsync(_dbContext, cancellationToken);
            }

            _logger.LogInformation("Database initialized.");
        }
    }
}