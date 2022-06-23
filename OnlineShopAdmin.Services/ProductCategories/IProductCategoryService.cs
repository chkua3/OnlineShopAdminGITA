using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Services.ProductCategories.Messages;

namespace OnlineShopAdmin.Services.ProductCategories;

public interface IProductCategoryService
{
    List<SelectListItem> GetCategorySelectListItems();

    Task<IResponse<GetProductCategoryResponse>> GetProductCategoryByIdAsync(GetProductCategoryRequest request);

    IResponse<PagedList<GetProductCategoriesResponse>> GetProductCategoriesAsync(GetProductCategoriesRequest request);

    Task<IResponse<EmptyResponse>> CreateAsync(CreateProductCategoryRequest request);

    Task<IResponse<UpdateProductCategoryRequest>> GetUpdateProductCategoryRequestModel(GetProductCategoryRequest request);

    Task<IResponse<UpdateProductCategoryResponse>> UpdateAsync(UpdateProductCategoryRequest request);

    Task<IResponse<EmptyResponse>> DeleteAsync(DeleteProductCategoryRequest request);
}