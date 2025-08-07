using Library.Domain.Entities;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Writers.Update;

internal sealed class UpdateWriterCommandDomainValidator(IWriterRepository repository)
{
    public async Task<Result<Writer>> ValidateAsync(UpdateWriterCommand request)
    {
        var errors = new List<string>();
        var writer = await repository.GetByExpressionAsync(x=>x.Id.Equals(request.Id));
        
        if (writer is not null) return Result<Writer>.Succeed(writer);
        errors.Add($"Writer with ID {request.Id} does not exist.");
        return Result<Writer>.Failure(errors);
    }
}