using AutoMapper;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.Addresses.Messages;
using OnlineShopAdmin.Services.Addresses.Models;
using OnlineShopAdmin.Services.Customers.Messages;
using OnlineShopAdmin.Services.Customers.Models;
using OnlineShopAdmin.Services.ProductCategories.Messages;
using OnlineShopAdmin.Services.ProductCategories.Models;
using OnlineShopAdmin.Services.Products.Messages;
using OnlineShopAdmin.Services.Products.Models;
using OnlineShopAdmin.Services.SalesOrderHeaders.Messages;
using OnlineShopAdmin.Services.SalesOrderHeaders.Models;

namespace OnlineShopAdmin.Services;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Product, ProductResponseModel>();
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();
        CreateMap<ProductResponseModel, UpdateProductRequest>();
        CreateMap<ProductCategory, GetProductCategory>();
        CreateMap<ProductModel, GetProductModel>();

        CreateMap<ProductCategory, ProductCategoryResponseModel>();
        CreateMap<CreateProductCategoryRequest, ProductCategory>();
        CreateMap<UpdateProductCategoryRequest, ProductCategory>();
        CreateMap<ProductCategoryResponseModel, UpdateProductCategoryRequest>()
            .ForMember(desc => desc.ParentProductCategoryId,
                opt => opt.MapFrom(
                    src => src.ParentProductCategory.ProductCategoryId));
        CreateMap<ProductCategory, GetParentProductCategory>();

        CreateMap<Address, AddressResponseModel>();
        CreateMap<CreateAddressRequest, Address>();
        CreateMap<UpdateAddressRequest, Address>();
        CreateMap<AddressResponseModel, UpdateAddressRequest>();
        CreateMap<Address, GetAddress>();

        CreateMap<Customer, CustomerResponseModel>();
        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<CustomerResponseModel, UpdateCustomerRequest>();

        CreateMap<SalesOrderHeader, SalesOrderHeaderResponseModel>();
        CreateMap<CreateSalesOrderHeaderRequest, SalesOrderHeader>();
        CreateMap<CreateSalesOrderHeaderRequest, SalesOrderDetail>();
        CreateMap<Customer, GetCustomer>()
            .ForMember(desc => desc.CustomerName,
                opt => opt.MapFrom(
                    src => $"{src.FirstName} {src.LastName}"));

        CreateMap<SalesOrderDetail, SalesOrderDetailResponseModel>();
        CreateMap<Product, GetProduct>();
    }
}