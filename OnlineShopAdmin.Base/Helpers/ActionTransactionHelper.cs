using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShopAdmin.Base.Interfaces;

namespace OnlineShopAdmin.Base.Helpers;

public class ActionTransactionHelper : IActionTransactionHelper
{
    private IUnitOfWork _unitOfWork;
    private ITransaction _transaction;

    public ActionTransactionHelper(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void BeginTransaction()
    {
        _transaction = _unitOfWork.BeginTransaction();
    }

    public void EndTransaction(ActionExecutedContext filterContext)
    {
        if (_transaction == null)
        {
            throw new NotSupportedException();
        }

        if (filterContext.Exception == null)
        {
            _unitOfWork.Commit();
            _transaction.Commit();
        }
        else
        {
            try
            {
                _transaction.Rollback();
            }
            catch (Exception ex)
            {
                throw new AggregateException(filterContext.Exception, ex);
            }
        }
    }

    public void CloseSession()
    {
        if (_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }

        if (_unitOfWork != null)
        {
            _unitOfWork.Dispose();
            _unitOfWork = null;
        }
    }
}