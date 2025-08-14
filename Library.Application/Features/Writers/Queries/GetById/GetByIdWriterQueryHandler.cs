using Library.Application.Dto.WriterDtos;
using Library.Domain.Repositories;
using Mapster;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetById;

internal sealed class GetByIdWriterQueryHandler(IWriterRepository repository) 
    : IRequestHandler<GetByIdWriterQuery, Result<WriterDto>>
{
    public async Task<Result<WriterDto>> Handle(GetByIdWriterQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetByIdWriterQueryDomainValidator(repository);
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsSuccessful || validationResult.Data is null)
        {
            return Result<WriterDto>.Failure(validationResult.ErrorMessages ?? []);
        }
        
        var writer = validationResult.Data;
        
        return writer.Adapt<WriterDto>();
    }
}