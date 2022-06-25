using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace OnlineShopAdmin.Services.Products.Messages;

public class CreateProductRequest
{
    public string Name { get; set; }

    public string ProductNumber { get; set; }

    public string Color { get; set; }

    public decimal StandardCost { get; set; }

    public decimal ListPrice { get; set; }

    public string Size { get; set; }

    public decimal? Weight { get; set; }

    public int? ProductCategoryId { get; set; }

    public int? ProductModelId { get; set; }

    public DateTime SellStartDate { get; set; }

    public DateTime? SellEndDate { get; set; }

    public DateTime? DiscontinuedDate { get; set; }

    public IFormFile File { get; set; }
}

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(request => request.Name).MaximumLength(50).NotNull().NotEmpty();
        RuleFor(request => request.ProductNumber).NotEmpty().MaximumLength(25);
        RuleFor(request => request.Color).MaximumLength(15)
            .When(request => !string.IsNullOrWhiteSpace(request.Color));
        RuleFor(request => request.StandardCost).GreaterThanOrEqualTo(0);
        RuleFor(request => request.ListPrice).GreaterThanOrEqualTo(0);
        RuleFor(request => request.Size).MaximumLength(5)
            .When(request => !string.IsNullOrWhiteSpace(request.Size));
        RuleFor(request => request.Weight).GreaterThanOrEqualTo(0)
            .When(request => request.Weight.HasValue);
        RuleFor(request => request.ProductCategoryId).GreaterThan(0)
            .When(request => request.ProductCategoryId.HasValue);
        RuleFor(request => request.ProductModelId).GreaterThan(0)
            .When(request => request.ProductModelId.HasValue);
        RuleFor(request => request.SellStartDate).Must(request => request != default);
        RuleFor(request => request.SellEndDate).Must(request => request != default)
            .When(request => request.SellEndDate.HasValue);
        RuleFor(request => request.DiscontinuedDate).Must(request => request != default)
            .When(request => request.DiscontinuedDate.HasValue);
        RuleFor(request => request.File)
            .Must(request => request.FileName.Length <= 50)
            .Must(file =>
                file.ContentType.Equals("image/jpeg") ||
                file.ContentType.Equals("image/jpg") ||
                file.ContentType.Equals("image/png"))
            .When(request => request.File != null);
    }
}