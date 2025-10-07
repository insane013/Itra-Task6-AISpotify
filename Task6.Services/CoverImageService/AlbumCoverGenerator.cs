using Bogus;
using SkiaSharp;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Task6.Services.CoverImageService;
public static class AlbumCoverGenerator
{
    private static readonly Random Rand = new();
    private static readonly HttpClient Http = new();
    private static readonly string[] Fonts = { "Arial", "Verdana", "Impact", "Roboto", "Comic Sans MS" };

    public static async Task<string> GenerateAsync(string title, string author, bool isSingle, int seed, int size = 256)
    {
        string imageUrl = @$"https://picsum.photos/seed/{seed}/{size}/{size}"; // случайная фоновая картинка

        using var imageStream = await Http.GetStreamAsync(imageUrl);
        using var bitmap = SKBitmap.Decode(imageStream);
        using var surface = SKSurface.Create(new SKImageInfo(size, size));
        var canvas = surface.Canvas;

        // ⚙️ Рисуем фон
        canvas.Clear(SKColors.Black);
        canvas.DrawBitmap(bitmap, new SKRect(0, 0, size, size));

        // ⚙️ Настройки текста
        var paint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            Typeface = SKTypeface.FromFamilyName(RandomFont())
        };

        var titleFont = new SKFont(paint.Typeface, size / 10);
        var authorFont = new SKFont(paint.Typeface, size / 18);

        // Автоматический перенос строк
        title = WrapText(title, titleFont, size * 0.9f);
        author = WrapText(author, authorFont, size * 0.9f);

        // 💬 Выбираем случайную схему расположения текста
        switch (Rand.Next(4))
        {
            case 0: // Название сверху, автор снизу
                DrawText(canvas, title, size / 2, size * 0.25f, titleFont, paint);
                DrawText(canvas, author, size / 2, size * 0.9f, authorFont, paint);
                break;

            case 1: // Название снизу, автор сверху
                DrawText(canvas, author, size / 2, size * 0.2f, authorFont, paint);
                DrawText(canvas, title, size / 2, size * 0.85f, titleFont, paint);
                break;

            case 2: // Вертикально слева
                DrawVerticalText(canvas, title, size * 0.15f, size, titleFont, paint);
                DrawText(canvas, author, size * 0.85f, size * 0.9f, authorFont, paint);
                break;

            case 3: // Вертикально справа
                DrawVerticalText(canvas, title, size * 0.85f, size, titleFont, paint);
                DrawText(canvas, author, size * 0.15f, size * 0.9f, authorFont, paint);
                break;
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        string base64 = Convert.ToBase64String(data.ToArray());
        return $"data:image/png;base64,{base64}";
    }

    private static void DrawText(SKCanvas canvas, string text, float x, float y, SKFont font, SKPaint paint)
    {
        var lines = text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            float offsetY = y + i * (font.Size * 1.2f);
            // добавим тень для читаемости
            paint.Color = SKColors.Black.WithAlpha(150);
            canvas.DrawText(lines[i], x + 2, offsetY + 2, font, paint);
            paint.Color = SKColors.White;
            canvas.DrawText(lines[i], x, offsetY, font, paint);
        }
    }

    private static void DrawVerticalText(SKCanvas canvas, string text, float x, float size, SKFont font, SKPaint paint)
    {
        canvas.Save();
        canvas.RotateDegrees(-90, x, size / 2);
        DrawText(canvas, text, size / 2, x, font, paint);
        canvas.Restore();
    }

    private static string WrapText(string text, SKFont font, float maxWidth)
    {
        using var paint = new SKPaint { Typeface = font.Typeface, TextSize = font.Size };
        var words = text.Split(' ');
        string currentLine = "";
        string result = "";

        foreach (var word in words)
        {
            var test = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            if (paint.MeasureText(test) > maxWidth)
            {
                result += currentLine + "\n";
                currentLine = word;
            }
            else currentLine = test;
        }

        result += currentLine;
        return result;
    }

    private static string RandomFont() => Fonts[Rand.Next(Fonts.Length)];
}
