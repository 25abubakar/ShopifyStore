using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public enum PaymentMethod
{
    CashOnDelivery = 1,
    BankTransfer = 2
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
