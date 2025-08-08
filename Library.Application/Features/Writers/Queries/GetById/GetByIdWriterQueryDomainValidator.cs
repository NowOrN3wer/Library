using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetById;

internal sealed class GetByIdWriterQueryDomainValidator(IWriterRepository repository)
    : BaseActiveEntityByIdValidator<Writer, GetByIdWriterQuery>(repository)
{
    public async Task<Result<Writer>> ValidateAsync(GetByIdWriterQuery request)
    {
        return await ValidateAsync(request, request.Id);
    }
}