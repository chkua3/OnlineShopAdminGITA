using Microsoft.AspNetCore.Mvc;
using OnlineShopAdmin.Base.Filters;
using OnlineShopAdmin.Services.Customers;
using OnlineShopAdmin.Services.Customers.Messages;

namespace OnlineShopAdmin.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// get customer by Id
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> GetCustomerById([FromQuery] GetCustomerRequest request)
    {
        var customer = await _customerService.GetCustomerByIdAsync(request);

        return View(customer.Data);
    }

    /// <summary>
    /// get customers
    /// </summary>
    [HttpGet]
    public IActionResult GetCustomers([FromQuery] GetCustomersRequest request)
    {
        var customers = _customerService.GetCustomersAsync(request);

        return View(customers);
    }

    /// <summary>
    /// create new customer
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Addresses = _customerService.GetAddressSelectListItems();

        return View();
    }

    /// <summary>
    /// create new customer
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Create([FromForm] CreateCustomerRequest request)
    {
        var response = await _customerService.CreateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(GetCustomers));

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Addresses = _customerService.GetAddressSelectListItems();

        return View();
    }

    /// <summary>
    /// update new customer
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> Update([FromQuery] GetCustomerRequest request)
    {
        var response = await _customerService.GetUpdateCustomerRequestModel(request);

        if (response.Data == null || response.Status.Code == Common.Enums.StatusCode.ProductNotFound) return NotFound();

        ViewBag.Addresses = _customerService.GetAddressSelectListItems();

        return View(response.Data);
    }

    /// <summary>
    /// update customer
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Update([FromForm] UpdateCustomerRequest request)
    {
        var response = await _customerService.UpdateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(Update), new { response.Data.CustomerId });

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Addresses = _customerService.GetAddressSelectListItems();
        
        return View();
    }

    /// <summary>
    /// delete customer
    /// </summary>
    [HttpGet]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Delete([FromQuery] DeleteCustomerRequest request)
    {
        var response = await _customerService.DeleteAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Addresses = _customerService.GetAddressSelectListItems();

        return RedirectToAction(nameof(GetCustomers));
    }
}