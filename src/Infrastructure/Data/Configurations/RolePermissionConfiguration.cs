using Domain;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Ecommerce.Domain.Enums.Permission;

namespace Ecommerce.Infrastructure.Data.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
        builder.HasData(
            Create(Role.Admin, Permission.ModifyShop),
            Create(Role.Admin, Permission.ModifyUser),
            Create(Role.Shop, Permission.ShopAccess),
            Create(Role.Shop, Permission.ModifyShop));
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission { RoleId = role.Value, PermissionId = (int)permission };
    }
}