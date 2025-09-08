using Library.Application.Dto.BookDtos;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Queries.GetById;

internal sealed class GetByIdBookQueryHandler(IBookRepository repository)
    : IRequestHandler<GetByIdBookQuery, Result<BookDetailDto>>
{
    public async Task<Result<BookDetailDto>> Handle(GetByIdBookQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetByIdBookQueryDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsSuccessful || validationResult.Data is null)
            return Result<BookDetailDto>.Failure(validationResult.ErrorMessages ?? []);

        var entity = validationResult.Data;

        return entity.Adapt<BookDetailDto>();
    }
}