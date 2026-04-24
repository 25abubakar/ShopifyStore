using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

[Authorize(Roles = "CEO,Admin,Employee")]
public class DashboardController(AppDbContext db) : Controller
{
    [Authorize(Roles = "CEO")]
    public Task<IActionResult> CEO() => BuildDashboard("CEO");

    [Authorize(Roles = "Admin")]
    public Task<IActionResult> Admin() => BuildDashboard("Admin");

    [Authorize(Roles = "Employee")]
    public Task<IActionResult> Employee() => BuildDashboard("Employee");

    private async Task<IActionResult> BuildDashboard(string roleName)
    {
        var model = new SalesDashboardViewModel
        {
            RoleName = roleName,
            TotalOrders = await db.Orders.CountAsync(),
            TotalSalesPkr = await db.Orders.SumAsync(x => (decimal?)x.TotalAmountPkr) ?? 0m,
            TotalProducts = await db.Products.CountAsync(),
            AvailableProducts = await db.Products.CountAsync(x => x.Stock > 0),
            SoldOutProducts = await db.Products.CountAsync(x => x.Stock <= 0),
            TopProducts = await db.OrderItems
                .Include(x => x.Product)
                .GroupBy(x => new { x.ProductId, ProductName = x.Product != null ? x.Product.Name : "Unknown" })
                .Select(x => new TopSellingProductRow
                {
                    ProductId = x.Key.ProductId,
                    ProductName = x.Key.ProductName,
                    SoldQty = x.Sum(i => i.Quantity)
                })
                .OrderByDescending(x => x.SoldQty)
                .Take(5)
                .ToListAsync(),
            RecentOrders = await db.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync()
        };

        return View("Index", model);
    }
}
