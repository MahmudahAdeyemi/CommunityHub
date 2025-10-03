using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product> AddAsync(Product product);
    Task<Product?> GetByIdAsync(string productId);
    Task<List<Product>> GetByCommunityAsync(string communityId);
    Task<IEnumerable<Product>> GetBySellerAsync(string sellerId);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}