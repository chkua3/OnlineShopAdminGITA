using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Enums;
using OnlineShopAdmin.Common.Helpers;
using OnlineShopAdmin.DataAccess.Models;
using OnlineShopAdmin.Services.ProductCategories.Messages;
using OnlineShopAdmin.Services.ProductCategories.Models;

namespace OnlineShopAdmin.Services.ProductCategories;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public List<SelectListItem> GetCategorySelectListItems()
    {
        var query = GetQuery();

        var categoriesModel = _mapper.Map<List<GetParentProductCategory>>(query);

        var categories = new GetParentProductCategories { ParentProductCategories = categoriesModel };

        var categorySelectListItems = categories.ParentProductCategories.Select(s => new SelectListItem
        {
            Value = s.ProductCategoryId.ToString(),
            Text = s.Name
        }).ToList();

        return categorySelectListItems;
    }

    public async Task<IResponse<GetProductCategoryResponse>> GetProductCategoryByIdAsync(GetProductCategoryRequest request)
    {
        var productCategory = await GetQuery()
            .FirstOrDefaultAsync(p => p.ProductCategoryId.Equals(request.ProductCategoryId));

        if (productCategory == null) return ResponseHelper.Fail<GetProductCategoryResponse>(StatusCode.ProductCategoryNotFound);

        var productCategoryResponseModel = _mapper.Map<ProductCategoryResponseModel>(productCategory);

        return ResponseHelper.Ok(new GetProductCategoryResponse { ProductCategory = productCategoryResponseModel });
    }

    public IResponse<PagedList<GetProductCategoriesResponse>> GetProductCategoriesAsync(GetProductCategoriesRequest request)
    {
        var query = GetQuery();

        #region Filter

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p => p.Name.Equals(request.Name));
        }

        if (!string.IsNullOrWhiteSpace(request.ParentProductCategoryName))
        {
            query = query.Where(p => p.ParentProductCategory.Name.Equals(request.ParentProductCategoryName));
        }

        #endregion
        
        var productCategories = new List<GetProductCategoriesResponse>();

        foreach (var productCategory in query)
        {
            productCategories.Add(new GetProductCategoriesResponse
            {
                ProductCategory = productCategory,
                ProductInCategory = productCategory.Products.Count
            });
        }

        var productCategoriesResponse =
            PagedList<GetProductCategoriesResponse>.Create(productCategories, request.PageNumber, request.PageSize);

        return ResponseHelper.Ok(productCategoriesResponse);
    }

    public async Task<IResponse<EmptyResponse>> CreateAsync(CreateProductCategoryRequest request)
    {
        if (await Exists(request.Name)) return ResponseHelper.Fail(StatusCode.ProductCategoryWithSameNameAlreadyExists);

        var product = _mapper.Map<ProductCategory>(request);

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

    public async Task<IResponse<UpdateProductCategoryRequest>> GetUpdateProductCategoryRequestModel(GetProductCategoryRequest request)
    {
        var response = await GetProductCategoryByIdAsync(request);

        if (response.Status.Code != StatusCode.Success)
            return ResponseHelper.Fail<UpdateProductCategoryRequest>(response.Status.Code, response.Status.Message);

        var updateProductRequestModel = _mapper.Map<UpdateProductCategoryRequest>(response.Data.ProductCategory);
        
        return ResponseHelper.Ok(updateProductRequestModel);
    }

    public async Task<IResponse<UpdateProductCategoryResponse>> UpdateAsync(UpdateProductCategoryRequest request)
    {
        if (await Exists(request.Name))
            return ResponseHelper.Fail<UpdateProductCategoryResponse>(StatusCode.ProductCategoryWithSameNameAlreadyExists);

        var productCategory = await GetQuery().FirstOrDefaultAsync(p => p.ProductCategoryId.Equals(request.ProductCategoryId));

        if (productCategory == null) return ResponseHelper.Fail<UpdateProductCategoryResponse>(StatusCode.ProductCategoryNotFound);

        _mapper.Map(request, productCategory);

        _unitOfWork.Update(productCategory);

        try
        {
            await _unitOfWork.CommitAsync();
            return ResponseHelper.Ok(new UpdateProductCategoryResponse { ProductCategoryId = productCategory.ProductCategoryId });
        }
        catch
        {
            return ResponseHelper.Fail<UpdateProductCategoryResponse>();
        }
    }

    public async Task<IResponse<EmptyResponse>> DeleteAsync(DeleteProductCategoryRequest request)
    {
        var productCategory = await GetQuery().FirstOrDefaultAsync(p => p.ProductCategoryId.Equals(request.ProductCategoryId));

        if (productCategory == null) return ResponseHelper.Fail(StatusCode.ProductCategoryNotFound);

        if (productCategory.Products.Any()) return ResponseHelper.Fail(StatusCode.ProductCategoryCanNotDelete);

        if (productCategory.InverseParentProductCategory != null && productCategory.InverseParentProductCategory.Any())
        {
            RecursiveDelete(productCategory);
        }

        _unitOfWork.Remove(productCategory);

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

    private void RecursiveDelete(ProductCategory productCategory)
    {
        if (productCategory.InverseParentProductCategory.Any())
        {
            var children = GetQuery()
                .Where(x => x.ParentProductCategoryId.Value.Equals(productCategory.ProductCategoryId));

            foreach (var child in children)
            {
                RecursiveDelete(child);
            }
        }

        _unitOfWork.Remove(productCategory);
    }

    private IQueryable<ProductCategory> GetQuery()
    {
        var query = _unitOfWork.Query<ProductCategory>().OrderByDescending(x => x.ModifiedDate);

        return query;
    }

    private async Task<bool> Exists(string name, int productCategoryId = 0)
    {
        var productCategories = GetQuery();

        return await productCategories.AnyAsync(pc => pc.Name.Equals(name) && !pc.ProductCategoryId.Equals(productCategoryId));
    }
}
