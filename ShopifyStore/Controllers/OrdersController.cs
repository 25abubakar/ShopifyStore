using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopifyStore.Data;
using ShopifyStore.Models;

namespace ShopifyStore.Controllers;

[Authorize(Roles = "CEO,Admin,Employee")]
public class OrdersController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var orders = await db.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        return View(orders);
    }

    [HttpPost]
    [Authorize(Roles = "CEO,Admin")]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var order = await db.Orders.FindAsync(id);
        if (order is null) return NotFound();

        if (!IsValidStatusTransition(orderStatus, paymentStatus))
        {
            TempData["OrderStatusError"] = "Invalid status combination selected.";
            return RedirectToAction(nameof(Index));
        }

        order.OrderStatus = orderStatus;
        order.PaymentStatus = paymentStatus;
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private static bool IsValidStatusTransition(OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        if (orderStatus == OrderStatus.PaymentUnderReview
            && paymentStatus is not (PaymentStatus.VerificationPending or PaymentStatus.GatewayPending))
        {
            return false;
        }

        if (orderStatus == OrderStatus.Delivered
            && paymentStatus is not (PaymentStatus.Paid or PaymentStatus.Unpaid))
        {
            return false;
        }

        if (orderStatus == OrderStatus.Cancelled
            && paymentStatus == PaymentStatus.Paid)
        {
            return false;
        }

        return true;
    }
}
