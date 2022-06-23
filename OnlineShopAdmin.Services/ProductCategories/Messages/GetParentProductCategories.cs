namespace OnlineShopAdmin.Services.ProductCategories.Messages;

public class GetParentProductCategories
{
    public List<GetParentProductCategory> ParentProductCategories { get; set; }
}

public class GetParentProductCategory
{
    public int ProductCategoryId { get; set; }

    public string Name { get; set; }
}