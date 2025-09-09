using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;

namespace ComuunityHub.Interfaces.Services;

public interface IUserService
{
    Task<BaseResponse> Login(LoginRequestModel model);
    Task<string> GenerateJwtToken(User user);
    Task<RegisterUserResponseModel> RegisterUser(RegisterUserRequestModel model);
}