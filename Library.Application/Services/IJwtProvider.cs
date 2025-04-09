using Library.Application.Features.Auth.Login;
using Library.Domain.Entities;

namespace Library.Application.Services
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateToken(AppUser user);
    }
}
