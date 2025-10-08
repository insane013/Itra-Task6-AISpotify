using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task6.Models;
using Task6.Services;
using Task6.Services.CoverImageService;
using Task6.Services.SongGenerationService;
using Task6.WebApp.Models;

namespace Task6.WebApp.Controllers;

[Route("Home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISongGenerationService songService;
    private readonly ICoverImageService imageService;

    private readonly int globalSeed = 123123123; // remove

    public HomeController(ILogger<HomeController> logger, ISongGenerationService songService, ICoverImageService imageService)
    {
        _logger = logger;
        this.songService = songService;
        this.imageService = imageService;
    }

    [Route("")]
    public IActionResult Index()
    {
        var songs = new List<Song>();

        for (int i = 0; i < 15; i++)
        {
            songs.Add(this.songService.Generate("en_US", globalSeed, i));
        }

        return View(songs);
    }

    [Route("cover/{locale}/{seed:int}/{index:int}")]
    public async Task<IActionResult> GetCoverImage(int seed, string locale, int index)
    {
        this._logger.LogInformation($"Cover generation for song #{index} with {locale} locale and {seed} seed.");
        var song = songService.Generate(locale, seed, index);

        var imageBytes = await imageService.GenerateCoverImage(song, locale, 256);
        return File(imageBytes, "image/png");
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
