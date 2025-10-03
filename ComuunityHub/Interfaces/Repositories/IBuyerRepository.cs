using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface IBuyerRepository
{
    Task<Buyer?> GetByIdAsync(string id);
    Task<Buyer?> GetByUserIdAsync(string userId);
    Task AddAsync(Buyer buyer);
    Task UpdateAsync(Buyer buyer);
    Task<bool> DeleteAsync(Buyer buyer);
}