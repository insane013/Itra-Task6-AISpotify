using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Task6.Models;
using Task6.Services.CoverImageService;
using Task6.Services.SongGenerationService;
using Task6.WebApp.Helpers;
using Task6.WebApp.Models;

namespace Task6.WebApp.Controllers;

[Route("Home")]
public class HomeController : Controller
{
    private const string DefaultSupportedLocale = "en_US";
    private const long DefaultSeed = 123;
    private readonly ILogger<HomeController> _logger;
    private readonly ISongGenerationService songService;
    private readonly ICoverImageService imageService;
    private readonly IConfiguration config;
    private string[] SupportedLocales { get; init; }

    public HomeController(ILogger<HomeController> logger, ISongGenerationService songService, ICoverImageService imageService, IConfiguration config)
    {
        _logger = logger;
        this.songService = songService;
        this.imageService = imageService;
        this.config = config;

        var localesString = config["BogusLocales"];
        this.SupportedLocales = localesString?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? new[] { DefaultSupportedLocale };
    }

    [Route("")]
    [HttpGet]
    public IActionResult Index([FromQuery] long seed, string locale, float likes)
    {
        return View(this.GenerateModel(seed, locale, likes));
    }

    [Route("Regenerate")]
    [HttpGet]
    public IActionResult SongsPartial([FromQuery] string locale, long seed, float likes)
    {
        var models = this.GenerateModel(seed, locale, likes);
        return PartialView("_SongListPartial", models.songs);
    }

    [Route("Locales")]
    [HttpGet]
    public IActionResult GetLocales()
    {
        var result = this.SupportedLocales
            .OrderBy(l => l)
            .Select(code => new
            {
                code
            });

        return Json(result);
    }

    [Route("cover/{locale}/{seed:long}/{index:int}")]
    public async Task<IActionResult> GetCoverImage(long seed, string locale, int index)
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

    private string GetLocale(string locale)
    {
        if (!string.IsNullOrWhiteSpace(locale) && this.SupportedLocales.Contains(locale))
        {
            return locale;
        }
        else
        {
            var requestCulture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
            return requestCulture?.UICulture?.Name.Replace('-', '_') ?? DefaultSupportedLocale;
        }
    }

    private (IEnumerable<Song> songs, GenerationData data) GenerateModel(long seed, string locale, float likes)
    {
        locale = this.GetLocale(locale);

        var songs = new List<Song>();
        for (int i = 0; i < 15; i++)
        {
            songs.Add(this.songService.Generate(locale, seed, i, likes));
        }

        return (songs, new GenerationData { Locale = locale, Seed = seed, Likes = likes });
    }
}
