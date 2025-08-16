using FluentValidation;

namespace Library.Application.Features.Categories.Commands.Update;

public class UpdateCategoryCommandRequestValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id boş veya geçersiz olamaz.");
        
        RuleFor(x => x.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("İsim alanı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("İsim en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .WithMessage("İsim en fazla 255 karakter olabilir.");
    }
}