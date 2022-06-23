using Microsoft.AspNetCore.Mvc;
using OnlineShopAdmin.Base.Filters;
using OnlineShopAdmin.Services.Addresses;
using OnlineShopAdmin.Services.Addresses.Messages;

namespace OnlineShopAdmin.Controllers;

public class AddressesController : Controller
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    /// <summary>
    /// get address by Id
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> GetAddressById([FromQuery] GetAddressRequest request)
    {
        var address = await _addressService.GetAddressByIdAsync(request);

        return View(address.Data);
    }

    /// <summary>
    /// get addresses
    /// </summary>
    [HttpGet]
    public IActionResult GetAddresses([FromQuery] GetAddressesRequest request)
    {
        var addresses = _addressService.GetAddressesAsync(request);

        return View(addresses);
    }

    /// <summary>
    /// create new address
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// create new address
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Create([FromForm] CreateAddressRequest request)
    {
        var response = await _addressService.CreateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success) return RedirectToAction(nameof(GetAddresses));

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        return View();
    }

    /// <summary>
    /// update new customer
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> Update([FromQuery] GetAddressRequest request)
    {
        var response = await _addressService.GetUpdateAddressRequestModel(request);

        if (response.Data == null || response.Status.Code == Common.Enums.StatusCode.ProductNotFound) return NotFound();

        return View(response.Data);
    }

    /// <summary>
    /// update address
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Update([FromForm] UpdateAddressRequest request)
    {
        var response = await _addressService.UpdateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(Update), new { response.Data.AddressId });

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        return View();
    }

    /// <summary>
    /// delete address
    /// </summary>
    [HttpGet]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Delete([FromQuery] DeleteAddressRequest request)
    {
        var response = await _addressService.DeleteAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        return RedirectToAction(nameof(GetAddresses));
    }
}