using Bogus;
using Library.Application.Features.Writers.Add;
using System.ComponentModel.DataAnnotations;

namespace Library.Application.SeedData;

public static class WriterFaker
{
    public static Faker<AddWriterCommand> GetFaker()
    {
        return new Faker<AddWriterCommand>()
            .RuleFor(w => w.firstName, f =>
            {
                var name = f.Name.FirstName();
                return string.IsNullOrWhiteSpace(name) ? "John" : name;
            })
            .RuleFor(w => w.lastName, f =>
            {
                var name = f.Name.LastName();
                return string.IsNullOrWhiteSpace(name) ? "Doe" : name;
            })
            .RuleFor(w => w.biography, f =>
            {
                var bio = f.Lorem.Paragraphs(1, 3);
                return string.IsNullOrWhiteSpace(bio) ? "No biography available." : bio;
            })
            .RuleFor(w => w.nationality, f =>
            {
                var nation = f.Address.Country();
                return string.IsNullOrWhiteSpace(nation) ? "Unknown" : nation;
            })
            .RuleFor(w => w.birthDate, f => f.Date.PastOffset(80, DateTimeOffset.UtcNow.AddYears(-20)))
            .RuleFor(w => w.deathDate, (f, w) =>
            {
                // Eğer yaşı 40-80 arasındaysa öldü kabul et, değilse null
                return f.Random.Bool() ? w.birthDate?.AddYears(f.Random.Int(40, 80)) : null;
            })
            .RuleFor(w => w.website, f =>
            {
                var url = f.Internet.UrlWithPath("https");
                return Uri.IsWellFormedUriString(url, UriKind.Absolute) ? url : "https://example.com";
            })
            .RuleFor(w => w.email, f =>
            {
                var email = f.Internet.Email();
                return new EmailAddressAttribute().IsValid(email) ? email : "valid@email.com";
            });
    }
}
