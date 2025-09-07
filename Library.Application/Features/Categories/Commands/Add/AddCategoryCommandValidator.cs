using FluentValidation;

namespace Library.Application.Features.Categories.Commands.Add;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("İsim alanı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("İsim en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .WithMessage("Açıklama en fazla 255 karakter olabilir.");
    }
}