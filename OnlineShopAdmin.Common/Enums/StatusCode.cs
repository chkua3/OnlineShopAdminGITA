using System.ComponentModel.DataAnnotations;

namespace OnlineShopAdmin.Common.Enums;

public enum StatusCode
{
    Error = 0,
    Success = 1,

    [Display(Name = "Product not found")]
    ProductNotFound,
    [Display(Name = "Can not delete product. There are some orders related to this product")]
    ProductCanNotDelete,
    [Display(Name = "Product with same name or number already exists")]
    ProductWithSameNameOrNumberAlreadyExists,

    [Display(Name = "Product category not found")]
    ProductCategoryNotFound,
    [Display(Name = "Can not delete category. There are some products related to this category")]
    ProductCategoryCanNotDelete,
    [Display(Name = "Product category with same name already exists")]
    ProductCategoryWithSameNameAlreadyExists,


    [Display(Name = "sales order header not found")]
    SalesOrderHeaderNotFound,
    [Display(Name = "Sales Order Header with same number already exists")]
    SalesOrderHeaderNumberWithSameNameAlreadyExists,
    [Display(Name = "Can not delete Sales Order Header. There are some Sales Order Details related to this category")]
    SalesOrderHeaderCanNotDelete,
    [Display(Name = "sales order details not found")]
    SalesOrderDetailsNotFound,

    [Display(Name = "customer not found")]
    CustomerNotFound,
    [Display(Name = "Can not delete customer. There are some  related to this customer")]
    CustomerCanNotDelete,


    [Display(Name = "address not found")]
    AddressNotFound,
    [Display(Name = "Can not delete address. There are some  related to this address")]
    AddressCanNotDelete,
}