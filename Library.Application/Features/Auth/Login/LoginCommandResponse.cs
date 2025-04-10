namespace Library.Application.Features.Auth.Login;

public sealed record LoginCommandResponse(
    string Token,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpires);