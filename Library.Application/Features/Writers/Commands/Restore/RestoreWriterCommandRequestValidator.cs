using FluentValidation;

namespace Library.Application.Features.Writers.Commands.Restore;

internal sealed class RestoreWriterCommandRequestValidator: AbstractValidator<RestoreWriterCommand>
{
    public RestoreWriterCommandRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id alanı boş olamaz.");
    }
}