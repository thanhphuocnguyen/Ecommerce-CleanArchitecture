namespace Ecommerce.Domain.Enums;

public enum Permission
{
    /// <summary>
    /// Represents the permissions related to the access shop.
    /// </summary>
    ModifyShop = 1,

    /// <summary>
    /// Represents the permissions related to the access admin.
    /// </summary>
    ModifyUser = 2,

    /// <summary>
    /// Represents the permissions related to the access user.
    /// </summary>
    ShopAccess = 3,
}