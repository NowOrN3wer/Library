using FluentValidation;
using Library.Application.Features.Writers.Add;

public sealed class AddWriterCommandValidator : AbstractValidator<AddWriterCommand>
{
    public AddWriterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("İsim alanı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("İsim en fazla 100 karakter olabilir.");

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage("Soyisim en fazla 100 karakter olabilir.");

        RuleFor(x => x.Biography)
            .MaximumLength(500)
            .WithMessage("Biyografi en fazla 500 karakter olabilir.");

        RuleFor(x => x.Nationality)
            .MaximumLength(100)
            .WithMessage("Uyruk en fazla 100 karakter olabilir.");

        RuleFor(x => x.Website)
            .MaximumLength(255).WithMessage("Web sitesi en fazla 255 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website))
            .Must(link => Uri.IsWellFormedUriString(link, UriKind.Absolute))
            .WithMessage("Geçerli bir web sitesi adresi giriniz.");

        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email en fazla 255 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}
