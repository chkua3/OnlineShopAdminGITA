using FluentValidation;
using OnlineShopAdmin.Services.Customers.Models;

namespace OnlineShopAdmin.Services.Customers.Messages;

public class GetCustomerRequest
{
    public int CustomerId { get; set; }
}

public class GetCustomerResponse
{
    public CustomerResponseModel Customer { get; set; }
}

public class GetCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
{
    public GetCustomerRequestValidator()
    {
        RuleFor(request => request.CustomerId).NotEmpty();
    }
}