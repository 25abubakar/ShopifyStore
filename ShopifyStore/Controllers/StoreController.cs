using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

public class StoreController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? category)
    {
        var query = db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(x => x.Category == category);
        }

        ViewBag.Categories = await db.Products.Select(x => x.Category).Distinct().OrderBy(x => x).ToListAsync();
        ViewBag.ActiveCategory = category;
        return View(await query.OrderBy(x => x.Name).ToListAsync());
    }

    public async Task<IActionResult> Product(int id)
    {
        var product = await db.Products
            .Include(x => x.Reviews.OrderByDescending(r => r.CreatedAt))
            .FirstOrDefaultAsync(x => x.Id == id);
        if (product is null) return NotFound();
        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> Checkout(int productId)
    {
        var product = await db.Products.FindAsync(productId);
        if (product is null) return NotFound();
        ViewBag.Product = product;
        return View(new CheckoutViewModel { ProductId = productId, Quantity = 1 });
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var product = await db.Products.FindAsync(model.ProductId);
        if (product is null) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.Product = product;
            return View(model);
        }

        if (product.Stock < model.Quantity)
        {
            ModelState.AddModelError(string.Empty, "Requested quantity is not available in stock.");
            ViewBag.Product = product;
            return View(model);
        }

        var total = product.PricePkr * model.Quantity;
        var order = new Order
        {
            CustomerName = model.CustomerName,
            Phone = model.Phone,
            Address = model.Address,
            PaymentMethod = model.PaymentMethod,
            TotalAmountPkr = total,
            Items =
            [
                new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = model.Quantity,
                    UnitPricePkr = product.PricePkr
                }
            ]
        };

        product.Stock -= model.Quantity;
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(OrderSuccess), new { id = order.Id });
    }

    public async Task<IActionResult> OrderSuccess(int id)
    {
        var order = await db.Orders.Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        if (order is null) return NotFound();
        return View(order);
    }
}
