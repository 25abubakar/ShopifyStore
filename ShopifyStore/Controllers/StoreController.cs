using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

public class StoreController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(
        string? department,
        string? category,
        string? subcategory,
        string? search,
        decimal? minPrice,
        decimal? maxPrice,
        bool inStockOnly = false,
        string sortBy = "name")
    {
        var query = db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(department))
        {
            query = query.Where(x => x.Department.ToLower() == department.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(x => x.Category.ToLower() == category.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(subcategory))
        {
            query = query.Where(x => x.Subcategory.ToLower() == subcategory.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.Trim().ToLower();
            query = query.Where(x =>
                x.Name.ToLower().Contains(normalized)
                || x.Description.ToLower().Contains(normalized)
                || x.Brand.ToLower().Contains(normalized)
                || x.Sku.ToLower().Contains(normalized));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(x => x.PricePkr >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(x => x.PricePkr <= maxPrice.Value);
        }

        if (inStockOnly)
        {
            query = query.Where(x => x.Stock > 0);
        }

        query = query.Where(x => x.IsActive);

        query = sortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(x => x.PricePkr),
            "price_desc" => query.OrderByDescending(x => x.PricePkr),
            "newest" => query.OrderByDescending(x => x.Id),
            _ => query.OrderBy(x => x.Name)
        };

        var model = new StoreIndexViewModel
        {
            Products = await query.ToListAsync(),
            Departments = await db.Products.Select(x => x.Department).Where(x => x != "").Distinct().OrderBy(x => x).ToListAsync(),
            Categories = await db.Products.Select(x => x.Category).Where(x => x != "").Distinct().OrderBy(x => x).ToListAsync(),
            Subcategories = await db.Products.Select(x => x.Subcategory).Where(x => x != "").Distinct().OrderBy(x => x).ToListAsync(),
            Department = department ?? string.Empty,
            Category = category ?? string.Empty,
            Subcategory = subcategory ?? string.Empty,
            Search = search ?? string.Empty,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            InStockOnly = inStockOnly,
            SortBy = sortBy
        };

        var activeDepartment = department?.Trim() ?? string.Empty;
        (model.HeroTitle, model.HeroSubtitle, model.HeroImageUrl) = activeDepartment.ToLowerInvariant() switch
        {
            "men" => ("Men's New Collection", "Premium men styles, footwear and essentials.", "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?auto=format&fit=crop&w=2000&q=80"),
            "women" => ("Women's Latest Arrivals", "Elegant women wear and fashion picks.", "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=2000&q=80"),
            "kids" => ("Kids Collection", "Comfortable and colorful styles for kids.", "https://images.unsplash.com/photo-1519238359922-989348752efb?auto=format&fit=crop&w=2000&q=80"),
            "accessories" => ("Accessories Picks", "Bags, belts and watches for every look.", "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?auto=format&fit=crop&w=2000&q=80"),
            "beauty" => ("Beauty Essentials", "Skincare, makeup and personal care products.", "https://images.unsplash.com/photo-1596462502278-27bfdc403348?auto=format&fit=crop&w=2000&q=80"),
            _ => ("New Sustainable Knits & Styles", "Premium quality fashion for Pakistan with COD, transfer and digital wallet checkout options.", "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=2000&q=80")
        };

        return View(model);
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

        if (model.PaymentMethod is PaymentMethod.BankTransfer or PaymentMethod.EasypaisaOrJazzCash)
        {
            if (string.IsNullOrWhiteSpace(model.PaymentReference))
            {
                ModelState.AddModelError(nameof(model.PaymentReference), "Payment reference is required for manual transfer methods.");
            }

            if (string.IsNullOrWhiteSpace(model.PaymentScreenshotUrl))
            {
                ModelState.AddModelError(nameof(model.PaymentScreenshotUrl), "Payment screenshot URL is required for manual transfer methods.");
            }
        }

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
        var isManualTransfer = model.PaymentMethod is PaymentMethod.BankTransfer or PaymentMethod.EasypaisaOrJazzCash;
        var isGateway = model.PaymentMethod == PaymentMethod.OnlineGateway;
        var order = new Order
        {
            CustomerName = model.CustomerName,
            Phone = model.Phone,
            Address = model.Address,
            PaymentMethod = model.PaymentMethod,
            BankTransactionId = isManualTransfer ? model.PaymentReference : string.Empty,
            PaymentScreenshotUrl = isManualTransfer ? model.PaymentScreenshotUrl : string.Empty,
            PaymentReference = model.PaymentReference,
            PaymentStatus = isManualTransfer
                ? PaymentStatus.VerificationPending
                : isGateway
                    ? PaymentStatus.GatewayPending
                    : PaymentStatus.Unpaid,
            OrderStatus = isManualTransfer ? OrderStatus.PaymentUnderReview : OrderStatus.Pending,
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
