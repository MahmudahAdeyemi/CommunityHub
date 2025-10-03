using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(string id);
    Task<List<Category>> GetAllAsync();
    Task<Category> UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<Category?> GetByNameAndParentAsync(string name, string? parentId);
    Task<List<Category>> GetCategoryTreeAsync();
}