using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotnetCheckpoint1.Dtos;
using DotnetCheckpoint1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetCheckpoint1.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new AppUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        AppUser? user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null) return Unauthorized("Invalid email or password");

        bool userIsLockedOut = await _userManager.IsLockedOutAsync(user);
        if (userIsLockedOut)
        {
            return Unauthorized("User is locked out due to too many login attempts");
        }

        bool validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
        {
            await _userManager.AccessFailedAsync(user);
            return Unauthorized("Invalid email or password");
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        string accessToken = await GenerateJwtToken(user);
        string refreshToken = await GenerateRefreshToken(user);

        return Ok(new { accessToken, refreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        AppUser? user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return Unauthorized("Invalid token");

        bool isValid = await _userManager.VerifyUserTokenAsync(
            user, "Default", "RefreshToken", request.RefreshToken);

        if (!isValid) return Unauthorized("Invalid or expired refresh token");

        string accessToken = await GenerateJwtToken(user);
        string refreshToken = await GenerateRefreshToken(user);

        return Ok(new { accessToken, refreshToken });
    }

    private async Task<string> GenerateJwtToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        List<Claim> claims = [
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            ..roles.Select<string, Claim>(role => new(ClaimTypes.Role, role))
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GenerateRefreshToken(AppUser user)
    {
        var refreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");
        await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
        return refreshToken;
    }
}