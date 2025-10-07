using System.Threading.Tasks;
using Bogus;
using Task6.Models;
using Task6.Services.CoverImageService;
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

    async Task<Song> ISongGenerationService.Generate(string locale, int seed, int index)
    {
        int madSeed = index * 1392 + seed;
        Randomizer.Seed = new Random(madSeed);

        var faker = new Faker(locale);

        var song = this.textService.GenerateSongInfo(index, locale, madSeed, faker);

        //song.CoverImage = this.coverImageService.GenerateCoverImage(song, faker, seed);

        song.CoverImage = await AlbumCoverGenerator.GenerateAsync(song.IsSingle ? song.Title : song.Album, song.Artist, song.IsSingle, madSeed);

        //song.CoverImage = faker.Image.PicsumUrl();

        return song;
    }

    IEnumerable<Song> ISongGenerationService.BulkGenerate(string locale, int seed)
    {
        throw new NotImplementedException();
    }
}
