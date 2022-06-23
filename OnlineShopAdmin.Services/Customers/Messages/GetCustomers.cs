using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.DataAccess.Models;

namespace OnlineShopAdmin.Services.Customers.Messages;

public class GetCustomersRequest : Request
{
    public string Title { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string CompanyName { get; set; }

    public string EmailAddress { get; set; }

    public string Phone { get; set; }
}

public class GetCustomersResponse
{
    public Customer Customer { get; set; }

    public int NumberOfAddresses { get; set; }
}