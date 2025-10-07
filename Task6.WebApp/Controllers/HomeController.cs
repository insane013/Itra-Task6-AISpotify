using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task6.Models;
using Task6.Services;
using Task6.Services.SongGenerationService;
using Task6.WebApp.Models;

namespace Task6.WebApp.Controllers;

[Route("Home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISongGenerationService songService;

    public HomeController(ILogger<HomeController> logger, ISongGenerationService songService)
    {
        _logger = logger;
        this.songService = songService;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var songs = new List<Song>();
        for (int i = 0; i < 15; i++)
        {
            songs.Add(await this.songService.Generate("en_US", 123123123, i));
        }

        return View(songs);
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
