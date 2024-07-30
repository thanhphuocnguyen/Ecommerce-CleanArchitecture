using Ecommerce.Domain;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion(
            id => id.Value,
            value => new UserId(value));

        // Value objects
        builder
            .Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .IsRequired();

        // Properties
        builder.Property(x => x.FirstName).IsRequired();

        builder.Property(x => x.LastName).IsRequired();

        builder.Property(x => x.Username).IsRequired();

        builder.Property(x => x.PasswordHash).IsRequired();

        builder.Property(x => x.ProfilePicture);

        builder
            .HasMany(x => x.Addresses)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Created).IsRequired();

        builder.Property(x => x.CreatedBy).IsRequired();

        builder.Property(x => x.LastModified);

        builder.Property(x => x.LastModifiedBy);

        // Indexes
        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.HasIndex(x => x.Username).IsUnique();
        var admin = User.Create("root", "user", "admin@example.com", "12354774", "admin", "admin");

        admin.Value.AddRole(Role.Admin);

        builder.HasData();
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => new { x.UserId, x.RoleId });
    }
}