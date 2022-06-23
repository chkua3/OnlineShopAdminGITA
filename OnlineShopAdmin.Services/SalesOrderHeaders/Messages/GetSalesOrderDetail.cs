using FluentValidation;
using OnlineShopAdmin.Services.SalesOrderHeaders.Models;

namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class GetSalesOrderDetailRequest
{
    public int SalesOrderDetailId { get; set; }
}

public class GetSalesOrderDetailResponse
{
    public SalesOrderDetailResponseModel SalesOrderDetail { get; set; }
}

public class GetSalesOrderDetailRequestValidator : AbstractValidator<GetSalesOrderDetailRequest>
{
    public GetSalesOrderDetailRequestValidator()
    {
        RuleFor(request => request.SalesOrderDetailId).NotEmpty();
    }
}