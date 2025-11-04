using System;
using System.Collections.Generic;
using System.Linq;

namespace Glory_Ranking
{
    public static class FighterManager
    {
        public static List<Fighter> Fighters { get; private set; } = new List<Fighter>();

        public static void LoadFighters()
        {
            Fighters = FighterStorage.Load().Fighters.ToList();
        }

        public static void SaveFighters()
        {
            FighterStorage.Save(new FighterData { Fighters = Fighters.ToArray() });
        }

        public static void AddFighter(string name, int division)
        {
            if (Fighters.Any(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase))) return;

            Fighters.Add(new Fighter
            {
                Name = name,
                Division = division,
                Elo = 1000,
                PeakElo = 1000
            });

            SaveFighters();
        }

        public static Fighter? GetFighter(string name)
        {
            return Fighters.FirstOrDefault(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static void UpdateFighter(Fighter fighter)
        {
            fighter.PeakElo = Math.Max(fighter.PeakElo, fighter.Elo);
            SaveFighters();
        }

        public static void RecordFight(string winnerName, string loserName)
        {
            var winner = GetFighter(winnerName);
            var loser = GetFighter(loserName);

            if (winner == null || loser == null || winner == loser) return;

            const int k = 32;
            double expectedWinner = 1.0 / (1 + Math.Pow(10, (loser.Elo - winner.Elo) / 400.0));
            double expectedLoser = 1.0 / (1 + Math.Pow(10, (winner.Elo - loser.Elo) / 400.0));

            winner.Elo = (int)(winner.Elo + k * (1 - expectedWinner));
            loser.Elo = (int)(loser.Elo + k * (0 - expectedLoser));

            winner.PeakElo = Math.Max(winner.PeakElo, winner.Elo);
            loser.PeakElo = Math.Max(loser.PeakElo, loser.Elo);

            SaveFighters();
        }

        public static List<Fighter> GetLeaderboard(int? division = null, int top = 10)
        {
            var query = Fighters.AsEnumerable();
            if (division.HasValue)
                query = query.Where(f => f.Division == division.Value);

            return query.OrderByDescending(f => f.Elo)
                        .Take(top)
                        .ToList();
        }
    }
}
