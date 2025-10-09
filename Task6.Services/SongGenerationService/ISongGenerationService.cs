using Task6.Models;

namespace Task6.Services.SongGenerationService;

public interface ISongGenerationService
{
    public Song Generate(string locale, long seed, int index = 1);

    public Song BulkGenerate(string locale, long seed);
}
