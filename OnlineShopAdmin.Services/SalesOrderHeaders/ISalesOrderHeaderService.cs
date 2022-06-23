using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

namespace OnlineShopAdmin.Services.SalesOrderHeaders;

public interface ISalesOrderHeaderService
{
    List<SelectListItem> GetCustomerSelectListItems();

    List<SelectListItem> GetProductSelectListItems();

    Task<IResponse<GetSalesOrderHeaderResponse>> GetSalesOrderHeaderByIdAsync(GetSalesOrderHeaderRequest request);

    Task<IResponse<PagedList<GetSalesOrderHeadersResponse>>> GetSalesOrderHeadersAsync(GetSalesOrderHeadersRequest request);

    Task<IResponse<EmptyResponse>> CreateAsync(CreateSalesOrderHeaderRequest request);

    Task<IResponse<EmptyResponse>> DeleteAsync(DeleteSalesOrderHeaderRequest request);

    Task<IResponse<GetSalesOrderDetailResponse>> GetSalesOrderDetailByIdAsync(GetSalesOrderDetailRequest request);

    IResponse<PagedList<GetSalesOrderDetailsResponse>> GetSalesOrderDetailsAsync(GetSalesOrderDetailsRequest request);
}