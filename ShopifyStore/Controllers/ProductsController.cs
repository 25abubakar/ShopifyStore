using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

[Authorize(Roles = "CEO,Admin")]
public class ProductsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await db.Products.OrderBy(x => x.Name).ToListAsync());
    }

    [Authorize(Roles = "CEO,Admin")]
    public IActionResult Create()
    {
        PopulateTaxonomy();
        return View(new Product());
    }

    [HttpPost]
    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Create(Product product)
    {
        ValidateTaxonomy(product);
        if (!ModelState.IsValid)
        {
            PopulateTaxonomy();
            return View(product);
        }
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        PopulateTaxonomy();
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Edit(Product product)
    {
        ValidateTaxonomy(product);
        if (!ModelState.IsValid)
        {
            PopulateTaxonomy();
            return View(product);
        }
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

    private void ValidateTaxonomy(Product product)
    {
        if (!CatalogTaxonomy.IsValid(product.Department, product.Category, product.Subcategory))
        {
            ModelState.AddModelError(string.Empty, "Invalid department/category/subcategory combination.");
        }
    }

    private void PopulateTaxonomy()
    {
        ViewBag.Taxonomy = CatalogTaxonomy.Tree;
    }
}
