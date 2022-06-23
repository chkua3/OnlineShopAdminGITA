using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.DataAccess.Models;

namespace OnlineShopAdmin.Services.ProductCategories.Messages;

public class GetProductCategoriesRequest : Request
{
    public string Name { get; set; }

    public string ParentProductCategoryName { get; set; }
}

public class GetProductCategoriesResponse
{
    public ProductCategory ProductCategory { get; set; }

    public int ProductInCategory { get; set; }
}