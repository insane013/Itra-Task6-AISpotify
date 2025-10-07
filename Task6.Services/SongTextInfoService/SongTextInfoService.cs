using Bogus;
using Task6.Models;
using Task6.Services.FakerPatterns;

namespace Task6.Services.SongTextInfoService;

public class SongTextInfoService : ISongTextInfoService
{
    Song ISongTextInfoService.GenerateSongInfo(int index, string locale, int seed, Faker faker)
    {
        bool single = faker.Random.Bool();

        return new Song
        {
            index = index,
            Title = SongDataPatterns.FakeTitle(faker),
            Artist = faker.Person.FullName,
            IsSingle = single,
            Album = single ? "Single" : SongDataPatterns.FakeAlbum(faker),
            Genre = faker.Music.Genre(),
            SongLyrics = faker.Hacker.Phrase() + " " + faker.Lorem.Sentence(3),
            Rewards = $"{faker.Company.CompanyName()} {faker.Date.Past().Year}"
        };
    }
}
