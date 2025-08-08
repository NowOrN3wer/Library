using FluentValidation;

namespace Library.Application.Features.Writers.Queries.GetById;

internal sealed class GetByIdWriterQueryRequestValidator : AbstractValidator<GetByIdWriterQuery>
{
    public GetByIdWriterQueryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id alanı boş olamaz.");
    }
}