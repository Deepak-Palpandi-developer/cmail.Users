using Cgmail.Common.Model;
using Cmail.Users.Application.Dto.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cmail.Users.Application.Services.Auth;

public interface IAuthService
{
    Response<string> GenerateJwtToken(UserDto user);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Response<string> GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key ?? string.Empty));

        var claims = new[]
        {
            new Claim("userEmail", user.Email),
            new Claim("userName", string.Join(" ", [user.FirstName, user.LastName])),
            new Claim("lastLogin",user.LastLoginAt!.Value.ToString()),
            new Claim("id",user.Id.ToString()),
            new Claim("gender", user.Gender),
            new Claim("phone",user.Phone)
        };

        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
            signingCredentials: signIn
        );

        string authToken = new JwtSecurityTokenHandler().WriteToken(token);

        if (string.IsNullOrEmpty(authToken))
        {
            return new Response<string>
            {
                IsSuccess = false,
                Message = "Toke Creation Faild",
            };
        }

        return new Response<string>
        {
            IsSuccess = true,
            Message = "Successfully Loged in",
            Data = authToken,
        };
    }
}
