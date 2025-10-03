using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly MyContext _context;

    public CategoryRepository(MyContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category)
    {
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();
    }
    public async Task<Category?> GetByIdAsync(string id)
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Include(c => c.SubCategories)
            .ToListAsync();
    }
    public async Task<Category> UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }
    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Category>> GetCategoryTreeAsync()
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryId == null) 
            .Include(c => c.SubCategories)
            .ToListAsync();
    }
    public async Task<Category?> GetByNameAndParentAsync(string name, string? parentId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name && c.ParentCategoryId == parentId);
    }
}