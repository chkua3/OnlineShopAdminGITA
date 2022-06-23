using FluentValidation;

namespace OnlineShopAdmin.Services.Customers.Messages;

public class DeleteCustomerRequest
{
    public int CustomerId { get; set; }
}

public class DeleteCustomerRequestValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerRequestValidator()
    {
        RuleFor(request => request.CustomerId).NotEmpty();
    }
}