using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Implementation.Services;

public class EmailOTPService : IEmailOTPService
{
    private readonly IEmailOtpRepository  _emailOtpRepository;
    private readonly IUserRepository  _userRepository;
    private readonly IEmailService  _emailService;

    public EmailOTPService(IEmailOtpRepository emailOtpRepository, IUserRepository userRepository,
        IEmailService emailService)
    {
        _emailOtpRepository = emailOtpRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task SendandGenerateOTP(string userId)
    {
        var otpCode = new Random().Next(100000, 999999).ToString();

        var otp = new EmailOTP()
        {
            UserId = userId,
            OTP = otpCode,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };
        var user = await _userRepository.GetUserAsync(userId);
        await _emailOtpRepository.SaveOtpAsync(otp);
        await _emailService.SendEmailAsync(user.Email, "Email Verification OTP",
            $"Your verification code is: <h1>{otpCode}</h1>. It expires in 10 minutes.");
    }

    public async Task<BaseResponse> VerifyOTP(VerifyOtpRequestModel request)
    {
        var user = await _userRepository.GetUserAsync(request.UserId);
        var lastOtp = await _emailOtpRepository.GetLatestOtpByUserAsync(request.UserId);
        if (lastOtp == null)
        {
            return new BaseResponse
            {
                Message = "You do not have any otp verification code.",
                Status = false
            };
        }

        if (lastOtp.IsUsed)
        {
            return new BaseResponse
            {
                Message = "OTP already used",
                Status = false
            };
        }

        if (lastOtp.ExpiresAt < DateTime.UtcNow)
        {
            return new BaseResponse
            {
                Message = "OTP has expired",
                Status = false
            };
        }

        if (lastOtp.OTP != request.Otp)
        {
            return new BaseResponse
            {
                Message = "OTP doesn't match",
                Status = false
            };
        }
        
        await _emailOtpRepository.MarkAsUsedAsync(lastOtp);
        user.IsEmailConfirmed = true;
        await _userRepository.UpdateUser(user);
        return new BaseResponse
        {
            Message = "OTP verified",
            Status = true
        };
    }
}