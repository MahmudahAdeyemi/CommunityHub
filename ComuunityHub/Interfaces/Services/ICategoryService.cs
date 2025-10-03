using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface ICategoryService
{
    Task<BaseResponse> CreateCategory(AddCategoryRequestModel request);
    Task<BaseResponse> UpdateCategory(string categoryId,UpdateCategoryRequestModel request);
    Task<BaseResponse> DeleteCategory(string categoryId);
    Task<CategoryResponseModel> GetCategoryById(string categoryId);
    Task<CategoriesResponseModel> GetAllCategories();
    Task<CategoriesResponseModel> GetCategoryTree();
}