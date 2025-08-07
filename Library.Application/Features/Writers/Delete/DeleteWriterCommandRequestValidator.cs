using FluentValidation;

namespace Library.Application.Features.Writers.Delete;

public class DeleteWriterCommandRequestValidator: AbstractValidator<DeleteWriterCommand>
{
    public DeleteWriterCommandRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id alanı boş olamaz.");
    }
}