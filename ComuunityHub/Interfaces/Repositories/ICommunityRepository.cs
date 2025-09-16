using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface ICommunityRepository
{
    Task CreateCommunity(Community community);
    Task<Community?> GetByIdAsync(string id);
    Task<List<Community>> GetAllAsync();
    Task<IEnumerable<Community>> GetUserCommunity(string userId);
    Task<Community?> GetCommunityByName(string name);
    Task UpdateCommunity(Community community);
}