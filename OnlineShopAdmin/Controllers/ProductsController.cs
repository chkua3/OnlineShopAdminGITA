using Microsoft.AspNetCore.Mvc;
using OnlineShopAdmin.Base.Filters;
using OnlineShopAdmin.Services.Products;
using OnlineShopAdmin.Services.Products.Messages;

namespace OnlineShopAdmin.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// get product by Id
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> GetProductById([FromQuery] GetProductRequest request)
    {
        var product = await _productService.GetProductByIdAsync(request);

        return View(product.Data);
    }

    /// <summary>
    /// get products
    /// </summary>
    [HttpGet]
    public IActionResult GetProducts([FromQuery] GetProductsRequest request)
    {
        var products = _productService.GetProductsAsync(request);

        return View(products);
    }

    /// <summary>
    /// create new product
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = _productService.GetCategorySelectListItems();

        ViewBag.ProductModels = _productService.GetProductModelSelectListItems();

        return View();
    }

    /// <summary>
    /// create new product
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
    {
        var response = await _productService.CreateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(GetProducts));

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Categories = _productService.GetCategorySelectListItems();

        ViewBag.ProductModels = _productService.GetProductModelSelectListItems();

        return View();
    }

    /// <summary>
    /// update new product
    /// </summary>
    [HttpGet]
    [ValidateModel]
    public async Task<IActionResult> Update([FromQuery] GetProductRequest request)
    {
        var response = await _productService.GetUpdateProductRequestModel(request);

        if (response.Data == null || response.Status.Code == Common.Enums.StatusCode.ProductNotFound) return NotFound();

        ViewBag.Categories = _productService.GetCategorySelectListItems();

        ViewBag.ProductModels = _productService.GetProductModelSelectListItems();

        return View(response.Data);
    }

    /// <summary>
    /// update product
    /// </summary>
    [HttpPost]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Update([FromForm] UpdateProductRequest request)
    {
        var response = await _productService.UpdateAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Success)
            return RedirectToAction(nameof(Update),new { response.Data.ProductId});

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        ViewBag.Categories = _productService.GetCategorySelectListItems();

        ViewBag.ProductModels = _productService.GetProductModelSelectListItems();
        
        return View();
    }

    /// <summary>
    /// delete product
    /// </summary>
    [HttpGet]
    [ValidateModel]
    [ServiceFilter(typeof(UnitOfWorkFilterAttribute))]
    public async Task<IActionResult> Delete([FromQuery] DeleteProductRequest request)
    {
        var response = await _productService.DeleteAsync(request);

        if (response.Status.Code == Common.Enums.StatusCode.Error) return BadRequest();

        TempData["ErrorMessage"] = response.Status.Message;

        return RedirectToAction(nameof(GetProducts));
    }
}