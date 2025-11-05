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

        public static void AddFighter(string _name, int _division)
        {
            if (Fighters.Any(f => f.Name.Equals(_name, StringComparison.OrdinalIgnoreCase))) return;

            Fighters.Add(new Fighter
            {
                Name = _name,
                Division = _division,
                Elo = 1000,
                PeakElo = 1000
            });

            SaveFighters();
        }

        public static Fighter? GetFighter(string _name)
        {
            return Fighters.FirstOrDefault(_f => _f.Name.Equals(_name, StringComparison.OrdinalIgnoreCase));
        }

        public static void UpdateFighter(Fighter _fighter)
        {
            _fighter.PeakElo = Math.Max(_fighter.PeakElo, _fighter.Elo);
            SaveFighters();
        }

        public static void RecordFight(string _winnerName, string _loserName)
        {
            var _winner = GetFighter(_winnerName);
            var _loser = GetFighter(_loserName);

            if (_winner == null || _loser == null || _winner == _loser) return;

            const int k = 32;

            double _expectedWinner = 1.0 / (1 + Math.Pow(10, (_loser.Elo - _winner.Elo) / 400.0));
            double _expectedLoser = 1.0 / (1 + Math.Pow(10, (_winner.Elo - _loser.Elo) / 400.0));

            int _winnerOldElo = _winner.Elo;
            int _loserOldElo = _loser.Elo;

            _winner.Elo = (int)(_winner.Elo + k * (1 - _expectedWinner));
            _loser.Elo = (int)(_loser.Elo + k * (0 - _expectedLoser));

            _winner.PeakElo = Math.Max(_winner.PeakElo, _winner.Elo);
            _loser.PeakElo = Math.Max(_loser.PeakElo, _loser.Elo);

            _winner.Wins++;
            _loser.Losses++;

            int _winnerEloGain = _winner.Elo - _winnerOldElo;
            int _loserEloLoss = _loserOldElo - _loser.Elo;

            if (_winnerEloGain > _winner.BiggestEloGain)
                _winner.BiggestEloGain = _winnerEloGain;

            if (_loserEloLoss > _loser.BiggestEloLoss)
                _loser.BiggestEloLoss = _loserEloLoss;

            SaveFighters();
        }

        public static List<Fighter> GetLeaderboard(int? _division = null, int _top = 10)
        {
            var _query = Fighters.AsEnumerable();
            if (_division.HasValue)
                _query = _query.Where(_f => _f.Division == _division.Value);

            return _query.OrderByDescending(_f => _f.Elo)
                        .Take(_top)
                        .ToList();
        }

        public static void SaveToFile()
        {
            SaveFighters();
        }

        public static void LoadFromFile()
        {
            LoadFighters();
            if (Fighters == null)
                Fighters = new List<Fighter>();
        }

        public static void ResetFighters()
        {
            FighterStorage.Reset();
            LoadFighters();
        }
        public static List<Fighter> GetAllFighters()
        {
            LoadFighters();
            return Fighters.ToList();
        }
    }
}
