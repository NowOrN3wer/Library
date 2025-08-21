using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;

namespace Library.Application.SeedData.PublisherSeed;

public class PublisherSeeder(
    IPublisherRepository repository,
    IUnitOfWorkWithTransaction unitOfWork)
{
    public void SeedData()
    {
        int total = 30;
        int batchSize = 30;
        var faker = PublisherFaker.GetFaker();

        for (var i = 0; i < total / batchSize; i++)
        {
            var batch = Enumerable.Range(0, batchSize)
                .Select(_ =>
                {
                    var dto = faker.Generate(); 
                    return dto.Adapt<Publisher>();
                })
                .ToList();

            repository.AddRange(batch);
            unitOfWork.SaveChanges();
        }
    }
}