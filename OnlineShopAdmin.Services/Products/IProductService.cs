using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Services.Products.Messages;

namespace OnlineShopAdmin.Services.Products;

public interface IProductService
{
    List<SelectListItem> GetCategorySelectListItems();
    
    List<SelectListItem> GetProductModelSelectListItems();

    Task<IResponse<GetProductResponse>> GetProductByIdAsync(GetProductRequest request);

    IResponse<PagedList<GetProductsResponse>> GetProductsAsync(GetProductsRequest request);

    Task<IResponse<EmptyResponse>> CreateAsync(CreateProductRequest request);

    Task<IResponse<UpdateProductRequest>> GetUpdateProductRequestModel(GetProductRequest request);
    
    Task<IResponse<UpdateProductResponse>> UpdateAsync(UpdateProductRequest request);

    Task<IResponse<EmptyResponse>> DeleteAsync(DeleteProductRequest request);
}