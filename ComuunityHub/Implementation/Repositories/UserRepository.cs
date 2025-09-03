using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MyContext _context;
    public UserRepository(MyContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserAsync(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}