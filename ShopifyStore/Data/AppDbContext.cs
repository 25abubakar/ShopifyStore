using Microsoft.EntityFrameworkCore;
using ShopifyStore.Models;

namespace ShopifyStore.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<ProductTaxonomyEntry> ProductTaxonomyEntries => Set<ProductTaxonomyEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(x => x.Department)
            .HasMaxLength(80);

        modelBuilder.Entity<Product>()
            .Property(x => x.Subcategory)
            .HasMaxLength(120);

        modelBuilder.Entity<ProductTaxonomyEntry>()
            .HasIndex(x => new { x.Department, x.Category, x.Subcategory })
            .IsUnique();
    }
}
