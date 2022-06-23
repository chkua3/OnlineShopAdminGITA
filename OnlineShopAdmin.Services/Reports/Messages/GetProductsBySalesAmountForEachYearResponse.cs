using OnlineShopAdmin.Services.Products.Models;

namespace OnlineShopAdmin.Services.Reports.Messages;

public class GetProductsBySalesAmountForEachYearResponse
{
    public List<GetProductBySalesAmountForEachYearResponse> ProductsBySalesAmountForEachYear { get; set; }
}

public class GetProductBySalesAmountForEachYearResponse
{
    public ProductResponseModel Product { get; set; }
    public int OrderYear { get; set; }
}
