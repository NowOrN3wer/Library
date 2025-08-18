using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Commands.Add;

internal sealed class AddBookCommandHandler(
    IBookRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<AddBookCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddBookCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Book>();
        await repository.AddAsync(entity, cancellationToken);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}