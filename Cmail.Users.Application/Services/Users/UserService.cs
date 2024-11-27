using AutoMapper;
using Cgmail.Common.Model;
using Cmail.Users.Application.Dto.Users;
using Cmail.Users.Domain.Entities.Users;
using Cmail.Users.Domain.Repository.Users;
using Microsoft.Extensions.Configuration;

namespace Cmail.Users.Application.Services.Users;

public interface IUserService
{
    Task<Response<UserDto?>> GetUser(Guid userId);
    Task<Response<UserDto?>> CreateUser(UserDto userDto, string myIP);
    Task<Response<UserDto?>> Login(string email, string password);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<Response<UserDto?>> GetUser(Guid userId)
    {
        var data = await _userRepository.GetUserDetailsAsync(userId);

        var mappedData = data != null ? _mapper.Map<UserDto>(data) : null;

        return new Response<UserDto?>
        {
            IsSuccess = data != null,
            Message = data != null ? "User details fetched successfully" : "User not found",
            Data = mappedData,
        };
    }

    public async Task<Response<UserDto?>> CreateUser(UserDto userDto, string myIP)
    {
        var userEntity = _mapper.Map<User>(userDto);

        if (userEntity == null) return new Response<UserDto?> { IsSuccess = false, Message = "Invalid user data" };

        userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        userEntity.CreatedAt = DateTime.UtcNow;
        userEntity.IsActive = true;
        userEntity.CreatedIp = myIP;
        userEntity.LastLoginAt = DateTime.UtcNow;
        userEntity.VerificationCode = GenerateOtp();
        userEntity.VerificationCodeExpireAt = DateTime.UtcNow.AddMinutes(5);

        await _userRepository.CreateUserAsync(userEntity);

        var createdUser = await _userRepository.GetUserDetailsAsync(userEntity.Id);


        var mappedData = createdUser != null ? _mapper.Map<UserDto>(createdUser) : null;

        return new Response<UserDto?>
        {
            IsSuccess = createdUser != null,
            Message = createdUser != null ? "User created successfully" : "User creation failed",
            Data = mappedData,
        };
    }

    public async Task<Response<UserDto?>> Login(string email, string password)
    {
        var user = await _userRepository.GetUserDetailsAsync(email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return new Response<UserDto?>
            {
                IsSuccess = false,
                Message = "Invalid username or password",
            };
        }

        if (!user.IsVerified)
            return new Response<UserDto?>
            {
                IsSuccess = false,
                Message = "User not verified",
            };

        user.LastLoginAt = DateTime.UtcNow;

        await _userRepository.UpdateUserAsync(user);

        var userDto = _mapper.Map<UserDto>(user);

        return new Response<UserDto?>
        {
            IsSuccess = true,
            Message = "Login successful",
            Data = userDto,
        };
    }

    #region Private
    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
    #endregion
}

