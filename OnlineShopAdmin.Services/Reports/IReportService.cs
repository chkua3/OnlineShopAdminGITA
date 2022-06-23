using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Services.Reports.Messages;

namespace OnlineShopAdmin.Services.Reports;

public interface IReportService
{
    Task<IResponse<GetCustomersBySalesAmountResponse>> CustomersBySalesAmount();
    Task<IResponse<GetCustomersBySalesAmountForEachYearResponse>> CustomersBySalesAmountForEachYear();
    Task<IResponse<GetProductsBySalesAmountResponse>> ProductsBySalesAmount();
    Task<IResponse<GetProductsBySalesProfitResponse>> ProductsBySalesProfit();
    Task<IResponse<GetProductsBySalesAmountForEachYearResponse>> ProductsBySalesAmountForEachYear();
}