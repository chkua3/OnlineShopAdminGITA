namespace OnlineShopAdmin.Services.Products.Messages;

public class GetProductCategories
{
    public List<GetProductCategory> ProductCategory { get; set; }
}

public class GetProductCategory
{
    public int ProductCategoryId { get; set; }

    public string Name { get; set; }
}