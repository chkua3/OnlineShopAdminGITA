using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Services.Customers.Messages;

namespace OnlineShopAdmin.Services.Customers;

public interface ICustomerService
{
    List<SelectListItem> GetAddressSelectListItems();

    Task<IResponse<GetCustomerResponse>> GetCustomerByIdAsync(GetCustomerRequest request);

    IResponse<PagedList<GetCustomersResponse>> GetCustomersAsync(GetCustomersRequest request);

    Task<IResponse<EmptyResponse>> CreateAsync(CreateCustomerRequest request);

    Task<IResponse<UpdateCustomerRequest>> GetUpdateCustomerRequestModel(GetCustomerRequest request);

    Task<IResponse<UpdateCustomerResponse>> UpdateAsync(UpdateCustomerRequest request);

    Task<IResponse<EmptyResponse>> DeleteAsync(DeleteCustomerRequest request);
}