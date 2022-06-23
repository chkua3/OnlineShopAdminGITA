namespace OnlineShopAdmin.Services.Addresses.Models;

public class AddressResponseModel
{
    public int AddressId { get; set; }
    
    public string AddressLine1 { get; set; }
    
    public string AddressLine2 { get; set; }
    
    public string City { get; set; }
    
    public string StateProvince { get; set; }
    
    public string CountryRegion { get; set; }
    
    public string PostalCode { get; set; }
}