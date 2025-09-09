using ComuunityHub.Data;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ComuunityHub.Implementation.Repositories;

public class EmailOtpRepository : IEmailOtpRepository
{
    private readonly MyContext _context;
    
    public EmailOtpRepository(MyContext context)
    {
        _context = context;
    }
    public async Task SaveOtpAsync(EmailOTP otp)
    {
        _context.EmailOTPs.Add(otp);
        await _context.SaveChangesAsync();
    }
    public async Task<EmailOTP?> GetValidOtpAsync(string userId, string code)
    {
        return await _context.EmailOTPs
            .Where(o => o.UserId == userId
                        && o.OTP == code
                        && !o.IsUsed
                        && o.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();

    }
    public async Task MarkAsUsedAsync(EmailOTP otp)
    {
        otp.IsUsed = true;
        await _context.SaveChangesAsync();
    }

    public async Task<EmailOTP> GetLatestOtpByUserAsync(string userId)
    {
        return await _context.EmailOTPs
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt) 
            .FirstOrDefaultAsync();
    }
}