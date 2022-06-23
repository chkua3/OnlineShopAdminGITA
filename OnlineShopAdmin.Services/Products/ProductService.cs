using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Enums;
using OnlineShopAdmin.Common.Helpers;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.Products.Messages;
using OnlineShopAdmin.Services.Products.Models;

namespace OnlineShopAdmin.Services.Products;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public List<SelectListItem> GetCategorySelectListItems()
    {
        var query = _unitOfWork.Query<ProductCategory>();

        var categoriesModel = _mapper.Map<List<GetProductCategory>>(query);

        var categories = new GetProductCategories { ProductCategory = categoriesModel };
        
        var categorySelectListItems = categories.ProductCategory.Select(s => new SelectListItem
        {
            Value = s.ProductCategoryId.ToString(),
            Text = s.Name
        }).ToList();

        return categorySelectListItems;
    }

    public List<SelectListItem> GetProductModelSelectListItems()
    {
        var query = _unitOfWork.Query<ProductModel>();

        var categoriesModel = _mapper.Map<List<GetProductModel>>(query);

        var productModels = new GetProductModels { ProductModels = categoriesModel };

        var productModelSelectListItems = productModels.ProductModels.Select(s => new SelectListItem
        {
            Value = s.ProductModelId.ToString(),
            Text = s.Name
        }).ToList();

        return productModelSelectListItems;
    }

    public async Task<IResponse<GetProductResponse>> GetProductByIdAsync(GetProductRequest request)
    {
        var product = await GetQuery().FirstOrDefaultAsync(p => p.ProductId.Equals(request.ProductId));

        if (product == null) return ResponseHelper.Fail<GetProductResponse>(StatusCode.ProductNotFound);

        var productResponseModel = _mapper.Map<ProductResponseModel>(product);

        var fileName = FileHelper.DownloadFile(product.ThumbNailPhoto);
        productResponseModel.FileName = fileName;
        productResponseModel.FileExtension = Path.GetExtension(product.ThumbnailPhotoFileName)?.TrimStart('.');

        return ResponseHelper.Ok(new GetProductResponse { Product = productResponseModel });
    }

    public IResponse<PagedList<GetProductsResponse>> GetProductsAsync(GetProductsRequest request)
    {
        var query = GetQuery();

        #region Filter

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(c => c.Name.Equals(request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.ProductNumber))
        {
            query = query.Where(c => c.ProductNumber.Equals(request.ProductNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.Color))
        {
            query = query.Where(c => c.Color.Equals(request.Color));
        }

        if (request.StandardCost.HasValue)
        {
            query = query.Where(c => c.StandardCost.Equals(request.StandardCost));
        }

        if (request.ListPrice.HasValue)
        {
            query = query.Where(c => c.ListPrice.Equals(request.ListPrice));
        }

        if (!string.IsNullOrWhiteSpace(request.Size))
        {
            query = query.Where(c => c.Size.Equals(request.Size));
        }

        if (request.Weight.HasValue)
        {
            query = query.Where(c => c.Weight.Equals(request.Weight));
        }

        #endregion

        var products = new List<GetProductsResponse>();

        foreach (var product in query)
        {
            var orders = product.SalesOrderDetails.Select(x => x.OrderQty);
            var orderQuantity = orders.Aggregate(0, (current, order) => current + order);

            products.Add(new GetProductsResponse { Product = product, OrdersQuantity = orderQuantity });
        }

        var productsResponse = PagedList<GetProductsResponse>.Create(products, request.PageNumber, request.PageSize);

        return ResponseHelper.Ok(productsResponse);
    }

    public async Task<IResponse<EmptyResponse>> CreateAsync(CreateProductRequest request)
    {
        if (await Exists(request.Name, request.ProductNumber)) return ResponseHelper.Fail(StatusCode.ProductWithSameNameOrNumberAlreadyExists);

        var product = _mapper.Map<Product>(request);

        if (request.File != null)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fileName = FileHelper.SaveFile(request.File, folderPath);
            var fileBytes = await request.File.GetBytes();
            
            product.ThumbNailPhoto = fileBytes;
            product.ThumbnailPhotoFileName = fileName;
        }

        await _unitOfWork.AddAsync(product);

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

    public async Task<IResponse<UpdateProductRequest>> GetUpdateProductRequestModel(GetProductRequest request)
    {
        var response = await GetProductByIdAsync(request);

        if (response.Status.Code != StatusCode.Success)
            return ResponseHelper.Fail<UpdateProductRequest>(response.Status.Code, response.Status.Message);
        
        var updateProductRequestModel = _mapper.Map<UpdateProductRequest>(response.Data.Product);

        return ResponseHelper.Ok(updateProductRequestModel);
    }

    public async Task<IResponse<UpdateProductResponse>> UpdateAsync(UpdateProductRequest request)
    {
        if (await Exists(request.Name, request.ProductNumber, request.ProductId))
            return ResponseHelper.Fail<UpdateProductResponse>(StatusCode.ProductWithSameNameOrNumberAlreadyExists);

        var product = await GetQuery().FirstOrDefaultAsync(p => p.ProductId.Equals(request.ProductId));

        if (product == null) return ResponseHelper.Fail<UpdateProductResponse>(StatusCode.ProductNotFound);

        _mapper.Map(request, product);

        if (request.File != null)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            FileHelper.DeleteFile($"{folderPath}/{product.ThumbnailPhotoFileName}");

            var fileName = FileHelper.SaveFile(request.File, folderPath);
            var fileBytes = await request.File.GetBytes();

            product.ThumbNailPhoto = fileBytes;
            product.ThumbnailPhotoFileName = fileName;
        }

        _unitOfWork.Update(product);

        try
        {
            await _unitOfWork.CommitAsync();

            return ResponseHelper.Ok(new UpdateProductResponse { ProductId = product.ProductId });
        }
        catch
        {
            return ResponseHelper.Fail<UpdateProductResponse>();
        }
    }

    public async Task<IResponse<EmptyResponse>> DeleteAsync(DeleteProductRequest request)
    {
        var product = await GetQuery().FirstOrDefaultAsync(p => p.ProductId.Equals(request.ProductId));

        if (product == null) return ResponseHelper.Fail(StatusCode.ProductNotFound);

        if (product.SalesOrderDetails.Any()) return ResponseHelper.Fail(StatusCode.ProductCanNotDelete);

        _unitOfWork.Remove(product);

        try
        {
            await _unitOfWork.CommitAsync();

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            FileHelper.DeleteFile($"{folderPath}/{product.ThumbnailPhotoFileName}");

            return ResponseHelper.Ok();
        }
        catch
        {
            return ResponseHelper.Fail();
        }
    }

    private IQueryable<Product> GetQuery()
    {
        var query = _unitOfWork.Query<Product>().OrderByDescending(product=>product.ModifiedDate);

        return query;
    }

    private async Task<bool> Exists(string productName, string productNumber, int productId = 0)
    {
        var products = GetQuery();

        if (await products.AnyAsync(product => product.Name.Equals(productName) && !product.ProductId.Equals(productId)))
            return true;

        if (await products.AnyAsync(product => product.ProductNumber.Equals(productNumber) && !product.ProductId.Equals(productId)))
            return true;

        return false;
    }
}
