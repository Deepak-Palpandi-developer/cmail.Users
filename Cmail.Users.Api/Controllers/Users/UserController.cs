using Cgmail.Common.Exceptions;
using Cgmail.Common.Helpers;
using Cmail.Users.Application.Dto.Users;
using Cmail.Users.Application.Models.Users;
using Cmail.Users.Application.Services.Auth;
using Cmail.Users.Application.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cmail.Users.Api.Controllers.Users;


[Authorize]
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger,
        IAuthService authService, IConfiguration configuration)
    {
        _userService = userService;
        _logger = logger;
        _authService = authService;
    }

    [HttpPost("get-user-by-id")]
    public async Task<IActionResult> GetUserById([FromBody] Guid userId)
    {
        try
        {
            var result = await _userService.GetUser(userId);

            return Ok(result);
        }
        catch (BadRequestException brex)
        {
            _logger.LogWarning(brex, brex.Message);
            return BadRequest(new
            {
                brex.Message
            });
        }
        catch (NotFoundException nfx)
        {
            _logger.LogWarning(nfx, nfx.Message);
            return NotFound(new { nfx.Source, nfx.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(417, new { ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
    {
        try
        {
            string? myIP = CommonHelper.GetIPAddress(HttpContext) ?? string.Empty;

            var result = await _userService.CreateUser(userDto, myIP);

            return Ok(result);
        }
        catch (BadRequestException brex)
        {
            _logger.LogWarning(brex, brex.Message);
            return BadRequest(new
            {
                brex.Message
            });
        }
        catch (NotFoundException nfx)
        {
            _logger.LogWarning(nfx, nfx.Message);
            return NotFound(new { nfx.Source, nfx.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(417, new { ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("log-in")]
    public async Task<IActionResult> LogIn([FromBody] LoginRequest request)
    {
        try
        {
            var userResult = await _userService.Login(request.Email, request.Password);

            if (userResult.IsSuccess && userResult.Data != null)
            {
                var response = _authService.GenerateJwtToken(userResult.Data);
                return Ok(response);
            }
            return Ok(userResult);
        }
        catch (BadRequestException brex)
        {
            _logger.LogWarning(brex, brex.Message);
            return BadRequest(new
            {
                brex.Message
            });
        }
        catch (NotFoundException nfx)
        {
            _logger.LogWarning(nfx, nfx.Message);
            return NotFound(new { nfx.Source, nfx.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(417, new { ex.Message });
        }
    }

}
