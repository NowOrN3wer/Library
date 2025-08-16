namespace Library.Application.SeedData.CategorySeed;

using Bogus;
using Library.Application.Features.Categories.Commands.Add;

public static class CategoryFaker
{
    // İstersen buraya yeni kategoriler ekleyebilirsin
    private static readonly IReadOnlyList<(string Name, string ExampleSubgenres)> BookCategories = new[]
    {
        ("Fiction", "Literary, Contemporary, Historical"),
        ("Non-Fiction", "Biography, Memoir, Essay"),
        ("Science Fiction", "Cyberpunk, Space Opera, Dystopia"),
        ("Fantasy", "Epic, Urban, Dark"),
        ("Mystery", "Detective, Cozy, Noir"),
        ("Thriller", "Psychological, Techno, Crime"),
        ("Horror", "Supernatural, Gothic, Folk"),
        ("Romance", "Contemporary, Historical, Paranormal"),
        ("Historical", "WWII, Victorian, Ancient"),
        ("Young Adult", "Coming-of-Age, Fantasy, Romance"),
        ("Children's", "Picture Books, Early Readers, Middle Grade"),
        ("Poetry", "Free Verse, Narrative, Haiku"),
        ("Drama/Plays", "Tragedy, Comedy, Historical"),
        ("Graphic Novels", "Superhero, Slice of Life, Manga"),
        ("Philosophy", "Ethics, Metaphysics, Logic"),
        ("Self-Help", "Productivity, Mindfulness, Habits"),
        ("Business & Economics", "Startup, Finance, Management"),
        ("Science", "Physics, Biology, Astronomy"),
        ("Technology", "Programming, AI, Security"),
        ("History", "World, Military, Cultural"),
        ("Travel", "Guide, Memoir, Adventure"),
        ("Art & Design", "Photography, Architecture, Painting"),
        ("Cooking", "Baking, Vegan, Regional"),
        ("Health & Fitness", "Nutrition, Training, Longevity"),
        ("Religion & Spirituality", "Theology, Comparative, Meditation"),
        ("Education & Reference", "Study Guides, Language, Encyclopedias"),
        ("True Crime", "Investigations, Courtroom, Forensics"),
        ("Short Stories", "Anthology, Literary, Speculative"),
        ("Essays", "Cultural Critique, Personal, Political"),
        ("Humor", "Satire, Essays, Cartoons")
    };

    public static Faker<AddCategoryCommand> GetFaker()
    {
        return new Faker<AddCategoryCommand>()
            // İstiyorsan tekrarsız üretmek için PickRandomWithout kullanabilirsin (Bogus 35+).
            // Aşağıda sıralı ve döngüsel veriyorum ki 'count' > kategori sayısı olsa da çalışsın.
            .RuleFor(w => w.Name, (f, u) =>
            {
                var tuple = BookCategories[f.IndexFaker % BookCategories.Count];
                return Truncate(tuple.Name, 100);
            })
            .RuleFor(w => w.Description, (f, u) =>
            {
                var tuple = BookCategories[f.IndexFaker % BookCategories.Count];
                // Kısa, anlamlı açıklama + örnek alt türler
                var sentence = f.Lorem.Sentence(10, 3);
                var desc = $"{tuple.Name} books. Subgenres: {tuple.ExampleSubgenres}. {sentence}";
                return Truncate(desc, 300);
            });
    }

    public static List<AddCategoryCommand> Generate(int count = 50)
        => GetFaker().Generate(count);

    private static string Truncate(string value, int maxLength)
        => string.IsNullOrEmpty(value) || value.Length <= maxLength
            ? value
            : value[..maxLength];
}