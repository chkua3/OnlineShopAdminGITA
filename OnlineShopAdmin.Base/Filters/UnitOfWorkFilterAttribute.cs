using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShopAdmin.Base.Interfaces;

namespace OnlineShopAdmin.Base.Filters;

public class UnitOfWorkFilterAttribute : ActionFilterAttribute
{
    private readonly IActionTransactionHelper _helper;

    public UnitOfWorkFilterAttribute(IActionTransactionHelper helper)
    {
        _helper = helper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _helper.BeginTransaction();
    }

    public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
    {
        _helper.EndTransaction(actionExecutedContext);
        _helper.CloseSession();
    }
}