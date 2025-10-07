using Bogus;
using SkiaSharp;
using System.IO;
using Task6.Models;

namespace Task6.Services.CoverImageService;

public class CoverImageService : ICoverImageService
{
    private int size = 800;
    private readonly string[] Fonts = {
        "Arial", "Roboto", "Verdana", "Impact", "Comic Sans MS"
    };

    private readonly SKColor[] Palette = new[]
    {
        new SKColor(255, 99, 71),   // tomato
        new SKColor(135, 206, 250), // skyblue
        new SKColor(60, 179, 113),  // mediumseagreen
        new SKColor(238, 130, 238), // violet
        new SKColor(255, 215, 0)    // gold
    };

    private readonly Random Rand = new();
    string ICoverImageService.GenerateCoverImage(Song song, Faker faker, int seed)
    {
        using var surface = SKSurface.Create(new SKImageInfo(size, size));
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.Black);

        // 🎨 Генерация фона
        DrawBackground(canvas, size);

        // 📜 Выбор варианта компоновки
        int layoutType = Rand.Next(4);
        var fontTitle = new SKFont(SKTypeface.FromFamilyName(RandomFont()), size / 10);
        var fontAuthor = new SKFont(SKTypeface.FromFamilyName(RandomFont()), size / 16);

        var paint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };

        // 🔠 Перенос строк при необходимости
        string title = WrapText(song.Title, fontTitle, size * 0.9f);
        string author = WrapText(song.Artist, fontAuthor, size * 0.9f);

        // 🧱 Отрисовка текста
        switch (layoutType)
        {
            case 0:
                DrawTextCentered(canvas, title, size / 2, size / 3, fontTitle, paint);
                DrawTextCentered(canvas, author, size / 2, size * 0.8f, fontAuthor, paint);
                break;

            case 1:
                DrawTextCentered(canvas, author, size / 2, size / 3, fontAuthor, paint);
                DrawTextCentered(canvas, title, size / 2, size * 0.8f, fontTitle, paint);
                break;

            case 2:
                DrawVerticalText(canvas, title, size * 0.15f, fontTitle, paint, size);
                DrawTextCentered(canvas, author, size * 0.85f, size * 0.9f, fontAuthor, paint, rotate: false);
                break;

            case 3:
                DrawVerticalText(canvas, title, size * 0.85f, fontTitle, paint, size);
                DrawTextCentered(canvas, author, size * 0.15f, size * 0.9f, fontAuthor, paint, rotate: false);
                break;
        }

        // 📦 Экспорт
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        string base64 = Convert.ToBase64String(data.ToArray());
        return $"data:image/png;base64,{base64}";
    }
    
    private void DrawBackground(SKCanvas canvas, int size)
    {
        // Простой, но живой фон: цветовые пятна + плавный градиент
        var shader = SKShader.CreateLinearGradient(
            new SKPoint(0, 0),
            new SKPoint(size, size),
            new[] { RandomColor(), RandomColor(), RandomColor() },
            null,
            SKShaderTileMode.Clamp
        );

        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(new SKRect(0, 0, size, size), paint);

        // Добавим немного шума
        using var noisePaint = new SKPaint();
        for (int i = 0; i < 1000; i++)
        {
            noisePaint.Color = RandomColor().WithAlpha((byte)Rand.Next(10, 40));
            float x = Rand.Next(size);
            float y = Rand.Next(size);
            canvas.DrawCircle(x, y, Rand.Next(1, 6), noisePaint);
        }
    }

    private void DrawTextCentered(SKCanvas canvas, string text, float x, float y, SKFont font, SKPaint paint, bool rotate = false)
    {
        if (rotate)
        {
            canvas.Save();
            canvas.RotateDegrees(-90, x, y);
        }

        var lines = text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            float offsetY = y + i * (font.Size * 1.2f);
            canvas.DrawText(lines[i], x, offsetY, font, paint);
        }

        if (rotate)
            canvas.Restore();
    }

    private void DrawVerticalText(SKCanvas canvas, string text, float x, SKFont font, SKPaint paint, int size)
    {
        canvas.Save();
        canvas.RotateDegrees(-90, x, size / 2);
        DrawTextCentered(canvas, text, size / 2, x, font, paint);
        canvas.Restore();
    }

    private string WrapText(string text, SKFont font, float maxWidth)
    {
        using var paint = new SKPaint { Typeface = font.Typeface, TextSize = font.Size };
        var words = text.Split(' ');
        string currentLine = "";
        string result = "";

        foreach (var word in words)
        {
            var testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            if (paint.MeasureText(testLine) > maxWidth)
            {
                result += currentLine + "\n";
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }

        result += currentLine;
        return result;
    }

    private SKColor RandomColor() => Palette[Rand.Next(Palette.Length)];
    private string RandomFont() => Fonts[Rand.Next(Fonts.Length)];
}
