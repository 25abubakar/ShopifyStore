using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public class Review
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string CustomerName { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }

    [Required, StringLength(800)]
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}
