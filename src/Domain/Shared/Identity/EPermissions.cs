namespace Ecommerce.Domain.Shared.Identity;

public static class EPermissions
{
    private static readonly EPermission[] _all = [
        new EPermission("View products", EAction.View, Resource.Product, IsBasic: true),
        new EPermission("Search products", EAction.Search, Resource.Product, IsBasic: true),
        new EPermission("Create products", EAction.Create, Resource.Product),
        new EPermission("Update products", EAction.Update, Resource.Product),
        new EPermission("Delete products", EAction.Delete, Resource.Product),
        new EPermission("Export products", EAction.Export, Resource.Product),
        new EPermission("Generate products", EAction.Generate, Resource.Product),
        new EPermission("Clean products", EAction.Clean, Resource.Product),

        new EPermission("View orders", EAction.View, Resource.Order, IsBasic: true),
        new EPermission("Search orders", EAction.Search, Resource.Order, IsBasic: true),
        new EPermission("Create orders", EAction.Create, Resource.Order, IsBasic: true),
        new EPermission("Update orders", EAction.Update, Resource.Order),
        new EPermission("Delete orders", EAction.Delete, Resource.Order),
        new EPermission("Export orders", EAction.Export, Resource.Order),

        new EPermission("View users", EAction.View, Resource.User, IsRoot: true),
        new EPermission("Search users", EAction.Search, Resource.User, IsRoot: true),
        new EPermission("Create users", EAction.Create, Resource.User, IsBasic: true),
        new EPermission("Update users", EAction.Update, Resource.User, IsRoot: true),
        new EPermission("Delete users", EAction.Delete, Resource.User, IsRoot: true),
        new EPermission("Export users", EAction.Export, Resource.User, IsRoot: true),

        new EPermission("View roles", EAction.View, Resource.Role, IsRoot: true),
        new EPermission("Search roles", EAction.Search, Resource.Role, IsRoot: true),
        new EPermission("Create roles", EAction.Create, Resource.Role, IsRoot: true),
        new EPermission("Update roles", EAction.Update, Resource.Role, IsRoot: true),
        new EPermission("Delete roles", EAction.Delete, Resource.Role, IsRoot: true),

        new EPermission("View permissions", EAction.View, Resource.Permission, IsRoot: true),
        new EPermission("Search permissions", EAction.Search, Resource.Permission, IsRoot: true),
        new EPermission("Create permissions", EAction.Create, Resource.Permission, IsRoot: true),
        new EPermission("Update permissions", EAction.Update, Resource.Permission, IsRoot: true),
        new EPermission("Delete permissions", EAction.Delete, Resource.Permission, IsRoot: true),
    ];

    public static IReadOnlyList<EPermission> All => _all;

    public static IReadOnlyList<EPermission> Basic => All.Where(p => p.IsBasic).ToList();

    public static IReadOnlyList<EPermission> Root => All.Where(p => p.IsRoot).ToList();

    public static IReadOnlyList<EPermission> For(string resource) => All.Where(p => p.Resource == resource).ToList();

    public static EPermission? Find(string action, string resource) => All.SingleOrDefault(p => p.Action == action && p.Resource == resource);
}

public record EPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);

    public static string NameFor(string action, string resource) => $"Perms.{resource}.{action}";
}

public static class EAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
}

public static class Resource
{
    public const string Product = nameof(Product);
    public const string Order = nameof(Order);
    public const string User = nameof(User);
    public const string Role = nameof(Role);
    public const string Permission = nameof(Permission);
    public const string Vendor = nameof(Vendor);
    public const string Moderator = nameof(Moderator);
    public const string Admin = nameof(Admin);
    public const string Category = nameof(Category);
    public const string Brand = nameof(Brand);
    public const string Customer = nameof(Customer);
    public const string Address = nameof(Address);
    public const string Cart = nameof(Cart);
    public const string CartItem = nameof(CartItem);
    public const string Review = nameof(Review);
    public const string Wishlist = nameof(Wishlist);
    public const string Notification = nameof(Notification);
    public const string Setting = nameof(Setting);
    public const string File = nameof(File);
    public const string Log = nameof(Log);
    public const string Audit = nameof(Audit);
    public const string Report = nameof(Report);
    public const string Dashboard = nameof(Dashboard);
    public const string Payment = nameof(Payment);
    public const string Shipping = nameof(Shipping);
    public const string Tax = nameof(Tax);
    public const string Coupon = nameof(Coupon);
    public const string Discount = nameof(Discount);
    public const string Refund = nameof(Refund);
    public const string Exchange = nameof(Exchange);
    public const string Return = nameof(Return);
    public const string Transaction = nameof(Transaction);
    public const string Invoice = nameof(Invoice);
    public const string OrderItem = nameof(OrderItem);
    public const string ProductCategory = nameof(ProductCategory);
    public const string ProductBrand = nameof(ProductBrand);
    public const string ProductReview = nameof(ProductReview);
    public const string ProductWishlist = nameof(ProductWishlist);
    public const string ProductImage = nameof(ProductImage);
    public const string ProductVariant = nameof(ProductVariant);
    public const string ProductAttribute = nameof(ProductAttribute);
    public const string ProductAttributeGroup = nameof(ProductAttributeGroup);
    public const string ProductAttributeOption = nameof(ProductAttributeOption);
    public const string ProductAttributeSet = nameof(ProductAttributeSet);
    public const string ProductAttributeSetGroup = nameof(ProductAttributeSetGroup);
    public const string ProductAttributeSetOption = nameof(ProductAttributeSetOption);
    public const string ProductAttributeSetGroupOption = nameof(ProductAttributeSetGroupOption);
}