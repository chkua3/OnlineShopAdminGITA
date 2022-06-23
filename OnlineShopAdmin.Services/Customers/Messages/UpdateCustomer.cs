using FluentValidation;

namespace OnlineShopAdmin.Services.Customers.Messages;

public class UpdateCustomerRequest
{
    public int CustomerId { get; set; }

    public bool NameStyle { get; set; }

    public string Title { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Suffix { get; set; }

    public string CompanyName { get; set; }

    public string SalesPerson { get; set; }

    public string EmailAddress { get; set; }

    public string Phone { get; set; }

    public int AddressId { get; set; }

    public string AddressType { get; set; }

    public bool ChangePassword { get; set; } = false;

    public string Password { get; set; }
}

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(request => request.CustomerId).NotEmpty();
        RuleFor(request => request.Title).MaximumLength(8).When(request => !string.IsNullOrWhiteSpace(request.Title));
        RuleFor(request => request.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(request => request.MiddleName).MaximumLength(50)
            .When(request => !string.IsNullOrWhiteSpace(request.MiddleName));
        RuleFor(request => request.LastName).NotEmpty().MaximumLength(50);
        RuleFor(request => request.Suffix).MaximumLength(10)
            .When(request => !string.IsNullOrWhiteSpace(request.Suffix));
        RuleFor(request => request.CompanyName).MaximumLength(128).
            When(request => !string.IsNullOrWhiteSpace(request.CompanyName));
        RuleFor(request => request.SalesPerson).MaximumLength(256)
            .When(request => !string.IsNullOrWhiteSpace(request.SalesPerson));
        RuleFor(request => request.EmailAddress).MaximumLength(50)
            .When(request => !string.IsNullOrWhiteSpace(request.EmailAddress));
        RuleFor(request => request.Phone).MaximumLength(25)
            .When(request => !string.IsNullOrWhiteSpace(request.Phone));
        RuleFor(request => request.AddressId).GreaterThan(0);
        RuleFor(request => request.AddressType).NotEmpty().MaximumLength(50);
        RuleFor(request => request.Password).NotEmpty().When(request => request.ChangePassword);
    }
}

public class UpdateCustomerResponse
{
    public int CustomerId { get; set; }
}