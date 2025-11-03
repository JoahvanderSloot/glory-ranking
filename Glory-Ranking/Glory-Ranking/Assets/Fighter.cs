public class Fighter
{
    public required string Name { get; set; }
    public int Division { get; set; } = 5;
    public int Elo { get; set; } = 1000;
    public int PeakElo { get; set; } = 1000;

}

public class FighterData
{
    public string Version = "fighter-data-v1";

    public Fighter[] Fighters;
}