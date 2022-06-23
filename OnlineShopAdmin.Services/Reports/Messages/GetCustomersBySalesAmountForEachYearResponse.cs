using OnlineShopAdmin.Services.Customers.Models;

namespace OnlineShopAdmin.Services.Reports.Messages;

public class GetCustomersBySalesAmountForEachYearResponse
{
    public List<GetCustomerBySalesAmountForEachYearResponse> CustomerBySalesAmountForEachYear { get; set; }
}

public class GetCustomerBySalesAmountForEachYearResponse
{
    public CustomerResponseModel Customer { get; set; }

    public int OrderYear { get; set; }
}