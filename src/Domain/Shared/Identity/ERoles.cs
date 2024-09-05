namespace Ecommerce.Domain.Shared;

public static class ERoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Vendor = "Vendor";
    public const string Moderator = "Moderator";

    public static IReadOnlyList<string> DefaultRoles => new List<string>
    {
        Admin,
        User,
        Vendor,
    };

    public static IReadOnlyList<string> AllRoles => new List<string>
    {
        Admin,
        User,
        Vendor,
        Moderator,
    };

    public static bool IsValidRole(string role) => AllRoles.Contains(role);

    public static bool IsDefaultRole(string role) => DefaultRoles.Contains(role);
}