using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Library.Application.Common.Interfaces;
using Library.Application.Extensions;
using Library.Application.Features.Auth.Login;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Library.Infrastructure.Services;

internal class JwtProvider(
    UserManager<AppUser> userManager,
    IOptions<JwtOptions> jwtOptions,
    ITimeService timeService) : IJwtProvider
{
    public async Task<LoginCommandResponse> CreateToken(AppUser user)
    {
        List<Claim> claims = new()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Name", user.FullName),
            new Claim("Email", user.Email ?? ""),
            new Claim("UserName", user.UserName ?? "")
        };

        var now = timeService.UtcNow;
        var expires = now.AddHours(1);
        var refreshTokenExpires = expires.AddHours(1);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));

        JwtSecurityToken jwtSecurityToken = new(
            jwtOptions.Value.Issuer,
            jwtOptions.Value.Audience,
            claims,
            now.UtcDateTime,
            expires.UtcDateTime,
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512));

        JwtSecurityTokenHandler handler = new();
        var token = handler.WriteToken(jwtSecurityToken);

        var refreshToken = Guid.NewGuid().ToString();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = refreshTokenExpires;

        await userManager.UpdateAsync(user);

        return new LoginCommandResponse(token, refreshToken, refreshTokenExpires.ToTurkeyTime());
    }
}