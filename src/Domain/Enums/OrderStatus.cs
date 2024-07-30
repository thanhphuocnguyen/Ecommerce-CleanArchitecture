namespace Ecommerce.Domain.Enums;

/// <summary>
/// Represents the status of an order.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// The order is new.
    /// </summary>
    New,

    /// <summary>
    /// The order has been paid.
    /// </summary>
    Paid,

    /// <summary>
    /// The order has been shipped.
    /// </summary>
    Shipped,

    /// <summary>
    /// The order has been delivered.
    /// </summary>
    Delivered,

    /// <summary>
    /// The order has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The order has been returned.
    /// </summary>
    Returned,

    /// <summary>
    /// The order has been refunded.
    /// </summary>
    Refunded
}