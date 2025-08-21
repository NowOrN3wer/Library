using Library.Application.Common.Interfaces;
using Library.Application.SeedData.PublisherSeed;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Publishers.Commands.Add;

internal sealed class AddPublisherCommandHandler(
    IPublisherRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<AddPublisherCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddPublisherCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Publisher>();
        await repository.AddAsync(entity, cancellationToken);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}