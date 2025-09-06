using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace Library.Application.Features.Books.Queries.GetById;

internal sealed class GetByIdBookQueryDomainValidator(IBookRepository repository)
    : BaseActiveEntityByIdValidator<Book, GetByIdBookQuery>(repository)
{
    public async Task<Result<Book>> ValidateAsync(GetByIdBookQuery request, CancellationToken ct = default)
    {
        var entity = await repository
            .AsQueryable()
            .Where(x => x.Id == request.Id && x.IsDeleted == EntityStatus.ACTIVE)
            .Include(b => b.Writer)
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(ct);

        return entity is not null
            ? Result<Book>.Succeed(entity)
            : Result<Book>.Failure([$"Book with ID {request.Id} does not exist or is deleted."]);
    }
}
