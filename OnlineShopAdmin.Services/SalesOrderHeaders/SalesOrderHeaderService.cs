using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Enums;
using OnlineShopAdmin.Common.Helpers;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.SalesOrderHeaders.Messages;
using OnlineShopAdmin.Services.SalesOrderHeaders.Models;

namespace OnlineShopAdmin.Services.SalesOrderHeaders;

public class SalesOrderHeaderService : ISalesOrderHeaderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SalesOrderHeaderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public List<SelectListItem> GetCustomerSelectListItems()
    {
        var query = _unitOfWork.Query<Customer>();

        var customersModel = _mapper.Map<List<GetCustomer>>(query);

        var customers = new GetCustomers { Customers = customersModel };

        var customerSelectListItems = customers.Customers.Select(s => new SelectListItem
        {
            Value = s.CustomerId.ToString(),
            Text = s.CustomerName
        }).ToList();

        return customerSelectListItems;
    }

    public List<SelectListItem> GetProductSelectListItems()
    {
        var query = _unitOfWork.Query<Product>();

        var productsModel = _mapper.Map<List<GetProduct>>(query);

        var products = new GetProducts { Products = productsModel };

        var productSelectListItems = products.Products.Select(s => new SelectListItem
        {
            Value = s.ProductId.ToString(),
            Text = s.Name
        }).ToList();

        return productSelectListItems;
    }

    public async Task<IResponse<GetSalesOrderHeaderResponse>> GetSalesOrderHeaderByIdAsync(GetSalesOrderHeaderRequest request)
    {
        var salesOrderHeader = await GetQuery().FirstOrDefaultAsync(p => p.SalesOrderId.Equals(request.SalesOrderId));

        if (salesOrderHeader == null) return ResponseHelper.Fail<GetSalesOrderHeaderResponse>(StatusCode.SalesOrderHeaderNotFound);

        var salesOrderHeaderResponseModel = _mapper.Map<SalesOrderHeaderResponseModel>(salesOrderHeader);

        return ResponseHelper.Ok(new GetSalesOrderHeaderResponse { SalesOrderHeader = salesOrderHeaderResponseModel });
    }

    public async Task<IResponse<PagedList<GetSalesOrderHeadersResponse>>> GetSalesOrderHeadersAsync(GetSalesOrderHeadersRequest request)
    {
        var query = GetQuery();

        #region Filter

        if (request.OrderDate.HasValue)
        {
            query = query.Where(s => s.OrderDate.Date.Equals(request.OrderDate.Value.Date));
        }

        if (request.DueDate.HasValue)
        {
            query = query.Where(s => s.DueDate.Date.Equals(request.DueDate.Value.Date));
        }

        if (request.ShipDate.HasValue)
        {
            query = query.Where(s => s.ShipDate.Value.Date.Equals(request.ShipDate.Value.Date));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(s => s.Status.Equals(request.Status));
        }

        if (request.OnlineOrderFlag.HasValue)
        {
            query = query.Where(s => s.OnlineOrderFlag.Equals(request.OnlineOrderFlag));
        }

        if (!string.IsNullOrWhiteSpace(request.SalesOrderNumber))
        {
            query = query.Where(s => s.SalesOrderNumber.Equals(request.SalesOrderNumber));
        }
        
        if (!string.IsNullOrWhiteSpace(request.PurchaseOrderNumber))
        {
            query = query.Where(s => s.PurchaseOrderNumber.Equals(request.PurchaseOrderNumber));
        }
        
        if (!string.IsNullOrWhiteSpace(request.AccountNumber))
        {
            query = query.Where(s => s.AccountNumber.Equals(request.AccountNumber));
        }
        
        if (!string.IsNullOrWhiteSpace(request.ShipMethod))
        {
            query = query.Where(s => s.ShipMethod.Equals(request.ShipMethod));
        }
        
        if (!string.IsNullOrWhiteSpace(request.CreditCardApprovalCode))
        {
            query = query.Where(s => s.CreditCardApprovalCode.Equals(request.CreditCardApprovalCode));
        }

        if (request.SubTotal.HasValue)
        {
            query = query.Where(s => s.SubTotal.Equals(request.SubTotal));
        }

        if (request.TaxAmt.HasValue)
        {
            query = query.Where(s => s.TaxAmt.Equals(request.TaxAmt));
        }

        if (request.Freight.HasValue)
        {
            query = query.Where(s => s.Freight.Equals(request.Freight));
        }

        if (request.TotalDue.HasValue)
        {
            query = query.Where(s => s.TotalDue.Equals(request.TotalDue));
        }

        if (!string.IsNullOrWhiteSpace(request.Comment))
        {
            query = query.Where(s => s.Comment.Equals(request.Comment));
        }

        #endregion

        var salesOrderHeaders = new List<GetSalesOrderHeadersResponse>();

        foreach (var salesOrderHeader in query)
        {
            var salesOrderHeaderObject = new GetSalesOrderHeadersResponse { SalesOrderHeader = salesOrderHeader };

            var customer = await _unitOfWork.Query<Customer>()
                .FirstOrDefaultAsync(c => c.CustomerId.Equals(salesOrderHeader.CustomerId));

            if (customer != null) salesOrderHeaderObject.CustomerFullName = $"{customer.FirstName} {customer.LastName}";

            salesOrderHeaders.Add(salesOrderHeaderObject);
        }

        var salesOrderHeadersResponse =
            PagedList<GetSalesOrderHeadersResponse>.Create(salesOrderHeaders, request.PageNumber, request.PageSize);

        return ResponseHelper.Ok(salesOrderHeadersResponse);
    }

    public async Task<IResponse<EmptyResponse>> CreateAsync(CreateSalesOrderHeaderRequest request)
    {
        if (await Exists(request.SalesOrderNumber)) return ResponseHelper.Fail(StatusCode.SalesOrderHeaderNumberWithSameNameAlreadyExists);

        var salesOrderHeader = _mapper.Map<SalesOrderHeader>(request);

        var salesOrderDetail = _mapper.Map<SalesOrderDetail>(request);

        salesOrderHeader.SalesOrderDetails.Add(salesOrderDetail);

        await _unitOfWork.AddAsync(salesOrderHeader);

        try
        {
            await _unitOfWork.CommitAsync();
            return ResponseHelper.Ok();
        }
        catch
        {
            return ResponseHelper.Fail();
        }
    }

    public async Task<IResponse<EmptyResponse>> DeleteAsync(DeleteSalesOrderHeaderRequest request)
    {
        var salesOrderHeader = await GetQuery().FirstOrDefaultAsync(p => p.SalesOrderId.Equals(request.SalesOrderId));

        if (salesOrderHeader == null) return ResponseHelper.Fail(StatusCode.ProductNotFound);

        if (salesOrderHeader.SalesOrderDetails.Any())
            _unitOfWork.RemoveRange(salesOrderHeader.SalesOrderDetails);

        _unitOfWork.Remove(salesOrderHeader);

        try
        {
            await _unitOfWork.CommitAsync();
            return ResponseHelper.Ok();
        }
        catch
        {
            return ResponseHelper.Fail();
        }
    }
    
    public async Task<IResponse<GetSalesOrderDetailResponse>> GetSalesOrderDetailByIdAsync(GetSalesOrderDetailRequest request)
    {
        var salesOrderDetail = await _unitOfWork.Query<SalesOrderDetail>().FirstOrDefaultAsync(p => p.SalesOrderDetailId.Equals(request.SalesOrderDetailId));

        if (salesOrderDetail == null) return ResponseHelper.Fail<GetSalesOrderDetailResponse>(StatusCode.SalesOrderDetailsNotFound);

        var salesOrderDetailResponseModel = _mapper.Map<SalesOrderDetailResponseModel>(salesOrderDetail);

        return ResponseHelper.Ok(new GetSalesOrderDetailResponse { SalesOrderDetail = salesOrderDetailResponseModel });
    }

    public IResponse<PagedList<GetSalesOrderDetailsResponse>> GetSalesOrderDetailsAsync(GetSalesOrderDetailsRequest request)
    {
        var query = _unitOfWork.Query<SalesOrderDetail>().Where(s => s.SalesOrderId.Equals(request.SaleOrderId));

        #region Filter

        if (request.OrderQty.HasValue)
        {
            query = query.Where(s => s.OrderQty.Equals(request.OrderQty.Value));
        }

        if (request.ProductId.HasValue)
        {
            query = query.Where(s => s.ProductId.Equals(request.ProductId));
        }

        if (request.UnitPrice.HasValue)
        {
            query = query.Where(s => s.UnitPrice.Equals(request.UnitPrice));
        }

        if (request.UnitPriceDiscount.HasValue)
        {
            query = query.Where(s => s.UnitPriceDiscount.Equals(request.UnitPriceDiscount));
        }

        if (request.LineTotal.HasValue)
        {
            query = query.Where(s => s.LineTotal.Equals(request.LineTotal));
        }
        
        #endregion

        var salesOrderDetails = new List<GetSalesOrderDetailsResponse>();

        foreach (var salesOrderDetail in query)
        {
            var salesOrderDetailObject = new GetSalesOrderDetailsResponse { SalesOrderDetail = salesOrderDetail };
            
            salesOrderDetails.Add(salesOrderDetailObject);
        }

        var salesOrderHeadersResponse =
            PagedList<GetSalesOrderDetailsResponse>.Create(salesOrderDetails, request.PageNumber, request.PageSize);

        return ResponseHelper.Ok(salesOrderHeadersResponse);
    }

    private IQueryable<SalesOrderHeader> GetQuery()
    {
        var query = _unitOfWork.Query<SalesOrderHeader>().OrderByDescending(x => x.ModifiedDate);

        return query;
    }

    private async Task<bool> Exists(string salesOrderNumber)
    {
        var salesOrderHeaders = _unitOfWork.Query<SalesOrderHeader>();

        return await salesOrderHeaders.AnyAsync(soh => soh.SalesOrderNumber.Equals(salesOrderNumber));
    }
}
