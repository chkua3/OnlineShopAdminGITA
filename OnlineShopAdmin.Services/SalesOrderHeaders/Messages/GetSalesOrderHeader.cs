using FluentValidation;
using OnlineShopAdmin.Services.SalesOrderHeaders.Models;

namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class GetSalesOrderHeaderRequest
{
    public int SalesOrderId { get; set; }
}

public class GetSalesOrderHeaderResponse
{
    public SalesOrderHeaderResponseModel SalesOrderHeader { get; set; }
}

public class GetSalesOrderHeaderRequestValidator : AbstractValidator<GetSalesOrderHeaderRequest>
{
    public GetSalesOrderHeaderRequestValidator()
    {
        RuleFor(request => request.SalesOrderId).NotEmpty();
    }
}