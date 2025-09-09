using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetUserAsync(string userId);
    Task<User> GetUserByEmailAsync(string email);
    Task AddUser(User user);
    Task UpdateUser(User user);
    Task RemoveUser(User user);
    Task<User> GetUserByUsernameAsync(string username);
}