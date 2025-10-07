using Task6.Models;

namespace Task6.Services.SongGenerationService;

public interface ISongGenerationService
{
    public Task<Song> Generate(string locale, int seed, int index = 1);

    public IEnumerable<Song> BulkGenerate(string locale, int seed);
}
