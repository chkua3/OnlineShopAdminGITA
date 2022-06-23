using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.DataAccess.Models;

namespace OnlineShopAdmin.Services.Products.Messages;

public class GetProductsRequest : Request
{
    public string Name { get; set; }

    public string ProductNumber { get; set; }

    public string Color { get; set; }

    public decimal? StandardCost { get; set; }

    public decimal? ListPrice { get; set; }

    public string Size { get; set; }

    public decimal? Weight { get; set; }
}

public class GetProductsResponse
{
    public Product Product { get; set; }

    public int OrdersQuantity { get; set; }
}