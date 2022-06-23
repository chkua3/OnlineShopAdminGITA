using OnlineShopAdmin.Services.Customers.Models;

namespace OnlineShopAdmin.Services.Reports.Messages;

public class GetCustomersBySalesAmountResponse
{
    public List<GetCustomerBySalesAmountResponse> CustomerBySalesAmount { get; set; }
}

public class GetCustomerBySalesAmountResponse
{
    public CustomerResponseModel Customer { get; set; }

    public int ToTalSales { get; set; }
}