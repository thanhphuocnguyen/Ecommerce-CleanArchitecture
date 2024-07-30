using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        IEnumerable<Permission> permissions = Enum
            .GetValues<Domain.Enums.Permission>()
            .Select(p => new Permission { Id = (int)p, Name = p.ToString() }).ToArray();

        builder.HasData(permissions);
    }
}