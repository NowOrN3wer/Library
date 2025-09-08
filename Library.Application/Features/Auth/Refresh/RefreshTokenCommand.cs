using Library.Application.Features.Auth.Login;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Auth.Refresh;

public sealed record RefreshTokenCommand(Guid RefreshToken)
    : IRequest<Result<LoginCommandResponse>>;