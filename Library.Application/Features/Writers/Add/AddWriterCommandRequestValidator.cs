using FluentValidation;
using Library.Application.Features.Writers.Add;

public sealed class AddWriterCommandValidator : AbstractValidator<AddWriterCommand>
{
    public AddWriterCommandValidator()
    {
        RuleFor(x => x.firstName)
            .NotEmpty().WithMessage("İsim alanı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("İsim en fazla 100 karakter olabilir.");

        RuleFor(x => x.lastName)
            .MaximumLength(100)
            .WithMessage("Soyisim en fazla 100 karakter olabilir.");

        RuleFor(x => x.biography)
            .MaximumLength(500)
            .WithMessage("Biyografi en fazla 500 karakter olabilir.");

        RuleFor(x => x.nationality)
            .MaximumLength(100)
            .WithMessage("Uyruk en fazla 100 karakter olabilir.");

        RuleFor(x => x.website)
            .MaximumLength(255).WithMessage("Web sitesi en fazla 255 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.website))
            .Must(link => Uri.IsWellFormedUriString(link, UriKind.Absolute))
            .WithMessage("Geçerli bir web sitesi adresi giriniz.");

        RuleFor(x => x.email)
            .MaximumLength(255).WithMessage("Email en fazla 255 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.email))
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}
