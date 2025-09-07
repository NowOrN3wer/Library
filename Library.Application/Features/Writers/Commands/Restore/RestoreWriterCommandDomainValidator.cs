using Library.Application.Common.Validators;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Repositories;
using TS.Result;

namespace Library.Application.Features.Writers.Commands.Restore;

internal sealed class RestoreWriterCommandDomainValidator(IWriterRepository repository)
    : BaseEntityStateValidator<Writer>(repository) // <- burada parametre *türetilmiş sınıfın* ctor’una gider
{
    public Task<Result<Writer>> ValidateAsync(RestoreWriterCommand request, CancellationToken ct = default)
    {
        return EnsureStateAsync(request.Id, EntityStatus.DELETED,
            "Writer is already active and cannot be restored.", ct);
    }
}