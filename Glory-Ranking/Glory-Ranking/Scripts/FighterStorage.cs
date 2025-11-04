using System.IO;
using System.Text.Json;

namespace Glory_Ranking
{
    public static class FighterStorage
    {
        private static string FilePath = "Fighters.json";

        public static void Save(FighterData _data)
        {
            var _options = new JsonSerializerOptions { WriteIndented = true };
            string _json = JsonSerializer.Serialize(_data, _options);
            File.WriteAllText(FilePath, _json);
        }

        public static FighterData Load()
        {
            if (!File.Exists(FilePath))
            {
                return new FighterData { Fighters = new Fighter[0] };
            }

            string _json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<FighterData>(_json) ?? new FighterData { Fighters = new Fighter[0] };
        }

        public static void Reset()
        {
            var _emptyData = new FighterData { Fighters = new Fighter[0] };
            Save(_emptyData);
        }
    }
}
