using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

[Authorize(Roles = "CEO,Admin,Employee")]
public class ProductsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await db.Products.OrderBy(x => x.Name).ToListAsync());
    }

    [Authorize(Roles = "CEO,Admin")]
    public IActionResult Create() => View(new Product());

    [HttpPost]
    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Edit(Product product)
    {
        if (!ModelState.IsValid) return View(product);
        db.Products.Update(product);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "CEO")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "CEO")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
