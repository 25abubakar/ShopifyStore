namespace ShopifyStore.Models;

public class ProductAdminRowViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Subcategory { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal PricePkr { get; set; }
    public int Stock { get; set; }
    public int QtySold { get; set; }
    public int QtyHeld { get; set; }
    public int QtyAvailable => Math.Max(0, Stock - QtyHeld);
}
