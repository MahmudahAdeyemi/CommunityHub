using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface ISellerRepository
{
    Task<ICollection<Seller>> GetByUserAsync(string userId);
    Task<Seller?> GetByIdAsync(string sellerId);
    Task<Seller?> GetByUserAndCommunityAsync(string userId, string communityId);
    Task<List<Seller>> GetByCommunityAsync(string communityId);
    Task DeleteAsync(Seller seller);
    Task<Seller> AddAsync(Seller seller);
    Task UpdateAsync(Seller seller);
}