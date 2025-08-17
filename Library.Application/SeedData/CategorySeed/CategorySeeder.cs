using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Mapster;

namespace Library.Application.SeedData.CategorySeed;

public class CategorySeeder(
    ICategoryRepository repository,
    IUnitOfWorkWithTransaction unitOfWork)
{
    public void SeedData()
    {
        int total = 30;
        int batchSize = 30;
        var faker = CategoryFaker.GetFaker(); // Faker<WriterDto>

        for (var i = 0; i < total / batchSize; i++)
        {
            var batch = Enumerable.Range(0, batchSize)
                .Select(_ =>
                {
                    var dto = faker.Generate(); // WriterDto
                    return dto.Adapt<Category>();
                })
                .Select(cmd => cmd.Adapt<Category>()) // Mapster ile
                .ToList();

            repository.AddRange(batch);
            unitOfWork.SaveChanges();
        }
    }
}