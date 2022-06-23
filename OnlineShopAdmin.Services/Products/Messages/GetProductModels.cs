namespace OnlineShopAdmin.Services.Products.Messages;

public class GetProductModels
{
    public List<GetProductModel> ProductModels { get; set; }
}

public class GetProductModel
{
    public int ProductModelId { get; set; }

    public string Name { get; set; }
}