using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Sku { get; set; } = string.Empty;

    [StringLength(80)]
    public string Brand { get; set; } = string.Empty;

    [StringLength(40)]
    public string Color { get; set; } = string.Empty;

    [StringLength(30)]
    public string Size { get; set; } = string.Empty;

    [StringLength(80)]
    public string Material { get; set; } = string.Empty;

    [StringLength(80)]
    public string CountryOfOrigin { get; set; } = "Pakistan";

    [StringLength(500)]
    public string CareInstructions { get; set; } = string.Empty;

    [Range(1, 10000000)]
    public decimal PricePkr { get; set; }

    [Range(0, 95)]
    public decimal DiscountPercent { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; }

    [Required, StringLength(120)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Subcategory { get; set; } = string.Empty;

    [StringLength(300)]
    public string ImageUrl { get; set; } = string.Empty;

    public List<Review> Reviews { get; set; } = [];
}
