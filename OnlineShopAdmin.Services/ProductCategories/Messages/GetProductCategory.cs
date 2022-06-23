using FluentValidation;
using OnlineShopAdmin.Services.ProductCategories.Models;

namespace OnlineShopAdmin.Services.ProductCategories.Messages;

public class GetProductCategoryRequest
{
    public int ProductCategoryId { get; set; }
}

public class GetProductCategoryResponse
{
    public ProductCategoryResponseModel ProductCategory { get; set; }
}

public class GetProductCategoryRequestValidator : AbstractValidator<GetProductCategoryRequest>
{
    public GetProductCategoryRequestValidator()
    {
        RuleFor(request => request.ProductCategoryId).NotEmpty();
    }
}