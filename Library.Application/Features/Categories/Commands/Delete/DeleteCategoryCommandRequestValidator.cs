using FluentValidation;

namespace Library.Application.Features.Categories.Commands.Delete;

internal sealed class DeleteCategoryCommandRequestValidator : AbstractValidator<DeleteCategoryCommand>
{
   public DeleteCategoryCommandRequestValidator()
   {
      RuleFor(x => x.Id)
         .NotEmpty().WithMessage("Id alanı boş olamaz.");
   } 
}