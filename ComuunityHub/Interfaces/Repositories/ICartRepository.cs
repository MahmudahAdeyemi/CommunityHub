using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserId(string userId);
    Task AddCartAsync(Cart cart);
    Task UpdateCartAsync(Cart cart);
    Task EmptyCartAsync(Cart cart);
    Task<List<Cart>> GetExpiredCarts();
}