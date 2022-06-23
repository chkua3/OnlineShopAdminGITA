using FluentValidation;

namespace OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

public class DeleteSalesOrderHeaderRequest
{
    public int SalesOrderId { get; set; }
}

public class DeleteSalesOrderHeaderRequestValidator : AbstractValidator<DeleteSalesOrderHeaderRequest>
{
    public DeleteSalesOrderHeaderRequestValidator()
    {
        RuleFor(request => request.SalesOrderId).NotEmpty();
    }
}