using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data.Repositories;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Cart> Carts { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<LineItem> LineItems { get; set; } = null!;

    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;

    public DbSet<PaymentType> PaymentTypes { get; set; } = null!;

    public DbSet<Address> Addresses { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}