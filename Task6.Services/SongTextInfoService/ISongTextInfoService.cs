using Bogus;
using Task6.Models;

namespace Task6.Services.SongTextInfoService;

public interface ISongTextInfoService
{
    public Song GenerateSongInfo(int index, int seed, Faker faker);
}
