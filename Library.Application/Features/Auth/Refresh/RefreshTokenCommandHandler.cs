using Library.Application.Features.Auth.Login;
using Library.Application.Services;
using Library.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Library.Application.Features.Auth.Refresh;

internal sealed class RefreshTokenCommandHandler(
    UserManager<AppUser> userManager,
    IJwtProvider jwtProvider)
    : IRequestHandler<RefreshTokenCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken.ToString(), cancellationToken);

        if (user is null)
            return (401, "Geçersiz refresh token");
        
        if (user.RefreshTokenExpires is null || user.RefreshTokenExpires <= DateTimeOffset.UtcNow)
            return (401, "Refresh token süresi dolmuş");
        
        var newTokensResult = await jwtProvider.CreateToken(user);
        
        user.RefreshToken = newTokensResult.RefreshToken;
        user.RefreshTokenExpires = newTokensResult.RefreshTokenExpires.ToUniversalTime(); ;

        var update = await userManager.UpdateAsync(user);
        if (!update.Succeeded)
            return (500, "Kullanıcı güncellenemedi");
        
        return Result<LoginCommandResponse>.Succeed(newTokensResult);
    }
}