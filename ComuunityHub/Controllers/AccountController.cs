using ComuunityHub.Interfaces.Services;
using ComuunityHub.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace ComuunityHub.Controllers;
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IUserService  _userService;
    private readonly IEmailOTPService _emailOTPService;

    public AccountController(IUserService userService, IEmailOTPService emailOTPService)
    {
        _userService = userService;
        _emailOTPService = emailOTPService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequestModel  model)
    {
        var response = await _userService.RegisterUser(model);
        if (response.Status)
        {
            return Ok(new { Message = "Registration successful. OTP sent to email.", UserId = response.UserId });
        }
        return BadRequest(new { Message = response.Message });
    }
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody]VerifyOtpRequestModel request)
    {
        var response = await _emailOTPService.VerifyOTP(request);

        if (response.Status)
            return Ok(new { Message = "Email verified successfully!" });

        return BadRequest(new { Message = response.Message });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestModel model)
    {
        var response = await _userService.Login(model);
        if (response.Status)
        {
            return Ok(new {isEmailConfirmed=response.isEmailConfirmed, Message = "Login successful." });
        }
        return BadRequest(new { Message = response.Message , Token  = response.Token });
    }
}