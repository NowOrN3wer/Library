using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Update;

internal sealed class UpdateWriterCommandDomainValidator(IWriterRepository repository)
    : BaseActiveEntityByIdValidator<Writer, UpdateWriterCommand>(repository)
{
    public async Task<Result<Writer>> ValidateAsync(UpdateWriterCommand request)
    {
        return await ValidateAsync(request, request.Id);
    }
}