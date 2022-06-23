using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShopAdmin.Base.Interfaces;

public interface IActionTransactionHelper
{
    void BeginTransaction();
    void EndTransaction(ActionExecutedContext filterContext);
    void CloseSession();
}