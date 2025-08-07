using Library.Application.Common.Interfaces;
using Library.Domain.Repositories;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Update;

internal sealed class UpdateWriterCommandHandler(
    IWriterRepository repository,
    IUnitOfWorkWithTransaction unitOfWork) : IRequestHandler<UpdateWriterCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateWriterCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateWriterCommandDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsSuccessful || validationResult.Data is not { } write)
        {
            return Result<bool>.Failure(validationResult.ErrorMessages ?? []);
        }
        
        var writer = validationResult.Data;

        if (!string.IsNullOrWhiteSpace(request.FirstName) && request.FirstName != writer.FirstName)
            writer.FirstName = request.FirstName;

        if (!string.IsNullOrWhiteSpace(request.LastName) && request.LastName != writer.LastName)
            writer.LastName = request.LastName;

        if (!string.IsNullOrWhiteSpace(request.Biography) && request.Biography != writer.Biography)
            writer.Biography = request.Biography;

        if (!string.IsNullOrWhiteSpace(request.Nationality) && request.Nationality != writer.Nationality)
            writer.Nationality = request.Nationality;

        if (request.BirthDate.HasValue && writer!.BirthDate != request.BirthDate)
            writer.BirthDate = request.BirthDate;

        if (request.DeathDate.HasValue && writer!.DeathDate != request.DeathDate)
            writer.DeathDate = request.DeathDate;

        if (!string.IsNullOrWhiteSpace(request.Website) && request.Website != writer.Website)
            writer.Website = request.Website;

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != writer.Email)
            writer.Email = request.Email;
        
        repository.Update(writer);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}