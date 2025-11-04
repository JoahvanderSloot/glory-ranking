public class Fighter
{
    public required string Name { get; set; }
    public int Division { get; set; } = 6;
    public int Elo { get; set; } = 1000;
    public int PeakElo { get; set; } = 1000;
    public bool Retired { get; set; } = false;
}

public class FighterData
{
    public string Version = "fighter-data-v1";
    public Fighter[] Fighters { get; set; } = Array.Empty<Fighter>();
}
