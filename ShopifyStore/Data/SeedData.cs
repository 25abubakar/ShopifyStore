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
                new Product { Name = "Men Casual Shirt", Category = "Men", Description = "Premium cotton shirt for daily wear.", PricePkr = 2499, Stock = 50, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab" },
                new Product { Name = "Women Smart Watch", Category = "Women", Description = "Fitness tracking and notifications.", PricePkr = 15999, Stock = 30, ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30" },
                new Product { Name = "Organic Face Wash", Category = "Accessories", Description = "Hydrating skin care product.", PricePkr = 1199, Stock = 80, ImageUrl = "https://images.unsplash.com/photo-1556228578-8c89e6adf883" },
                new Product { Name = "Kids Learning Kit", Category = "Kids", Description = "Creative educational activity box.", PricePkr = 3499, Stock = 40, ImageUrl = "https://images.unsplash.com/photo-1587654780291-39c9404d746b" }
            );
        }

        db.SaveChanges();
    }
}
