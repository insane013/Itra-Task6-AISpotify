using System.Numerics;
using SkiaSharp;

namespace Task6.Services.CoverImageService;

public interface IDrawToolkit
{
    public void DrawText(string text, Vector2 coordinate, float maxWidth,
                         SKFont font, SKPaint textPaint, SKPaint strokePaint, SKCanvas canvas);
}
