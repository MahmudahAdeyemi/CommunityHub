using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface IProductService
{
    Task<BaseResponse> AddProduct(ProductRequestModel request,string communityId);
    Task<ProductResponseModel> GetProductById(string productId);
    Task<ProductsResponseModel> GetProductByCommunity(string communityId);
    Task<ProductsResponseModel> GetProductBySeller(string sellerId);
    Task<BaseResponse> UpdateProduct(string productId, ProductRequestModel request);
    Task<BaseResponse> DeleteProduct(string productId);
}