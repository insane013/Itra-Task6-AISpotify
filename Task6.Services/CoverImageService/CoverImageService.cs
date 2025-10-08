using System.Numerics;
using System.Threading.Tasks;
using Bogus;
using SkiaSharp;
using Task6.Models;
using Task6.Services.Helpers;


namespace Task6.Services.CoverImageService;

public class CoverImageService : ICoverImageService
{
    private const float FontSizeCoef = 0.1f;
    private const float MiddleCoef = 0.5f;
    private const float AvailableSizePercent = 0.9f;
    private const SKTextAlign DefaultTextAlign = SKTextAlign.Center;

    private readonly string[] Fonts = { "Arial", "Verdana", "Impact", "Roboto", "Comic Sans MS" };
    private readonly IDrawToolkit drawToolkit;

    public CoverImageService(IDrawToolkit drawToolkit)
    {
        this.drawToolkit = drawToolkit;
    }

    public async Task<byte[]> GenerateCoverImage(Song song, string locale, int size)
    {
        var madIndex = SeedHelper.GetSeed(song.GenData.Seed, song.index);
        Randomizer.Seed = new Random(madIndex);
        var faker = new Faker(song.GenData.Locale);

        var background = await this.GetRandomBackground(madIndex, size);

        return this.DrawCover(background, faker, song.Title, song.Artist);
    }

    private async Task<byte[]> GetRandomBackground(int seed, int size)
    {
        string imageUrl = @$"https://picsum.photos/seed/{seed}/{size}/{size}";

        using var http = new HttpClient();
        using var stream = await http.GetStreamAsync(imageUrl);

        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        return ms.ToArray();
    }

    private byte[] DrawCover(byte[] background, Faker faker, string title, string author)
    {
        using var bitmap = SKBitmap.Decode(background);

        int size = bitmap.Width;

        using var surface = SKSurface.Create(new SKImageInfo(size, size));
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.Black);
        canvas.DrawBitmap(bitmap, new SKRect(0, 0, size, size));

        var coordinate = this.GetTextCoordinations(faker, size);
        var font = this.GetRandomFont(faker, size);

        var textPaint = this.GetRandomPaint(faker);
        var strokePaint = this.GetStrokePaint(faker);

        this.drawToolkit.DrawText(title, coordinate.Item1, AvailableSizePercent, font, textPaint, strokePaint, canvas);
        this.drawToolkit.DrawText(author, coordinate.Item2, AvailableSizePercent, font, textPaint, strokePaint, canvas);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return data.ToArray();
    }

    private SKFont GetRandomFont(Faker faker, float canvasWidth)
    {
        return new SKFont
        {
            Typeface = SKTypeface.FromFamilyName(this.Fonts[faker.Random.Number(0, this.Fonts.Count() - 1)], SKFontStyle.Bold),
            Size = canvasWidth * FontSizeCoef,
        };
    }

    private SKPaint GetRandomPaint(Faker faker)
    {
        return new SKPaint
        {
            Color = new SKColor(faker.Random.Byte(), faker.Random.Byte(), faker.Random.Byte()),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = faker.Random.Number(3, 6)
        };
    }

    private SKPaint GetStrokePaint(Faker faker)
    {
        return new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = faker.Random.Number(0, 2),
        };
    }

    private (Vector2, Vector2) GetTextCoordinations(Faker faker, float size)
    {
        return faker.Random.Number(0, 1) switch
        {
            0 => (new Vector2(size * MiddleCoef, size * FontSizeCoef),
                  new Vector2(size * MiddleCoef, size * (1 - FontSizeCoef * 3))),
            _ => (new Vector2(size * MiddleCoef, size * (1 - FontSizeCoef * 3)),
                  new Vector2(size * MiddleCoef, size * FontSizeCoef))
        };
    }
}
