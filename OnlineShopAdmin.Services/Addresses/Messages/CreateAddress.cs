using FluentValidation;

namespace OnlineShopAdmin.Services.Addresses.Messages;

public class CreateAddressRequest
{
    public string AddressLine1 { get; set; }
    
    public string AddressLine2 { get; set; }
    
    public string City { get; set; }
    
    public string StateProvince { get; set; }
    
    public string CountryRegion { get; set; }
    
    public string PostalCode { get; set; }
}

public class CreateAddressRequestValidator : AbstractValidator<CreateAddressRequest>
{
    public CreateAddressRequestValidator()
    {
        RuleFor(request => request.AddressLine1).NotEmpty().MaximumLength(60);
        RuleFor(request => request.AddressLine2).MaximumLength(60)
            .When(request => !string.IsNullOrWhiteSpace(request.AddressLine2));
        RuleFor(request => request.City).NotEmpty().MaximumLength(30);
        RuleFor(request => request.StateProvince).NotEmpty().MaximumLength(50);
        RuleFor(request => request.CountryRegion).NotEmpty().MaximumLength(50);
        RuleFor(request => request.PostalCode).NotEmpty().MaximumLength(15);
    }
}