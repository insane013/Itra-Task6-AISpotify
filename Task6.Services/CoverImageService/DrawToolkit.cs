using System.Numerics;
using SkiaSharp;

namespace Task6.Services.CoverImageService;

public class DrawToolkit : IDrawToolkit
{
    public const float LineSpaceCoef = 1.2f;
    public void DrawText(string text, Vector2 coordinate, float maxWidth,
                         SKFont font, SKPaint paint, SKPaint strokePaint, SKCanvas canvas)
    {
        text = this.WrapText(text, font, maxWidth);
        var lines = text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            float offsetY = coordinate.Y + i * (font.Size * LineSpaceCoef);

            this.WriteLine(lines[i], new Vector2(coordinate.X, offsetY),
                           font, paint, strokePaint, canvas);
        }
    }

    private void WriteLine(string line, Vector2 coordinate,
                         SKFont font, SKPaint paint, SKPaint strokePaint, SKCanvas canvas)
    {
        canvas.DrawText(line, coordinate.X, coordinate.Y, SKTextAlign.Center, font, paint);
        canvas.DrawText(line, coordinate.X, coordinate.Y, SKTextAlign.Center, font, strokePaint);
    }

    private string WrapText(string text, SKFont font, float maxWidth)
    {
        var words = text.Split(' ');

        string currentLine = "";
        string result = "";

        foreach (var word in words)
        {
            var test = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            if (font.MeasureText(test) > maxWidth)
            {
                result += currentLine + "\n";
                currentLine = word;
            }
            else currentLine = test;
        }

        result += currentLine;

        return result;
    }
}
