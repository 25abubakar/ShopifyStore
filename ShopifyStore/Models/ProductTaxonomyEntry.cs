using System.ComponentModel.DataAnnotations;

namespace ShopifyStore.Models;

public class ProductTaxonomyEntry
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Subcategory { get; set; } = string.Empty;
}
