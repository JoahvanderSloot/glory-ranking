public class Fighter
{
    // Main fighter properties
    public required string Name { get; set; }
    public int Division { get; set; } = 6;
    public int Elo { get; set; } = 1000;
    public int PeakElo { get; set; } = 1000;
    public bool Retired { get; set; } = false;

    // Extra fighter properties
    public int Wins { get; set; } = 0;
    public int Losses { get; set; } = 0;
    public int BiggestEloGain { get; set; } = 0;
    public int BiggestEloLoss { get; set; } = 0;
}

public class FighterData
{
    public string Version = "fighter-data-v1";
    public Fighter[] Fighters { get; set; } = Array.Empty<Fighter>();
}
