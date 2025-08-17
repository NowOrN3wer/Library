using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;

namespace Library.Application.SeedData.WriterSeed;

public class WriterSeeder(
    IWriterRepository repository,
    IUnitOfWorkWithTransaction unitOfWork)
{
    public void SeedData()
    {
        int total = 1_000;
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

            repository.AddRange(batch);
            unitOfWork.SaveChanges();
        }
    }
}