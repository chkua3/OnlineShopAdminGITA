namespace OnlineShopAdmin.Services.Products.Models;

public class ProductResponseModel
{
    public int ProductId { get; set; }

    public string Name { get; set; }
    
    public string ProductNumber { get; set; }
    
    public string Color { get; set; }
    
    public decimal StandardCost { get; set; }
    
    public decimal ListPrice { get; set; }
    
    public string Size { get; set; }
    
    public decimal? Weight { get; set; }
    
    public int? ProductCategoryId { get; set; }
    
    public int? ProductModelId { get; set; }
    
    public DateTime SellStartDate { get; set; }
    
    public DateTime? SellEndDate { get; set; }
    
    public DateTime? DiscontinuedDate { get; set; }
    
    public string FileName { get; set; }

    public string FileExtension { get; set; }
}