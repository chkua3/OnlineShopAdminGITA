using OnlineShopAdmin.Services.Products.Models;

namespace OnlineShopAdmin.Services.Reports.Messages;

public class GetProductsBySalesProfitResponse
{
    public List<GetProductBySalesProfitResponse> ProductBySalesProfit { get; set; }
}

public class GetProductBySalesProfitResponse
{
    public ProductResponseModel Product { get; set; }

    public decimal ToTalProfit { get; set; }
}