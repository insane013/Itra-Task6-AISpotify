using System.Threading.Tasks;
using Bogus;
using Task6.Models;
using Task6.Services.CoverImageService;
using Task6.Services.Helpers;
using Task6.Services.SongTextInfoService;

namespace Task6.Services.SongGenerationService;

public class SongGenerationService : ISongGenerationService
{
    private readonly ISongTextInfoService textService;
    private readonly ICoverImageService coverImageService;

    public SongGenerationService(ISongTextInfoService textService, ICoverImageService coverImageService)
    {
        this.textService = textService;
        this.coverImageService = coverImageService;
    }

    Song ISongGenerationService.Generate(string locale, long globalSeed, int index, float likesCoef)
    {
        int madSeed = SeedHelper.GetSeed(globalSeed, index);
        Randomizer.Seed = new Random(madSeed);

        var faker = new Faker(locale);

        var song = this.textService.GenerateSongInfo(index, madSeed, faker);

        song.Likes = this.GetLikes(likesCoef, faker);
        song.GenData = new GenerationData
        {
            Seed = globalSeed,
            Locale = locale,
            Likes = likesCoef
        };

        return song;
    }

    private int GetLikes(float likesCoef, Faker faker)
    {
        int likes = (int)Math.Floor((double)likesCoef);
        float fraction = likesCoef - likes;

        return faker.Random.Number(1, 100) < fraction * 100 ? ++likes : likes;
    }

    Song ISongGenerationService.BulkGenerate(string locale, long seed)
    {
        throw new NotImplementedException();
    }
}
