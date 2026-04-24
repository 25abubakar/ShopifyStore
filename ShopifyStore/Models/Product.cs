using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 10000000)]
    public decimal PricePkr { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    [Required, StringLength(120)]
    public string Category { get; set; } = string.Empty;

    [StringLength(300)]
    public string ImageUrl { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = [];
}
