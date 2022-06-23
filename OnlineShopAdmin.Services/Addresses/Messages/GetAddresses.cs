using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.DataAccess.Models;

namespace OnlineShopAdmin.Services.Addresses.Messages;

public class GetAddressesRequest : Request
{
    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public string City { get; set; }

    public string StateProvince { get; set; }

    public string CountryRegion { get; set; }

    public string PostalCode { get; set; }
}

public class GetAddressesResponse
{
    public Address Address { get; set; }
}