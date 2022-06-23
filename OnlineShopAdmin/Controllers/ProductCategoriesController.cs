using Microsoft.AspNetCore.Mvc;
using OnlineShopAdmin.Base.Filters;
using OnlineShopAdmin.Services.ProductCategories;
using OnlineShopAdmin.Services.ProductCategories.Messages;

namespace OnlineShopAdmin.Controllers;

public class ProductCategoryController : Controller
{
    private readonly IProductCategoryService _productCategoryService;

    public ProductCategoryController(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    /// <summary>
    /// get product category by Id
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> GetProductCategoryById([FromQuery] GetProductCategoryRequest request)
    {
        var productCategory = await _productCategoryService.GetProductCategoryByIdAsync(request);

        return View(productCategory.Data);
    }

    /// <summary>
    /// get product category
    /// </summary>
    [HttpGet]
    public IActionResult GetProductCategories([FromQuery] GetProductCategoriesRequest request)
    {
        var productCategories = _productCategoryService.GetProductCategoriesAsync(request);

        return View(productCategories);
    }

    /// <summary>
    /// create new product category
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = _productCategoryService.GetCategorySelectListItems();

        return View();
    }

    /// <summary>
    /// create new product category
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Create([FromForm] CreateProductCategoryRequest request)
    {
        var response = await _productCategoryService.CreateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(GetProductCategories));

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Categories = _productCategoryService.GetCategorySelectListItems();

        return View();
    }

    /// <summary>
    /// update new product
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> Update([FromQuery] GetProductCategoryRequest request)
    {
        var response = await _productCategoryService.GetUpdateProductCategoryRequestModel(request);
        
        if (response.Data == null || response.Status.Code == Common.Enums.StatusCode.ProductCategoryNotFound) return NotFound();

        ViewBag.Categories = _productCategoryService.GetCategorySelectListItems();

        return View(response.Data);
    }

    /// <summary>
    /// update product category
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Update([FromForm] UpdateProductCategoryRequest request)
    {
        var response = await _productCategoryService.UpdateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(Update), new { response.Data.ProductCategoryId });

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Categories = _productCategoryService.GetCategorySelectListItems();
        
        return View();
    }

    /// <summary>
    /// delete product category
    /// </summary>
    [HttpGet]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Delete([FromQuery] DeleteProductCategoryRequest request)
    {
        var response = await _productCategoryService.DeleteAsync(request);
        
        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        return RedirectToAction(nameof(GetProductCategories));
    }
}