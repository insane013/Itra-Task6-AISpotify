using Bogus;
using Task6.Models;

namespace Task6.Services.CoverImageService;

public interface ICoverImageService
{
    public string GenerateCoverImage(Song song, Faker faker, int seed);
}
