using FluentValidation;

namespace Library.Application.Features.Books.Queries.GetById;

internal sealed class GetByIdBookQueryRequestValidator : AbstractValidator<GetByIdBookQuery>
{
    public GetByIdBookQueryRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id alanı boş olamaz.");
    }
}