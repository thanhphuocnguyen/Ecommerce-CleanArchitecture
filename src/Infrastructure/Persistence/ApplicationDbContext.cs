using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence;

public sealed class ApplicationDbContext
    : IdentityDbContext<AppUser,
                        AppRole,
                        Guid,
                        IdentityUserClaim<Guid>,
                        IdentityUserRole<Guid>,
                        IdentityUserLogin<Guid>,
                        AppRoleClaim,
                        IdentityUserToken<Guid>>
{
    public DbSet<Product> Products => Set<Product>();

    public DbSet<Cart> Carts => Set<Cart>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<LineItem> LineItems => Set<LineItem>();

    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();

    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();

    public DbSet<Address> Addresses => Set<Address>();

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}