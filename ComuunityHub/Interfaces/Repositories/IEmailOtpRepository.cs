using ComuunityHub.Models;

namespace ComuunityHub.Interfaces.Repositories;

public interface IEmailOtpRepository
{
    Task SaveOtpAsync(EmailOTP otp);
    Task<EmailOTP?> GetValidOtpAsync(string userId, string code);
    Task MarkAsUsedAsync(EmailOTP otp);
    Task<EmailOTP> GetLatestOtpByUserAsync(string userId);
}