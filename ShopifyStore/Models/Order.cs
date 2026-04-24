using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public enum PaymentMethod
{
    CashOnDelivery = 1,
    BankTransfer = 2,
    EasypaisaOrJazzCash = 3,
    OnlineGateway = 4
}

public enum OrderStatus
{
    Pending = 1,
    PaymentUnderReview = 2,
    Confirmed = 3,
    Dispatched = 4,
    Delivered = 5,
    Cancelled = 6
}

public enum PaymentStatus
{
    Unpaid = 1,
    Paid = 2,
    VerificationPending = 3,
    Rejected = 4,
    GatewayPending = 5,
    GatewayFailed = 6
}

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [StringLength(60)]
    public string BankTransactionId { get; set; } = string.Empty;

    [StringLength(200)]
    public string PaymentScreenshotUrl { get; set; } = string.Empty;

    [StringLength(80)]
    public string PaymentReference { get; set; } = string.Empty;

    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    public decimal TotalAmountPkr { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> Items { get; set; } = [];
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPricePkr { get; set; }
}
