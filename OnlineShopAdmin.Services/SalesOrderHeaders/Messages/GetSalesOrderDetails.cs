using FluentValidation;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.DataAccess.Models;

namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class GetSalesOrderDetailsRequest : Request
{
    public int SaleOrderId { get; set; }

    public short? OrderQty { get; set; }
    
    public int? ProductId { get; set; }
    
    public decimal? UnitPrice { get; set; }
    
    public decimal? UnitPriceDiscount { get; set; }
    
    public decimal? LineTotal { get; set; }
}

public class GetSalesOrderDetailsRequestValidator : AbstractValidator<GetSalesOrderDetailsRequest>
{
    public GetSalesOrderDetailsRequestValidator()
    {
        RuleFor(request => request.SaleOrderId).NotEmpty().GreaterThan(0);
    }
}

public class GetSalesOrderDetailsResponse
{
    public SalesOrderDetail SalesOrderDetail { get; set; }
}