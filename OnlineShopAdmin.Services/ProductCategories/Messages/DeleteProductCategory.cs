using FluentValidation;

namespace OnlineShopAdmin.Services.ProductCategories.Messages;

public class DeleteProductCategoryRequest
{
    public int ProductCategoryId { get; set; }
}

public class DeleteProductCategoryRequestValidator : AbstractValidator<DeleteProductCategoryRequest>
{
    public DeleteProductCategoryRequestValidator()
    {
        RuleFor(request => request.ProductCategoryId).NotEmpty();
    }
}