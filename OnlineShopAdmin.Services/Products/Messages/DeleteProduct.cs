using FluentValidation;

namespace OnlineShopAdmin.Services.Products.Messages;

public class DeleteProductRequest
{
    public int ProductId { get; set; }
}

public class DeleteProductRequestValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductRequestValidator()
    {
        RuleFor(request => request.ProductId).NotEmpty();
    }
}