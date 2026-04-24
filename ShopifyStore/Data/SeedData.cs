using ShopifyStore.Models;

namespace ShopifyStore.Data;

public static class SeedData
{
    public static void EnsureSeeded(AppDbContext db)
    {
        if (!db.Users.Any())
        {
            db.Users.AddRange(
                new AppUser { FullName = "Company CEO", Username = "ceo", Password = "ceo123", Role = UserRole.CEO },
                new AppUser { FullName = "Store Admin", Username = "admin", Password = "admin123", Role = UserRole.Admin },
                new AppUser { FullName = "Store Employee", Username = "employee", Password = "emp123", Role = UserRole.Employee }
            );
        }

        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Men Casual Shirt", Department = "Men", Category = "Clothing", Subcategory = "Shirts", Description = "Premium cotton shirt for daily wear.", PricePkr = 2499, Stock = 50, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab" },
                new Product { Name = "Men Chino Pants", Department = "Men", Category = "Clothing", Subcategory = "Pants", Description = "Slim fit chino pants for smart casual looks.", PricePkr = 3299, Stock = 35, ImageUrl = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a" },
                new Product { Name = "Men Casual Sneakers", Department = "Men", Category = "Footwear", Subcategory = "Casual Shoes", Description = "Comfortable all-day casual sneakers.", PricePkr = 5999, Stock = 28, ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff" },
                new Product { Name = "Women Everyday Top", Department = "Women", Category = "Clothing", Subcategory = "Shirts", Description = "Soft breathable fabric for daily wear.", PricePkr = 2199, Stock = 52, ImageUrl = "https://images.unsplash.com/photo-1483985988355-763728e1935b" },
                new Product { Name = "Women Straight Pants", Department = "Women", Category = "Clothing", Subcategory = "Pants", Description = "Formal straight pants with stretch comfort.", PricePkr = 2999, Stock = 30, ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246" },
                new Product { Name = "Women Flat Shoes", Department = "Women", Category = "Footwear", Subcategory = "Flats", Description = "Comfort flats with durable sole.", PricePkr = 3899, Stock = 25, ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2" },
                new Product { Name = "Kids Graphic Tee", Department = "Kids", Category = "Clothing", Subcategory = "T-Shirts", Description = "Colorful graphic t-shirt for kids.", PricePkr = 1399, Stock = 60, ImageUrl = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f" },
                new Product { Name = "Kids Summer Shorts", Department = "Kids", Category = "Clothing", Subcategory = "Shorts", Description = "Lightweight shorts for summer comfort.", PricePkr = 1299, Stock = 55, ImageUrl = "https://images.unsplash.com/photo-1441986300917-64674bd600d8" },
                new Product { Name = "Kids Casual Sandals", Department = "Kids", Category = "Footwear", Subcategory = "Kids Sandals", Description = "Flexible kids sandals for daily activity.", PricePkr = 2199, Stock = 32, ImageUrl = "https://images.unsplash.com/photo-1515347619252-60a4bf4fff4f" },
                new Product { Name = "Classic Leather Belt", Department = "Accessories", Category = "General", Subcategory = "Belts", Description = "Genuine leather belt with clean buckle.", PricePkr = 1499, Stock = 70, ImageUrl = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49" },
                new Product { Name = "Minimalist Wrist Watch", Department = "Accessories", Category = "General", Subcategory = "Watches", Description = "Minimal watch suitable for formal and casual outfits.", PricePkr = 7999, Stock = 26, ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30" },
                new Product { Name = "Crossbody Bag", Department = "Accessories", Category = "General", Subcategory = "Bags", Description = "Compact everyday crossbody bag.", PricePkr = 4599, Stock = 33, ImageUrl = "https://images.unsplash.com/photo-1594633312681-425c7b97ccd1" }
            );
        }

        if (!db.ProductTaxonomyEntries.Any())
        {
            var taxonomyRows = db.Products
                .Select(x => new { x.Department, x.Category, x.Subcategory })
                .Distinct()
                .ToList();

            db.ProductTaxonomyEntries.AddRange(taxonomyRows.Select(x => new ProductTaxonomyEntry
            {
                Department = x.Department,
                Category = x.Category,
                Subcategory = x.Subcategory
            }));
        }

        db.SaveChanges();
    }
}
