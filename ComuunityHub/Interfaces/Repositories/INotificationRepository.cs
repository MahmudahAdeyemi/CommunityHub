using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
}