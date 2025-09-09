using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface IEmailOTPService
{
    Task SendandGenerateOTP(string userId);
    Task<BaseResponse> VerifyOTP(VerifyOtpRequestModel request);
}