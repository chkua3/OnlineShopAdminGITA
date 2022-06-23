using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Services.Addresses.Messages;

namespace OnlineShopAdmin.Services.Addresses;

public interface IAddressService
{
    Task<IResponse<GetAddressResponse>> GetAddressByIdAsync(GetAddressRequest request);

    IResponse<PagedList<GetAddressesResponse>> GetAddressesAsync(GetAddressesRequest request);

    Task<IResponse<EmptyResponse>> CreateAsync(CreateAddressRequest request);

    Task<IResponse<UpdateAddressRequest>> GetUpdateAddressRequestModel(GetAddressRequest request);

    Task<IResponse<UpdateAddressResponse>> UpdateAsync(UpdateAddressRequest request);

    Task<IResponse<EmptyResponse>> DeleteAsync(DeleteAddressRequest request);
}