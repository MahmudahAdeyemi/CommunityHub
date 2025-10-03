using ComuunityHub.DTOs;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;
using Elastic.Clients.Elasticsearch.MachineLearning;
using Category = ComuunityHub.Models.Category;

namespace ComuunityHub.Implementation.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<BaseResponse> CreateCategory(AddCategoryRequestModel request)
    {
        var existingCategory =
            await _categoryRepository.GetByNameAndParentAsync(request.Name, request.ParentCategoryId);
        if (existingCategory != null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Category with same name already exists in this level."
            };
        }

        Category category = new Category
        {
            Name = request.Name,
            ParentCategoryId = request.ParentCategoryId
        };
        await _categoryRepository.AddAsync(category);
        return new BaseResponse
        {
            Status = true,
            Message = "Category created successfully"
        };
    }

    public async Task<BaseResponse> UpdateCategory(string categoryId,UpdateCategoryRequestModel request)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Category not found"
            };
        }
        category.Name = request.Name;
        await _categoryRepository.UpdateAsync(category);
        return new BaseResponse
        {
            Status = true,
            Message = "Category updated successfully"
        };
    }

    public async Task<BaseResponse> DeleteCategory(string categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Category not found"
            };
        }
        await _categoryRepository.DeleteAsync(category);
        return new BaseResponse
        {
            Status = true,
            Message = "Category deleted successfully"
        };
    }
    public async Task<CategoryResponseModel> GetCategoryById(string categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            return new CategoryResponseModel
            {
                Status = false,
                Message = "Category not found"
            };
        }

        return new CategoryResponseModel
        {
            Data = MapToDto(category),
            Status = true,
            Message = "Category retrieved successfully"
        };
    }

    public async Task<CategoriesResponseModel> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return new CategoriesResponseModel()
        {
            Data = categories.Select(MapToDto).ToList(),
            Status = true,
            Message = "Categories retrieved successfully"
        };
    }
    private CategoryDTO MapToDto(Category category)
    {
        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            SubCategories = category.SubCategories?.Select(MapToDto).ToList() 
                            ?? new List<CategoryDTO>()
        };
    }
    public async Task<CategoriesResponseModel> GetCategoryTree()
    {
        var categories = await _categoryRepository.GetCategoryTreeAsync();
        return new CategoriesResponseModel()
        {
            Data = categories.Select(MapToDto).ToList(),
            Status = true,
            Message = "Categories retrieved successfully"
        };
    }
}