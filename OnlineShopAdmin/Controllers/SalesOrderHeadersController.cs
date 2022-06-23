using Microsoft.AspNetCore.Mvc;
using OnlineShopAdmin.Base.Filters;
using OnlineShopAdmin.Services.SalesOrderHeaders;
using OnlineShopAdmin.Services.SalesOrderHeaders.Messages;

namespace OnlineShopAdmin.Controllers;

public class SalesOrderHeadersController : Controller
{
    private readonly ISalesOrderHeaderService _salesOrderHeaderService;

    public SalesOrderHeadersController(ISalesOrderHeaderService salesOrderHeaderService)
    {
        _salesOrderHeaderService = salesOrderHeaderService;
    }

    /// <summary>
    /// get salesOrderHeaders by Id
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> GetSalesOrderHeadersById([FromQuery] GetSalesOrderHeaderRequest request)
    {
        var salesOrderHeaders = await _salesOrderHeaderService.GetSalesOrderHeaderByIdAsync(request);

        return View(salesOrderHeaders.Data);
    }

    /// <summary>
    /// get salesOrderHeaders
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSalesOrderHeaders([FromQuery] GetSalesOrderHeadersRequest request)
    {
        var salesOrderHeaders = await _salesOrderHeaderService.GetSalesOrderHeadersAsync(request);

        return View(salesOrderHeaders);
    }

    /// <summary>
    /// create new product
    /// </summary>
    [HttpGet]
    public IActionResult CreateHeader()
    {
        ViewBag.Customers = _salesOrderHeaderService.GetCustomerSelectListItems();

        ViewBag.Products = _salesOrderHeaderService.GetProductSelectListItems();

        return View();
    }

    /// <summary>
    /// create new salesOrderHeader
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> CreateHeader([FromForm] CreateSalesOrderHeaderRequest request)
    {
        var response = await _salesOrderHeaderService.CreateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(GetSalesOrderHeaders));

        if (response.Status.Code == Common.Enums.StatusCode.Error)
            return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Customers = _salesOrderHeaderService.GetCustomerSelectListItems();

        ViewBag.Products = _salesOrderHeaderService.GetProductSelectListItems();

        return View();
    }

    /// <summary>
    /// delete salesOrderHeader
    /// </summary>
    [HttpGet]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> DeleteHeader([FromQuery] DeleteSalesOrderHeaderRequest request)
    {
        var response = await _salesOrderHeaderService.DeleteAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Error)
            return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Customers = _salesOrderHeaderService.GetCustomerSelectListItems();

        ViewBag.Products = _salesOrderHeaderService.GetProductSelectListItems();

        return RedirectToAction(nameof(GetSalesOrderHeaders));
    }

    /// <summary>
    /// get salesOrderDetail by Id
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> GetSalesOrderDetailsById([FromQuery] GetSalesOrderDetailRequest request)
    {
        var salesOrderDetail = await _salesOrderHeaderService.GetSalesOrderDetailByIdAsync(request);

        return View(salesOrderDetail.Data);
    }

    /// <summary>
    /// get salesOrderDetails
    /// </summary>
    [HttpGet]
    public IActionResult GetSalesOrderDetails([FromQuery] GetSalesOrderDetailsRequest request)
    {
        var salesOrderDetail = _salesOrderHeaderService.GetSalesOrderDetailsAsync(request);

        return View(salesOrderDetail);
    }
}