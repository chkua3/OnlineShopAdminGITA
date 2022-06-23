using FluentValidation;
using OnlineShopAdmin.Services.Products.Models;

namespace OnlineShopAdmin.Services.Products.Messages;

public class GetProductRequest
{
    public int ProductId { get; set; }
}

public class GetProductResponse
{
    public ProductResponseModel Product { get; set; }
}

public class GetProductRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductRequestValidator()
    {
        RuleFor(request => request.ProductId).NotEmpty();
    }
}