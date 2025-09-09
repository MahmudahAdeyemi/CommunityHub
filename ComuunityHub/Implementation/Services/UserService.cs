using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ComuunityHub.Interfaces.Repositories;
using ComuunityHub.Interfaces.Services;
using ComuunityHub.Models;
using ComuunityHub.RequestModels;
using ComuunityHub.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace ComuunityHub.Implementation.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;
    private readonly IEmailOTPService  _emailOTPService;
    public UserService(IUserRepository userRepository, IConfiguration config, IEmailOTPService emailOTPService)
    {
        _userRepository = userRepository;
        _config = config;
        _emailOTPService = emailOTPService;

    }

    public async Task<LoginUserResponseModel> Login(LoginRequestModel model)
    {
        var user =await _userRepository.GetUserByEmailAsync(model.Email);
        if (user == null)
        {
            return new LoginUserResponseModel
            {
                isEmailConfirmed = true,
                Message = "You need to register",
                Status = false
            };
        }

        if (!user.IsEmailConfirmed)
        {
            return new LoginUserResponseModel
            {
                isEmailConfirmed = false,
                Message = "You need to verify your email before doing anything",
                Status = false
            };
        }
        if (user.Password != HashPassword(model.Password))
        {
            return new LoginUserResponseModel
            { 
                isEmailConfirmed = true,
                Message = "Invalid email or password",
                Status = false
            };
        }

        var token = await GenerateJwtToken(user);
        return new LoginUserResponseModel
        {
            Token=token,
            isEmailConfirmed = true,
            Message = "Login successful",
            Status = true
        };
        
    }
    
    public Task<string> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public async Task<RegisterUserResponseModel> RegisterUser(RegisterUserRequestModel model)
    {
        var emailCheck =await _userRepository.GetUserByEmailAsync(model.Email);
        if (emailCheck != null)
        {
            return new RegisterUserResponseModel
            {
                Message = "Email already exists",
                Status = false
            };
        }
        var userNameCheck = await _userRepository.GetUserByUsernameAsync(model.Username);
        if (userNameCheck != null)
        {
            return new RegisterUserResponseModel
            {
                Message = "Username already exists",
                Status = false
            };
        }
        User user = new User()
        {
            Address = model.Address,
            Email = model.Email,
            Password = HashPassword(model.Password),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            IsEmailConfirmed = false
        };
        await _userRepository.AddUser(user);
        await _emailOTPService.SendandGenerateOTP(user.Id);
        return new RegisterUserResponseModel
        {
            UserId = user.Id,
            Message = "User registered successfully",
            Status = true
        };
    }
    
    public static string HashPassword(string plainText)
    {
        var hashedPaswordBytes = SHA512.HashData(Encoding.UTF8.GetBytes(plainText));

        StringBuilder builder = new();
        foreach (var b in hashedPaswordBytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }
}