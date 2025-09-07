using FluentValidation;

namespace Library.Application.Features.Auth.Refresh;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(p => p.RefreshToken)
            .NotEmpty().WithMessage("Token boş olamaz")
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir Token giriniz");
    }
}