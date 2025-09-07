using FluentValidation;

namespace Library.Application.Features.Books.Commands.Add;

public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
{
    public AddBookCommandValidator()
    {
        // Zorunlu alanlar
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık (Title) alanı boş olamaz.")
            .MaximumLength(255).WithMessage("Başlık (Title) en fazla 255 karakter olabilir.");

        // Opsiyonel alanlar + uzunluk kısıtları
        RuleFor(x => x.Summary)
            .MaximumLength(1000).WithMessage("Özet (Summary) en fazla 1000 karakter olabilir.");

        RuleFor(x => x.ISBN)
            .MaximumLength(13).WithMessage("ISBN en fazla 13 karakter olabilir.")
            // Boş değilse temel pattern kontrolü (rakam veya tire)
            .Matches(@"^[0-9\-]*$").When(x => !string.IsNullOrWhiteSpace(x.ISBN))
            .WithMessage("ISBN yalnızca rakam ve '-' içerebilir.");

        RuleFor(x => x.Language)
            .MaximumLength(100).WithMessage("Dil (Language) en fazla 100 karakter olabilir.");

        // Sayısal/Tarihsel kontroller
        RuleFor(x => x.PageCount)
            .GreaterThan(0).When(x => x.PageCount.HasValue)
            .WithMessage("Sayfa sayısı (PageCount) pozitif olmalıdır.");

        RuleFor(x => x.PublishedDate)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).When(x => x.PublishedDate.HasValue)
            .WithMessage("Yayın tarihi (PublishedDate) gelecekte olamaz.");

        // İlişkiler (FK zorunlu)
        RuleFor(x => x.WriterId)
            .NotEmpty().WithMessage("WriterId alanı zorunludur.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId alanı zorunludur.");

        RuleFor(x => x.PublisherId)
            .NotEmpty().WithMessage("PublisherId alanı zorunludur.");
    }
}