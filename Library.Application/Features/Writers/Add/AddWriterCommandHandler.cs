using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Add;

internal sealed class AddWriterCommandHandler(
    IWriterRepository repository, 
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<AddWriterCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddWriterCommand request, CancellationToken cancellationToken)
    {
        var writer = request.Adapt<Writer>();
        repository.Add(writer);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync();
    }
}
