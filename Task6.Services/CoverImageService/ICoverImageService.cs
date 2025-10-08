using Bogus;
using Task6.Models;

namespace Task6.Services.CoverImageService;

public interface ICoverImageService
{
    public Task<byte[]> GenerateCoverImage(Song song, string locale, int size);
}
