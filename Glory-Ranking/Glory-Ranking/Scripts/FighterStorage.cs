using System.IO;
using System.Text.Json;

namespace Glory_Ranking
{
    public static class FighterStorage
    {
        private static string FilePath = "Fighters.json"; // same as your file

        public static void Save(FighterData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(FilePath, json);
        }

        public static FighterData Load()
        {
            if (!File.Exists(FilePath))
            {
                return new FighterData { Fighters = new Fighter[0] };
            }

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<FighterData>(json) ?? new FighterData { Fighters = new Fighter[0] };
        }

        public static void Reset()
        {
            var emptyData = new FighterData { Fighters = new Fighter[0] };
            Save(emptyData);
        }
    }
}
