using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.DataAccess.Models;

namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class GetSalesOrderHeadersRequest : Request
{
    public DateTime? OrderDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int? Status { get; set; }

    public bool? OnlineOrderFlag { get; set; }

    public string SalesOrderNumber { get; set; }

    public string PurchaseOrderNumber { get; set; }

    public string AccountNumber { get; set; }

    public string ShipMethod { get; set; }

    public string CreditCardApprovalCode { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? TaxAmt { get; set; }

    public decimal? Freight { get; set; }

    public decimal? TotalDue { get; set; }

    public string Comment { get; set; }
}

public class GetSalesOrderHeadersResponse
{
    public SalesOrderHeader SalesOrderHeader { get; set; }

    public string CustomerFullName { get; set; }
}