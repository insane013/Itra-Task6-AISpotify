namespace Task6.Models;

public class Song
{
    public int index { get; init; }
    public string Title { get; init; } = "Rule";
    public string Artist { get; init; } = "Ado";
    public string Album { get; init; } = "Ado's Best Adobum";
    public string Genre { get; init; } = "Masterpiece";
    public string SongLyrics { get; init; } = "Oriro, oriro, mushiro okkochiro..";
    public bool IsSingle { get; init; }
    public string Rewards { get; init; } = "The best, 2026";

    public int Likes { get; set; } = 99999999;

    public GenerationData GenData { get; set; } = new GenerationData();
}
