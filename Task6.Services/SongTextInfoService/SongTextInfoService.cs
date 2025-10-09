using Bogus;
using Task6.Models;
using Task6.Services.Helpers;

namespace Task6.Services.SongTextInfoService;

public class SongTextInfoService : ISongTextInfoService
{
    Song ISongTextInfoService.GenerateSongInfo(int index, int seed, Faker faker)
    {
        Randomizer.Seed = new Random(seed);
        bool single = faker.Random.Bool();

        return new Song
        {
            index = index,
            Title = faker.Commerce.ProductName(),
            Artist = faker.Person.FullName,
            IsSingle = single,
            Album = single ? "Single" : SongDataPatterns.FakeAlbum(faker),
            Genre = faker.Music.Genre(),
            SongLyrics = faker.Hacker.Phrase() + " " + faker.Lorem.Sentence(3),
            Rewards = $"{faker.Company.CompanyName()} {faker.Date.Past().Year}"
        };
    }
}
