namespace ShopifyStore.Models;

public class StoreIndexViewModel
{
    public List<Product> Products { get; set; } = [];
    public List<string> Departments { get; set; } = [];
    public List<string> Categories { get; set; } = [];
    public List<string> Subcategories { get; set; } = [];

    public string Department { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Subcategory { get; set; } = string.Empty;
    public string Search { get; set; } = string.Empty;
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool InStockOnly { get; set; }
    public string SortBy { get; set; } = "name";
}
