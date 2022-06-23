using OnlineShopAdmin.Services.Products.Models;

namespace OnlineShopAdmin.Services.Reports.Messages;

public class GetProductsBySalesAmountResponse
{
    public List<GetProductBySalesAmountResponse> ProductsBySalesAmount { get; set; }
}

public class GetProductBySalesAmountResponse
{
    public ProductResponseModel Product { get; set; }

    public int ToTalSales { get; set; }
}