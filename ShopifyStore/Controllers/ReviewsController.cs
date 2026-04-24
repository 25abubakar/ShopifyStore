using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

public class ReviewsController(AppDbContext db) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create(Review review)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Product", "Store", new { id = review.ProductId });
        }

        db.Reviews.Add(review);
        await db.SaveChangesAsync();
        return RedirectToAction("Product", "Store", new { id = review.ProductId });
    }

    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Index()
    {
        var reviews = await db.Reviews.Include(x => x.Product).OrderByDescending(x => x.CreatedAt).ToListAsync();
        return View(reviews);
    }

    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await db.Reviews.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        if (review is null) return NotFound();
        return View(review);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await db.Reviews.FindAsync(id);
        if (review is null) return NotFound();
        db.Reviews.Remove(review);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
