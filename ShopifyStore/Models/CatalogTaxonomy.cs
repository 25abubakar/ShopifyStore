namespace ShopifyStore.Models;

public static class CatalogTaxonomy
{
    public static readonly Dictionary<string, Dictionary<string, string[]>> Tree = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Men"] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Clothing"] = ["Shirts", "Pants", "Undergarments"],
            ["Footwear"] = ["Casual Shoes", "Formal Shoes", "Sandals"]
        },
        ["Women"] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Clothing"] = ["Shirts", "Pants", "Undergarments", "Eastern Wear"],
            ["Footwear"] = ["Heels", "Flats", "Casual Shoes"]
        },
        ["Kids"] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Clothing"] = ["T-Shirts", "Shorts", "School Wear"],
            ["Footwear"] = ["Kids Casual", "Kids Sandals"]
        },
        ["Accessories"] = new(StringComparer.OrdinalIgnoreCase)
        {
            ["General"] = ["Bags", "Watches", "Belts"]
        }
    };

    public static bool IsValid(string department, string category, string subcategory)
    {
        return Tree.TryGetValue(department, out var categories)
            && categories.TryGetValue(category, out var subcategories)
            && subcategories.Contains(subcategory, StringComparer.OrdinalIgnoreCase);
    }
}
