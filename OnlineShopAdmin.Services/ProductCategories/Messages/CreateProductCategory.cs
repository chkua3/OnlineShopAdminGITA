using FluentValidation;

namespace OnlineShopAdmin.Services.ProductCategories.Messages;

public class CreateProductCategoryRequest
{
    public int? ParentProductCategoryId { get; set; }

    public string Name { get; set; }
}

public class CreateProductCategoryRequestValidator : AbstractValidator<CreateProductCategoryRequest>
{
    public CreateProductCategoryRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().MaximumLength(50);
        RuleFor(request => request.ParentProductCategoryId).GreaterThan(0)
            .When(request => request.ParentProductCategoryId.HasValue);
    }
}