using Library.Application.Common.Interfaces;
//using Library.Application.SeedData;
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
      /*  int total = 1_000_00;
        int batchSize = 1000;
        var faker = WriterFaker.GetFaker(); // Faker<WriterDto>

        for (var i = 0; i < total / batchSize; i++)
        {
            var batch = Enumerable.Range(0, batchSize)
                .Select(_ =>
                {
                    var dto = faker.Generate(); // WriterDto
                    return dto.Adapt<Writer>();
                })
                .Select(cmd => cmd.Adapt<Writer>()) // Mapster ile
                .ToList();

            await repository.AddRangeAsync(batch);
            await unitOfWork.SaveChangesAsync();

            Console.WriteLine($"Inserted {batchSize} writers");
        }*/

        var writer = request.Adapt<Writer>();
        await repository.AddAsync(writer, cancellationToken);
        return await unitOfWork.SaveChangesAndReturnSuccessAsync(cancellationToken);
    }
}
