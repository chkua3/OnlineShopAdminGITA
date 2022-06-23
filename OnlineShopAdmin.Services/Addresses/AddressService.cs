using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Enums;
using OnlineShopAdmin.Common.Helpers;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.Addresses.Messages;
using OnlineShopAdmin.Services.Addresses.Models;

namespace OnlineShopAdmin.Services.Addresses;

public class AddressService : IAddressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResponse<GetAddressResponse>> GetAddressByIdAsync(GetAddressRequest request)
    {
        var address = await GetQuery().FirstOrDefaultAsync(a => a.AddressId.Equals(request.AddressId));

        if (address == null) return ResponseHelper.Fail<GetAddressResponse>(StatusCode.AddressNotFound);

        var addressResponseModel = _mapper.Map<AddressResponseModel>(address);

        return ResponseHelper.Ok(new GetAddressResponse { Address = addressResponseModel });
    }

    public IResponse<PagedList<GetAddressesResponse>> GetAddressesAsync(GetAddressesRequest request)
    {
        var query = GetQuery();

        #region Filter

        if (!string.IsNullOrWhiteSpace(request.AddressLine1))
        {
            query = query.Where(a => a.AddressLine1.Equals(request.AddressLine1));
        }

        if (!string.IsNullOrWhiteSpace(request.AddressLine2))
        {
            query = query.Where(a => a.AddressLine1.Equals(request.AddressLine2));
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            query = query.Where(a => a.AddressLine1.Equals(request.City));
        }

        if (!string.IsNullOrWhiteSpace(request.StateProvince))
        {
            query = query.Where(a => a.AddressLine1.Equals(request.StateProvince));
        }

        if (!string.IsNullOrWhiteSpace(request.CountryRegion))
        {
            query = query.Where(a => a.AddressLine1.Equals(request.CountryRegion));
        }

        if (!string.IsNullOrWhiteSpace(request.PostalCode))
        {
            query = query.Where(a => a.AddressLine1.Equals(request.PostalCode));
        }

        #endregion

        var addresses = new List<GetAddressesResponse>();

        foreach (var address in query)
        {
            addresses.Add(new GetAddressesResponse { Address = address });
        }

        var addressesResponse = PagedList<GetAddressesResponse>.Create(addresses, request.PageNumber, request.PageSize);

        return ResponseHelper.Ok(addressesResponse);
    }

    public async Task<IResponse<UpdateAddressRequest>> GetUpdateAddressRequestModel(GetAddressRequest request)
    {
        var response = await GetAddressByIdAsync(request);

        if (response.Status.Code != StatusCode.Success)
            return ResponseHelper.Fail<UpdateAddressRequest>(response.Status.Code, response.Status.Message);

        var updateAddressRequestModel = _mapper.Map<UpdateAddressRequest>(response.Data.Address);

        return ResponseHelper.Ok(updateAddressRequestModel);
    }

    public async Task<IResponse<EmptyResponse>> CreateAsync(CreateAddressRequest request)
    {
        var address = _mapper.Map<Address>(request);

        await _unitOfWork.AddAsync(address);

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

    public async Task<IResponse<UpdateAddressResponse>> UpdateAsync(UpdateAddressRequest request)
    {
        var address = await GetQuery().FirstOrDefaultAsync(a => a.AddressId.Equals(request.AddressId));

        if (address == null) return ResponseHelper.Fail<UpdateAddressResponse>(StatusCode.AddressNotFound);

        _mapper.Map(request, address);

        _unitOfWork.Update(address);

        try
        {
            await _unitOfWork.CommitAsync();
            return ResponseHelper.Ok(new UpdateAddressResponse { AddressId = address.AddressId });
        }
        catch
        {
            return ResponseHelper.Fail<UpdateAddressResponse>();
        }
    }

    public async Task<IResponse<EmptyResponse>> DeleteAsync(DeleteAddressRequest request)
    {
        var address = await GetQuery().FirstOrDefaultAsync(a => a.AddressId.Equals(request.AddressId));

        if (address == null) return ResponseHelper.Fail();

        _unitOfWork.RemoveRange(address.CustomerAddresses);

        _unitOfWork.Remove(address);

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

    private IQueryable<Address> GetQuery()
    {
        var query = _unitOfWork.Query<Address>().OrderByDescending(x => x.ModifiedDate);

        return query;
    }
}
