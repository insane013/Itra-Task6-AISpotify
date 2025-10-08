using System.Globalization;
using System.Text;
using Bogus;
using Task6.Models;

namespace Task6.Services.Helpers;

public static class SongDataPatterns
{
    public static string FakeTitle(Faker faker)
    {
        var pattern = faker.Random.Int(0, 5);

        string color = faker.Commerce.Color();
        string adjective = faker.Hacker.Adjective();
        string noun = faker.Hacker.Noun();
        string verb = faker.Hacker.Verb();

        string result = pattern switch
        {
            0 => $"{noun}",
            1 => $"{adjective} {noun}",
            2 => $"{noun} {verb}",
            3 => $"{verb} {verb}",
            4 => $"{faker.Name.FirstName()}",
            _ => $"{color} {noun}"
        };

        return Capitilize(result, faker.Locale);
    }

    public static string FakeAlbum(Faker faker)
    {
        var adjective = faker.Commerce.Color();
        var genre = faker.Music.Genre();
        var noun = faker.Hacker.Noun();
        var company = faker.Company.CompanyName();
        var verb = faker.Hacker.Verb();

        int pattern = faker.Random.Int(0, 5);

        string result = pattern switch
        {
            0 => $"{adjective} {genre}",
            1 => $"{company} {verb}",
            2 => $"{verb} {genre}",
            3 => $"{genre} {company}",
            4 => $"{noun} {adjective}",
            _ => $"{noun}"
        };

        return Capitilize(result, faker.Locale);
    }

    private static string Capitilize(string str, string locale)
    {
        TextInfo ti = CultureInfo.GetCultureInfo(locale).TextInfo;
        return ti.ToTitleCase(str);
    }
}
