namespace OnlineShopAdmin.Services.SalesOrderHeaders.Models;

public class SalesOrderDetailResponseModel
{
    public int SalesOrderId { get; set; }

    public int SalesOrderDetailId { get; set; }
    
    public short OrderQty { get; set; }
    
    public int ProductId { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal UnitPriceDiscount { get; set; }
    
    public decimal LineTotal { get; set; }
}