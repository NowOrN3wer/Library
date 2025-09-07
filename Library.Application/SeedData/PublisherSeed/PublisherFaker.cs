using Bogus;
using Library.Application.Features.Publishers.Commands.Add;

namespace Library.Application.SeedData.PublisherSeed;

public static class PublisherFaker
{
    // Örnek yayınevleri listesi
    private static readonly IReadOnlyList<(string Name, string Country)> Publishers = new[]
    {
        ("Penguin Random House", "USA"),
        ("HarperCollins", "USA"),
        ("Macmillan Publishers", "UK"),
        ("Simon & Schuster", "USA"),
        ("Hachette Livre", "France"),
        ("Oxford University Press", "UK"),
        ("Cambridge University Press", "UK"),
        ("Kodansha", "Japan"),
        ("Shueisha", "Japan"),
        ("Can Yayınları", "Turkey"),
        ("İş Bankası Kültür Yayınları", "Turkey"),
        ("Everest Yayınları", "Turkey")
    };

    public static Faker<AddPublisherCommand> GetFaker()
    {
        return new Faker<AddPublisherCommand>()
            .RuleFor(w => w.Name, (f, u) =>
            {
                var tuple = Publishers[f.IndexFaker % Publishers.Count];
                return Truncate(tuple.Name, 255);
            })
            .RuleFor(w => w.Country, (f, u) =>
            {
                var tuple = Publishers[f.IndexFaker % Publishers.Count];
                return Truncate(tuple.Country, 100);
            })
            .RuleFor(w => w.Website, (f, u) =>
            {
                var url = f.Internet.UrlWithPath("https");
                return Truncate(url, 255);
            })
            .RuleFor(w => w.Address, (f, u) =>
            {
                var addr = f.Address.FullAddress();
                return Truncate(addr, 500);
            });
    }

    public static List<AddPublisherCommand> Generate(int count = 50)
    {
        return GetFaker().Generate(count);
    }

    private static string Truncate(string value, int maxLength)
    {
        return string.IsNullOrEmpty(value) || value.Length <= maxLength
            ? value
            : value[..maxLength];
    }
}