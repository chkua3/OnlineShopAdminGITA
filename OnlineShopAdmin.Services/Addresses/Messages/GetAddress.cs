using FluentValidation;
using OnlineShopAdmin.Services.Addresses.Models;

namespace OnlineShopAdmin.Services.Addresses.Messages;

public class GetAddressRequest
{
    public int AddressId { get; set; }
}

public class GetAddressResponse
{
    public AddressResponseModel Address { get; set; }
}

public class GetAddressRequestValidator : AbstractValidator<GetAddressRequest>
{
    public GetAddressRequestValidator()
    {
        RuleFor(request => request.AddressId).NotEmpty();
    }
}