using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;

namespace ComuunityHub.Implementation.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly MyContext _context;
    public AddressRepository(MyContext context)
    {
        _context = context;
    }
    
    public async Task<Address> GetByIdAsync(string id)
    {
        return await _context.Addresses.FindAsync(id);
    }

    public async Task<Address> AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address address)
    {
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
    }
}