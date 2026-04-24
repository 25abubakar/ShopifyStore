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
    public string HeroTitle { get; set; } = "New Sustainable Knits & Styles";
    public string HeroSubtitle { get; set; } = "Premium quality fashion for Pakistan with flexible payment options.";
    public string HeroImageUrl { get; set; } = "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=2000&q=80";
}
