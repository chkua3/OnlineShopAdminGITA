namespace OnlineShopAdmin.Services.Customers.Messages;

public class GetAddresses
{
    public List<GetAddress> Addresses { get; set; }
}

public class GetAddress
{
    public int AddressId { get; set; }

    public string AddressLine1 { get; set; }
}