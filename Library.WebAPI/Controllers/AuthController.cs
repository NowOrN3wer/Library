using Library.Application.Features.Auth.Login;
using Library.Application.Features.Auth.Refresh;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

[AllowAnonymous]
public sealed class AuthController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>Refresh token ile yeni access + refresh üret (rotate)</summary>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand request, CancellationToken ct)
    {
        var res = await Mediator.Send(request, ct);
        return StatusCode(res.StatusCode, res);
    }

    [AllowAnonymous]
    [HttpPost("refresh-cookie")]
    public async Task<IActionResult> RefreshFromCookie(CancellationToken ct)
    {
        var rawToken = Request.Cookies["refresh_token"];
        if (string.IsNullOrWhiteSpace(rawToken))
            return BadRequest(new { error = "Refresh token bulunamadı" });

        if (!Guid.TryParse(rawToken, out var tokenGuid))
            return BadRequest(new { error = "Geçersiz refresh token formatı" });

        var res = await Mediator.Send(new RefreshTokenCommand(tokenGuid), ct);

        if (res.IsSuccessful && res.Data is not null)
            Response.Cookies.Append("refresh_token", res.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = res.Data.RefreshTokenExpires
            });

        return StatusCode(res.StatusCode, res);
    }
}