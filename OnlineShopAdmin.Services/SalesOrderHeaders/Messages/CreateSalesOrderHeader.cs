using FluentValidation;

namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class CreateSalesOrderHeaderRequest
{
    public DateTime OrderDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int Status { get; set; }

    public bool OnlineOrderFlag { get; set; }

    public string SalesOrderNumber { get; set; }

    public string PurchaseOrderNumber { get; set; }

    public string AccountNumber { get; set; }

    public int CustomerId { get; set; }

    public int? ShipToAddressId { get; set; }

    public int? BillToAddressId { get; set; }

    public string ShipMethod { get; set; }

    public string CreditCardApprovalCode { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmt { get; set; }

    public decimal Freight { get; set; }

    public decimal TotalDue { get; set; }

    public string Comment { get; set; }


    public short OrderQty { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal UnitPriceDiscount { get; set; }

    public decimal LineTotal { get; set; }
}

public class CreateSalesOrderHeaderRequestValidator : AbstractValidator<CreateSalesOrderHeaderRequest>
{
    public CreateSalesOrderHeaderRequestValidator()
    {
        RuleFor(request => request.OrderDate).Must(request => request != default);
        RuleFor(request => request.DueDate).Must(request => request != default);
        RuleFor(request => request.ShipDate).Must(request => request != default)
            .When(request => request.ShipDate.HasValue).NotEmpty();
        RuleFor(request => request.Status).GreaterThan(0);
        RuleFor(request => request.OnlineOrderFlag).NotEmpty();
        RuleFor(request => request.SalesOrderNumber).NotEmpty().MaximumLength(25);
        RuleFor(request => request.PurchaseOrderNumber).MaximumLength(25);
        RuleFor(request => request.AccountNumber).MaximumLength(15);
        RuleFor(request => request.CustomerId).NotEmpty().GreaterThan(0);
        RuleFor(request => request.ShipToAddressId).GreaterThan(0).When(request => request.ShipToAddressId.HasValue);
        RuleFor(request => request.BillToAddressId).GreaterThan(0).When(request => request.BillToAddressId.HasValue);
        RuleFor(request => request.ShipMethod).NotEmpty().MaximumLength(50);
        RuleFor(request => request.CreditCardApprovalCode).MaximumLength(15);
        RuleFor(request => request.SubTotal).GreaterThanOrEqualTo(0);
        RuleFor(request => request.TaxAmt).GreaterThanOrEqualTo(0);
        RuleFor(request => request.Freight).GreaterThanOrEqualTo(0);

        RuleFor(request => request.ProductId).GreaterThan(0);
    }
}