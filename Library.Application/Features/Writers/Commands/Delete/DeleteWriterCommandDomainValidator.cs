using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Delete;

internal sealed class DeleteWriterCommandDomainValidator(IWriterRepository repository)
    : BaseActiveEntityByIdValidator<Writer, DeleteWriterCommand>(repository)
{
    public async Task<Result<Writer>> ValidateAsync(DeleteWriterCommand request)
    {
        return await ValidateAsync(request, request.Id);
    }
}