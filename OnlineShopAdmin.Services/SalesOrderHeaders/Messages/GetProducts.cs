namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class GetProducts
{
    public List<GetProduct> Products { get; set; }
}

public class GetProduct
{
    public int ProductId { get; set; }

    public string Name { get; set; }
}