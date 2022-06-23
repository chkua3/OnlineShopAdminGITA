namespace OnlineShopAdmin.Services.Customers.Models;

public class CustomerResponseModel
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

    public int? AddressId { get; set; }

    public string AddressType { get; set; }
}