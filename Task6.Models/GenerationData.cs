namespace Task6.Models;

public class GenerationData
{
    public long Seed { get; init; }
    public string Locale { get; init; } = string.Empty;
    public float Likes { get; set; }
}
