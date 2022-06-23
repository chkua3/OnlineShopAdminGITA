namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class GetCustomers
{
    public List<GetCustomer> Customers { get; set; }
}

public class GetCustomer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; }
}