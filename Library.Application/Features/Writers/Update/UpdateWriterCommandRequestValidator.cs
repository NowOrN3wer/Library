using FluentValidation;

namespace Library.Application.Features.Writers.Update;

public class UpdateWriterCommandRequestValidator : AbstractValidator<UpdateWriterCommand>
{
    public UpdateWriterCommandRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id alanı boş olamaz.");
        
        RuleFor(x => x.FirstName)
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
            .Must(link => string.IsNullOrWhiteSpace(link) || Uri.IsWellFormedUriString(link, UriKind.Absolute))
            .WithMessage("Geçerli bir web sitesi adresi giriniz.");

        RuleFor(x => x.Email)
            .MaximumLength(255).WithMessage("Email en fazla 255 karakter olabilir.")
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Geçerli bir email adresi giriniz.");

    }
}
