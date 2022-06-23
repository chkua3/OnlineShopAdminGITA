using FluentValidation;

namespace OnlineShopAdmin.Services.Addresses.Messages;

public class DeleteAddressRequest
{
    public int AddressId { get; set; }
}

public class DeleteAddressRequestValidator : AbstractValidator<DeleteAddressRequest>
{
    public DeleteAddressRequestValidator()
    {
        RuleFor(request => request.AddressId).NotEmpty();
    }
}