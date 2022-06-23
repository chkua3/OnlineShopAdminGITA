using Microsoft.AspNetCore.Mvc;
using OnlineShopAdmin.Services.Reports;

namespace OnlineShopAdmin.Controllers;

public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult ChooseReport()
    {
        return View();
    }

    /// <summary>
    /// get Top 10 Customers by Sales amount.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> CustomersBySalesAmount()
    {
        var response = await _reportService.CustomersBySalesAmount();

        return View(response.Data);
    }

    /// <summary>
    /// get Top 10 Customers by Sales amount for each year.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> CustomersBySalesAmountForEachYear()
    {
        var response = await _reportService.CustomersBySalesAmountForEachYear();

        return View(response.Data);
    }

    /// <summary>
    /// get Top 10 Products by Sales amount.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ProductsBySalesAmount()
    {
        var response = await _reportService.ProductsBySalesAmount();

        return View(response.Data);
    }

    /// <summary>
    /// get Top 10 Products by Sales profit.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ProductsBySalesProfit()
    {
        var response = await _reportService.ProductsBySalesProfit();

        return View(response.Data);
    }

    /// <summary>
    /// get Top 10 Products by Sales amount for each year
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ProductsBySalesAmountForEachYear()
    {
        var response = await _reportService.ProductsBySalesAmountForEachYear();

        return View(response.Data);
    }
}