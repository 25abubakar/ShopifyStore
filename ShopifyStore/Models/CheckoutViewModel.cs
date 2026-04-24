using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public class CheckoutViewModel
{
    [Required, StringLength(150)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Address { get; set; } = string.Empty;

    [Required]
    public int ProductId { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; } = 1;

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    [StringLength(60)]
    public string BankTransactionId { get; set; } = string.Empty;

    [StringLength(200)]
    public string PaymentScreenshotUrl { get; set; } = string.Empty;
}
