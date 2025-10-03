using ComuunityHub.Data;
using ComuunityHub.Models;

namespace ComuunityHub.Implementation.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly MyContext _context;
    public NotificationRepository(MyContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.AddAsync(notification);
        await _context.SaveChangesAsync();
    }
}