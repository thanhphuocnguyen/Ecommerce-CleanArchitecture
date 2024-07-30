using Domain;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Value);

        builder
            .HasMany(r => r.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder
            .HasMany(r => r.Users)
            .WithMany(r => r.Roles);

        builder.HasData(Role.GetValues());
    }
}