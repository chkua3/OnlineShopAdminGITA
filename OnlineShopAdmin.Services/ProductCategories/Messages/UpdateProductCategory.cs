using FluentValidation;

namespace OnlineShopAdmin.Services.ProductCategories.Messages;

public class UpdateProductCategoryRequest
{
    public int ProductCategoryId { get; set; }
 
    public int? ParentProductCategoryId { get; set; }
    
    public string Name { get; set; }
}

public class UpdateProductCategoryRequestValidator : AbstractValidator<UpdateProductCategoryRequest>
{
    public UpdateProductCategoryRequestValidator()
    {
        RuleFor(request => request.ProductCategoryId).NotEmpty();
        RuleFor(request => request.Name).NotEmpty().MaximumLength(50);
        RuleFor(request => request.ParentProductCategoryId).GreaterThan(0)
            .When(request => request.ParentProductCategoryId.HasValue);
    }
}

public class UpdateProductCategoryResponse
{
    public int ProductCategoryId { get; set; }
}