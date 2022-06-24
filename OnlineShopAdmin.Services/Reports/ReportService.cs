using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Helpers;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.Customers.Models;
using OnlineShopAdmin.Services.Products.Models;
using OnlineShopAdmin.Services.Reports.Messages;

namespace OnlineShopAdmin.Services.Reports;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResponse<GetCustomersBySalesAmountResponse>> CustomersBySalesAmount()
    {
        var query = await _unitOfWork.Query<SalesOrderHeader>().ToListAsync();

        var salesOrderHeaders = query
            .GroupBy(x => x.CustomerId)
            .OrderByDescending(g => g.Count());

        var customersBySalesAmount = (
                from salesOrderHeader in salesOrderHeaders
                let customer = salesOrderHeader.Select(x => x).First(x => x.Customer != null).Customer
                let customerResponseModel = _mapper.Map<CustomerResponseModel>(customer)
                select new GetCustomerBySalesAmountResponse
                {
                    Customer = customerResponseModel, ToTalSales = salesOrderHeader.Count()
                })
            .Take(10)
            .ToList();

        return ResponseHelper.Ok(new GetCustomersBySalesAmountResponse { CustomerBySalesAmount = customersBySalesAmount });
    }

    public async Task<IResponse<GetCustomersBySalesAmountForEachYearResponse>> CustomersBySalesAmountForEachYear()
    {
        var query = await _unitOfWork.Query<SalesOrderHeader>().ToListAsync();

        var salesOrderHeaders = query
            .GroupBy(x => x.OrderDate.Year)
            .OrderByDescending(g => g.Count());

        var customersBySalesAmount = (
                from salesOrderHeader in salesOrderHeaders
                let customer = salesOrderHeader.Select(x => x).First(x => x.Customer != null).Customer
                let customerResponseModel = _mapper.Map<CustomerResponseModel>(customer)
                select new GetCustomerBySalesAmountForEachYearResponse
                {
                    Customer = customerResponseModel,
                    OrderYear = salesOrderHeader.Key
                })
            .Take(10)
            .ToList();

        return ResponseHelper.Ok(new GetCustomersBySalesAmountForEachYearResponse { CustomerBySalesAmountForEachYear = customersBySalesAmount });
    }

    public async Task<IResponse<GetProductsBySalesAmountResponse>> ProductsBySalesAmount()
    {
        var query = await _unitOfWork.Query<SalesOrderDetail>().ToListAsync();

        var salesOrderDetails = query
            .GroupBy(x => x.ProductId)
            .OrderByDescending(g => g.Count());

        var productsBySalesAmount = (
                from salesOrderHeader in salesOrderDetails
                let product = salesOrderHeader.Select(x => x).First(x => x.Product != null).Product
                let productResponseModel = _mapper.Map<ProductResponseModel>(product)
                select new GetProductBySalesAmountResponse
                {
                    Product = productResponseModel,
                    ToTalSales = salesOrderHeader.Count()
                })
            .Take(10)
            .ToList();

        return ResponseHelper.Ok(new GetProductsBySalesAmountResponse { ProductsBySalesAmount = productsBySalesAmount });
    }

    public async Task<IResponse<GetProductsBySalesProfitResponse>> ProductsBySalesProfit()
    {
        var query = await _unitOfWork.Query<SalesOrderDetail>().ToListAsync();

        var salesOrderDetails = query
            .GroupBy(x => x.ProductId)
            .OrderByDescending(g => g.Count());

        var productsBySalesProfit = (
                from salesOrderDetail in salesOrderDetails
                let product = salesOrderDetail.Select(x => x).First(x => x.Product != null).Product
                let productResponseModel = _mapper.Map<ProductResponseModel>(product)
                select new GetProductBySalesProfitResponse
                {
                    Product = productResponseModel,
                    ToTalProfit = salesOrderDetail.Sum(x => x.LineTotal)
                })
            .OrderByDescending(x => x.ToTalProfit)
            .Take(10)
            .ToList();

        return ResponseHelper.Ok(new GetProductsBySalesProfitResponse { ProductBySalesProfit = productsBySalesProfit });
    }

    public async Task<IResponse<GetProductsBySalesAmountForEachYearResponse>> ProductsBySalesAmountForEachYear()
    {
        var query = await _unitOfWork.Query<SalesOrderDetail>().ToListAsync();

        var salesOrderHeaders = query
            .GroupBy(x => x.SalesOrder.OrderDate.Year)
            .OrderByDescending(g => g.Count());

        var productsBySalesAmountForEachYear = (
                from salesOrderHeader in salesOrderHeaders
                let product = salesOrderHeader.Select(x => x).First(x => x.Product != null).Product
                let productResponseModel = _mapper.Map<ProductResponseModel>(product)
                select new GetProductBySalesAmountForEachYearResponse
                {
                    Product = productResponseModel,
                    OrderYear = salesOrderHeader.Key
                })
            .Take(10)
            .ToList();

        return ResponseHelper.Ok(new GetProductsBySalesAmountForEachYearResponse { ProductsBySalesAmountForEachYear = productsBySalesAmountForEachYear });
    }
}