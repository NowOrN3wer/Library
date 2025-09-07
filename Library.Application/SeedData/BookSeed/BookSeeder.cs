using Bogus;
using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.SeedData.BookSeed;

public static class BookSeeder
{
    public static async Task SeedAsync(
        IWriterRepository writerRepository,
        ICategoryRepository categoryRepository,
        IPublisherRepository publisherRepository,
        IBookRepository bookRepository,
        IUnitOfWorkWithTransaction unitOfWork,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // DB'den gerekli listeler
            var writers = await writerRepository.GetAll()
                .AsNoTracking()
                .Select(w => w.Id)
                .ToListAsync(cancellationToken);

            var categories = await categoryRepository.GetAll()
                .AsNoTracking()
                .Select(c => c.Id)
                .ToListAsync(cancellationToken);

            var publishers = await publisherRepository.GetAll()
                .AsNoTracking()
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            if (writers.Count == 0 || categories.Count == 0 || publishers.Count == 0)
                return;

            var rnd = new Random();
            var isbnSet = new HashSet<string>();
            var languages = new[]
                { "English", "Türkçe", "Deutsch", "Français", "Español", "Italiano", "Русский", "العربية", "日本語" };

            // Book faker
            var bookFaker = new Faker<Book>("tr")
                .RuleFor(b => b.Id, _ => Guid.CreateVersion7())
                .RuleFor(b => b.Title, f =>
                {
                    var title = f.Lorem.Sentence(f.Random.Int(2, 6)).TrimEnd('.');
                    return title.Length <= 255 ? title : title[..255];
                })
                .RuleFor(b => b.Summary, f =>
                {
                    if (!f.Random.Bool(0.6f)) return null;
                    var text = f.Lorem.Paragraphs(1);
                    return text.Length <= 1000 ? text : text.Substring(0, 1000);
                })
                .RuleFor(b => b.ISBN, f =>
                {
                    if (!f.Random.Bool(0.5f)) return null;
                    string candidate;
                    var tries = 0;
                    do
                    {
                        candidate = f.Random.ReplaceNumbers("#############"); // 13 hane
                        tries++;
                    } while (isbnSet.Contains(candidate) && tries < 5);

                    isbnSet.Add(candidate);
                    return candidate;
                })
                .RuleFor(b => b.Language, f => f.Random.Bool(0.7f) ? f.PickRandom(languages) : null)
                .RuleFor(b => b.PageCount, f => f.Random.Bool(0.8f) ? f.Random.Int(80, 900) : null)
                .RuleFor(b => b.PublishedDate, f =>
                {
                    if (!f.Random.Bool(0.75f)) return null;
                    // PastOffset yerel offset üretir -> UTC'ye çevir:
                    var dt = f.Date.PastOffset(30).ToUniversalTime(); // Offset = 00:00
                    return dt;
                });

            var buffer = new List<Book>(512);

            // Tüm işlemi transaction içinde çalıştır
            await unitOfWork.ExecuteInTransactionAsync(async ct =>
            {
                foreach (var writerId in writers)
                {
                    var count = rnd.Next(5, 16); // 5–15

                    for (var i = 0; i < count; i++)
                    {
                        var categoryId = categories[rnd.Next(categories.Count)];
                        var publisherId = publishers[rnd.Next(publishers.Count)];

                        var book = bookFaker.Generate();
                        book.WriterId = writerId;
                        book.CategoryId = categoryId;
                        book.PublisherId = publisherId;

                        // Navigation property'lere dokunmuyoruz (Entity'de default! var)
                        buffer.Add(book);

                        if (buffer.Count >= 500)
                        {
                            await bookRepository.AddRangeAsync(buffer, ct);
                            await unitOfWork.SaveChangesAsync(ct);
                            buffer.Clear();
                        }
                    }
                }

                if (buffer.Count > 0)
                {
                    await bookRepository.AddRangeAsync(buffer, ct);
                    await unitOfWork.SaveChangesAsync(ct);
                    buffer.Clear();
                }

                // ExecuteInTransactionAsync => commit eder
                return true;
            }, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}