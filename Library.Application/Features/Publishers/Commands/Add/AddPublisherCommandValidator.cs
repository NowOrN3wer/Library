using FluentValidation;

namespace Library.Application.Features.Publishers.Commands.Add;

internal sealed class AddPublisherCommandValidator : AbstractValidator<AddPublisherCommand>
{
    public AddPublisherCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("İsim alanı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("İsim en fazla 100 karakter olabilir.");        
        
        RuleFor(x => x.Website)
            .MaximumLength(255)
            .WithMessage("Website en fazla 255 karakter olabilir.");        
        
        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address en fazla 255 karakter olabilir.");        
        
        RuleFor(x => x.Country)
            .MaximumLength(100)
            .WithMessage("Ülke en fazla 255 karakter olabilir.");
    }
}