using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<Address> GetByIdAsync(string id);
    Task<Address> AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(Address address);
}