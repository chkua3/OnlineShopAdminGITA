using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Enums;
using OnlineShopAdmin.Common.Extensions;
using OnlineShopAdmin.Common.Helpers;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.Customers.Messages;
using OnlineShopAdmin.Services.Customers.Models;

namespace OnlineShopAdmin.Services.Customers;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public List<SelectListItem> GetAddressSelectListItems()
    {
        var query = _unitOfWork.Query<Address>();

        var addressesModel = _mapper.Map<List<GetAddress>>(query);

        var addresses = new GetAddresses { Addresses = addressesModel };

        var addressSelectListItems = addresses.Addresses.Select(s => new SelectListItem
        {
            Value = s.AddressId.ToString(),
            Text = s.AddressLine1
        }).ToList();

        return addressSelectListItems;
    }

    public async Task<IResponse<GetCustomerResponse>> GetCustomerByIdAsync(GetCustomerRequest request)
    {
        var customer = await GetQuery().FirstOrDefaultAsync(c => c.CustomerId.Equals(request.CustomerId));

        if (customer == null) return ResponseHelper.Fail<GetCustomerResponse>(StatusCode.CustomerNotFound);

        var customerResponseModel = _mapper.Map<CustomerResponseModel>(customer);

        customerResponseModel.AddressId = customer.CustomerAddresses?.FirstOrDefault()?.AddressId;
        customerResponseModel.AddressType = customer.CustomerAddresses?.FirstOrDefault()?.AddressType;

        return ResponseHelper.Ok(new GetCustomerResponse { Customer = customerResponseModel });
    }

    public IResponse<PagedList<GetCustomersResponse>> GetCustomersAsync(GetCustomersRequest request)
    {
        var query = GetQuery();

        #region Filter

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(c => c.Title.Equals(request.Title));
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            query = query.Where(c => c.FirstName.Equals(request.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(request.MiddleName))
        {
            query = query.Where(c => c.MiddleName.Equals(request.MiddleName));
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            query = query.Where(c => c.LastName.Equals(request.LastName));
        }

        if (!string.IsNullOrWhiteSpace(request.CompanyName))
        {
            query = query.Where(c => c.CompanyName.Equals(request.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(request.EmailAddress))
        {
            query = query.Where(c => c.EmailAddress.Equals(request.EmailAddress));
        }

        if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            query = query.Where(c => c.Phone.Equals(request.Phone));
        }

        #endregion

        var customers = new List<GetCustomersResponse>();

        foreach (var customer in query)
        {
            customers.Add(new GetCustomersResponse
            {
                Customer = customer, NumberOfAddresses = customer.CustomerAddresses.Count
            });
        }

        var customersResponse = PagedList<GetCustomersResponse>.Create(customers, request.PageNumber, request.PageSize);

        return ResponseHelper.Ok(customersResponse);
    }

    public async Task<IResponse<EmptyResponse>> CreateAsync(CreateCustomerRequest request)
    {
        var customer = _mapper.Map<Customer>(request);
        
        customer.PasswordSalt = CommonExtensions.CreateSalt(1);
        customer.PasswordHash = CommonExtensions.GenerateHash(request.Password, customer.PasswordSalt);

        await _unitOfWork.AddAsync(customer);

        customer.CustomerAddresses.Add(new CustomerAddress
        {
            CustomerId = customer.CustomerId,
            AddressId = request.AddressId,
            AddressType = request.AddressType
        });

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

    public async Task<IResponse<UpdateCustomerRequest>> GetUpdateCustomerRequestModel(GetCustomerRequest request)
    {
        var response = await GetCustomerByIdAsync(request);

        if (response.Status.Code != StatusCode.Success)
            return ResponseHelper.Fail<UpdateCustomerRequest>(response.Status.Code, response.Status.Message);

        var updateCustomerRequestModel = _mapper.Map<UpdateCustomerRequest>(response.Data.Customer);

        return ResponseHelper.Ok(updateCustomerRequestModel);
    }

    public async Task<IResponse<UpdateCustomerResponse>> UpdateAsync(UpdateCustomerRequest request)
    {
        var customer = await GetQuery().FirstOrDefaultAsync(c => c.CustomerId.Equals(request.CustomerId));

        if (customer == null) return ResponseHelper.Fail<UpdateCustomerResponse>(StatusCode.CustomerNotFound);

        _mapper.Map(request, customer);

        if (request.ChangePassword)
        {
            customer.PasswordSalt = CommonExtensions.CreateSalt(1);
            customer.PasswordHash = CommonExtensions.GenerateHash(request.Password, customer.PasswordSalt);
        }

        _unitOfWork.RemoveRange(customer.CustomerAddresses);

        _unitOfWork.Update(customer);

        customer.CustomerAddresses.Add(new CustomerAddress
        {
            CustomerId = customer.CustomerId,
            AddressId = request.AddressId,
            AddressType = request.AddressType
        });

        try
        {
            await _unitOfWork.CommitAsync();
            return ResponseHelper.Ok(new UpdateCustomerResponse { CustomerId = customer.CustomerId });
        }
        catch
        {
            return ResponseHelper.Fail<UpdateCustomerResponse>();
        }
    }

    public async Task<IResponse<EmptyResponse>> DeleteAsync(DeleteCustomerRequest request)
    {
        var customer = await GetQuery().FirstOrDefaultAsync(c => c.CustomerId.Equals(request.CustomerId));

        if (customer == null) return ResponseHelper.Fail(StatusCode.CustomerNotFound);

        _unitOfWork.RemoveRange(customer.CustomerAddresses);

        _unitOfWork.Remove(customer);
        
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

    private IQueryable<Customer> GetQuery()
    {
        var query = _unitOfWork.Query<Customer>().OrderByDescending(x => x.ModifiedDate);

        return query;
    }
}
