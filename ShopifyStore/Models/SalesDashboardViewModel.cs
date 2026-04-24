namespace ShopifyStore.Models;

public class SalesDashboardViewModel
{
    public string RoleName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalSalesPkr { get; set; }
    public int TotalProducts { get; set; }
    public int AvailableProducts { get; set; }
    public int SoldOutProducts { get; set; }
    public List<TopSellingProductRow> TopProducts { get; set; } = [];
    public List<Order> RecentOrders { get; set; } = [];
}

public class TopSellingProductRow
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int SoldQty { get; set; }
}
