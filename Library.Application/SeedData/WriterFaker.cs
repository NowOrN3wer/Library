using Bogus;
using Library.Application.Features.Writers.Add;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.SeedData;

public static class WriterFaker
{
    public static Faker<AddWriterCommand> GetFaker()
    {
        return new Faker<AddWriterCommand>()
            .RuleFor(w => w.FirstName, f =>
            {
                var name = f.Name.FirstName();
                return string.IsNullOrWhiteSpace(name) ? "John" : Truncate(name, 100);
            })
            .RuleFor(w => w.LastName, f =>
            {
                var name = f.Name.LastName();
                return string.IsNullOrWhiteSpace(name) ? "Doe" : Truncate(name, 100);
            })
            .RuleFor(w => w.Biography, f =>
            {
                var bio = f.Lorem.Paragraphs(1, 2);
                return string.IsNullOrWhiteSpace(bio) ? "No biography available." : Truncate(bio, 500);
            })
            .RuleFor(w => w.Nationality, f =>
            {
                var nation = f.Address.Country();
                return string.IsNullOrWhiteSpace(nation) ? "Unknown" : Truncate(nation, 100);
            })
            .RuleFor(w => w.BirthDate, f =>
            {
                return f.Date.PastOffset(80, DateTimeOffset.UtcNow.AddYears(-20));
            })
            .RuleFor(w => w.DeathDate, (f, w) =>
            {
                return f.Random.Bool() ? w.BirthDate?.AddYears(f.Random.Int(40, 80)) : null;
            })
            .RuleFor(w => w.Website, f =>
            {
                var url = f.Internet.UrlWithPath("https");
                return Uri.IsWellFormedUriString(url, UriKind.Absolute) ? Truncate(url, 255) : "https://example.com";
            })
            .RuleFor(w => w.Email, f =>
            {
                var email = f.Internet.Email();
                return new EmailAddressAttribute().IsValid(email) ? Truncate(email, 255) : "valid@email.com";
            });
    }

    private static string Truncate(string input, int maxLength)
    {
        return input.Length <= maxLength ? input : input[..maxLength];
    }
}
