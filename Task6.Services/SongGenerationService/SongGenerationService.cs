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

    Song ISongGenerationService.Generate(string locale, int globalSeed, int index)
    {
        int madSeed = SeedHelper.GetSeed(globalSeed, index);
        Randomizer.Seed = new Random(madSeed);

        var faker = new Faker(locale);

        var song = this.textService.GenerateSongInfo(index, madSeed, faker);

        song.GenData = new GenerationData
        {
            Seed = globalSeed,
            Locale = locale
        };

        return song;
    }

    Song ISongGenerationService.BulkGenerate(string locale, int seed)
    {
        throw new NotImplementedException();
    }
}
