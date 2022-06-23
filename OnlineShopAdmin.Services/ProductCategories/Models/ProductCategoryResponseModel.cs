namespace OnlineShopAdmin.Services.ProductCategories.Models;

public class ProductCategoryResponseModel
{
    public int ProductCategoryId { get; set; }
    
    public string Name { get; set; }

    public ProductCategoryResponseModel ParentProductCategory { get; set; }
}